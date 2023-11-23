using CateringManagement.Helper;
using CateringManagement.Models.DTO;
using CateringManagement.Models.Requests;
using CateringManagement.Repository;
using DAL.DomainClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetListUsers(int? role, string? searching)
        {
            var lstUser = await _userRepo.getLstUsers(role, searching);
            return Json(lstUser, new System.Text.Json.JsonSerializerOptions());
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetUserByEmployeeId(string employeeId)
        {
            var user = await _userRepo.GetUserEmployeeId(employeeId);
            return Json(user, new System.Text.Json.JsonSerializerOptions());
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromForm] UserCreateRequest userRequest, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var userSesion = HttpContext.Session.GetObjectFromJson<Users>("userLogin");

                var checkUser = _userRepo.GetUserByEmail(userRequest.Email);
                if (checkUser != null)
                {
                    return Json(new ResponseModel { Status = 2, Mess = "Email already exists" });
                }


                var maxid = Convert.ToInt32(await _userRepo.GetMaxEmployeeId()) + 1;
                Users user = new Users();
                user.EmployeeId = "Us" + maxid;
                user.FirstName = userRequest.FirstName;
                user.LastName = userRequest.LastName;
                user.Email = userRequest.Email;
                user.Password = userRequest.Password;
                user.DateOfBirth = userRequest.DateOfBirth;
                user.CreatedAt = DateTime.Now;
                user.Sex = userRequest.Sex;
                user.Status = userRequest.Status;
                user.Role = userRequest.Role;
                user.IsDeleted = 0;
                user.CreatedBy = userSesion.Id;
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
                    return Json(new ResponseModel { Status = 0, Mess = "Add Failure" });
                }
            }
            return Json(new ResponseModel { Status = 0, Mess = "Add Failure" });
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> EditUser([FromForm] UserEditRequest userRequest, IFormFile image)
        {
            if (userRequest.EmployeeId != null)
            {
                var user = await _userRepo.GetUserEmployeeId(userRequest.EmployeeId);
                if (user != null)
                {
                    var userSesion = HttpContext.Session.GetObjectFromJson<Users>("userLogin");
                    user.FirstName = userRequest.FirstName;
                    user.LastName = userRequest.LastName;
                    user.Email = userRequest.Email;
                    user.Password = userRequest.Password;
                    user.DateOfBirth = userRequest.DateOfBirth;
                    user.UpdatedAt = DateTime.Now;
                    user.Sex = userRequest.Sex;
                    user.Status = userRequest.Status;
                    user.Role = userRequest.Role;
                    user.CreatedBy = userSesion.Id;
                    if (image != null)
                    {
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

                    }

                    var result = await _userRepo.Update(user);
                    if (result > 0)
                    {


                        return Json(new ResponseModel { Status = 1, Mess = "Update Success" });
                    }
                    else
                    {
                        return Json(new ResponseModel { Status = 0, Mess = "Update Failure" });
                    }


                }
            }
            return Json(new ResponseModel { Status = 0, Mess = "Update Failure" });
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userRepo.DeleteUserByEmployeeId(userId);
            if (result > 0)
            {
                return Json(new ResponseModel { Status = 1, Mess = "Delete Success" });
            }
            else
            {
                return Json(new ResponseModel { Status = 0, Mess = "Delete Failure" });
            }

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
            if (changePasswordDTO.NewPassword == changePasswordDTO.CurrentPassword) return Json(new { result = 1 }); // pass mới đang trùng với hiện tại
            user.Password = changePasswordDTO.NewPassword;
            user.UpdatedAt = DateTime.Now;
            await _userRepo.Update(user);
            HttpContext.Session.SetString("userLogin", JsonConvert.SerializeObject(user));//update lại session
            return Json(new { result = 2 });
        }


    }
}
