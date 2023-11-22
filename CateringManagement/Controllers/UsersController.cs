using CateringManagement.Helper;
using CateringManagement.Models.DTO;
using CateringManagement.Models.Requests;
using CateringManagement.Repository;
using DAL.Context;
using DAL.DomainClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Runtime.ConstrainedExecution;
using static System.Net.Mime.MediaTypeNames;

namespace CateringManagement.Controllers
{
    public class UsersController : Controller
    {
        private readonly IWebHostEnvironment _env;
        UsersRepository _userRepo = new UsersRepository();
        private readonly ApplicationDbContext _context;
        public UsersController(IWebHostEnvironment env)
        {
            _env = env;
            _context = new ApplicationDbContext();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetListUsers()
        {
            var lstUser = await _userRepo.getLstUsers();
            return Json(lstUser, new System.Text.Json.JsonSerializerOptions());
        }
        [HttpPost]
        public async Task<IActionResult> AddUser([FromForm] UserCreateRequest userRequest, IFormFile image)
        {
            if (ModelState.IsValid)
            {

                var maxid = Convert.ToInt32(await _userRepo.GetMaxEmployeeId()) + 1;
                Users user = new Users();
                user.EmployeeId = "Us" + maxid;
                user.FirstName = userRequest.FirstName;
                user.LastName = userRequest.LastName;
                user.Email = userRequest.Email;
                user.Password = userRequest.Password;
                user.Sex = userRequest.Sex;
                user.Status = userRequest.Status;
                user.Role = userRequest.Role;
                user.IsDeleted = 1;
                var uploads = Path.Combine(_env.WebRootPath, "admin/assets/img");

                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                string extension = Path.GetExtension(image.FileName);
                var fileName = user.EmployeeId + extension;
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                user.Image = fileName;
                var result = await _userRepo.Create(user);
                if (result > 0)
                {


                    return Json(new ResponseModel { Status = 1, Mess = "Add Success" });
                }
                else
                {
                    Json(new ResponseModel { Status = 0, Mess = "Add Failure" });
                }
            }
            return Json(new ResponseModel { Status = 0, Mess = "Add Failure" });
        }

        [HttpGet]
        public async Task<ActionResult> ChangePassword()
        {
            var user = HttpContext.Session.GetObjectFromJson<Users>("userLogin");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var user = HttpContext.Session.GetObjectFromJson<Users>("userLogin");
            if (user == null) return Json(new { result = 3 }); // user chưa đăng nhập
            if (changePasswordDTO.CurrentPassword != user.Password) return Json(new { result = 0 }); // pass hiện tại ko đúng
            if(changePasswordDTO.NewPassword == changePasswordDTO.CurrentPassword) return Json(new { result = 1 }); // pass mới đang trùng với hiện tại
            user.Password = changePasswordDTO.NewPassword;
            user.UpdatedAt = DateTime.Now;
            _context.Users.Update(user);
            _context.SaveChanges();
            HttpContext.Session.SetString("userLogin", JsonConvert.SerializeObject(user));//update lại session
            return Json(new { result = 2 });
        }


    }
}
