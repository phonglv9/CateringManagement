using CateringManagement.Models.DTO;
using CateringManagement.Models.Requests;
using CateringManagement.Repository;
using DAL.DomainClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;
using static System.Net.Mime.MediaTypeNames;

namespace CateringManagement.Controllers
{
    public class UsersController : Controller
    {
        private readonly IWebHostEnvironment _env;
        UsersRepository _userRepo = new UsersRepository();
        public UsersController(IWebHostEnvironment env)
        {
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task <IActionResult> GetListUsers()
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
                user.EmployeeId  = "Us" + maxid;
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
                var result =   await _userRepo.Create(user);
                if (result > 0)
                {
                    

                    return Json(new ResponseModel { Status = 1, Mess = "Add Success" });
                }
                else
                {
                    Json(new ResponseModel { Status = 0, Mess = "Add Failure" });
                }
            }
            return  Json(new ResponseModel { Status = 0, Mess = "Add Failure" });
        }
        

    }
}
