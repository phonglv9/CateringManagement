using DAL.Context;
using EntityFramework.DomainClass;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CateringManagement.Models.DTO;
using CateringManagement.Helper;

namespace CateringManagement.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("mess")))
                ViewData["Mess"] = HttpContext.Session.GetString("mess");
            HttpContext.Session.Remove("mess");
            ViewBag.returnUrl = returnUrl;
            return View();
        }


        [HttpPost]        
        public async Task<IActionResult> Index(UserInfoDTO user, string returnUrl = null)
        {

            try
            {
                #region Đăng nhập khi đã tạo đc user
                ViewBag.Error = "Đăng nhập không thành công! Vui lòng nhập lại thông tin đăng nhập!";
                //--Kiểm tra dữ liệu đầu vào
                var Employees = _context.Users.ToList();
                Users Emp = Employees.FirstOrDefault(c => c.Email.ToLower() == user.Email.ToLower() && c.Password == user.password && c.Status == 1);

                if (Emp != null)
                {
                    Commons.setObjectAsJson(HttpContext.Session, "userLogin", Emp);
                    var claims = new List<Claim>();
                    claims.Add(new Claim("username", Emp.LastName + Emp.FirstName));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, Emp.EmployeeId));
                    claims.Add(new Claim(ClaimTypes.Email, Emp.EmployeeId));
                    claims.Add(new Claim(ClaimTypes.Name, Emp.LastName + Emp.FirstName));
                    claims.Add(new Claim(ClaimTypes.Country, user.Image != null ? user.Image : ""));
                    if (Emp.Role == DAL.Enums.UserPosition.Admin)
                    {
                        HttpContext.Session.SetString("myRole", "admin");
                        claims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }
                    else if(Emp.Role == DAL.Enums.UserPosition.Storage)
                    {
                        HttpContext.Session.SetString("myRole", "storage");
                        claims.Add(new Claim(ClaimTypes.Role, "storage"));
                    }
                    else if (Emp.Role == DAL.Enums.UserPosition.Chef)
                    {
                        HttpContext.Session.SetString("myRole", "chef");
                        claims.Add(new Claim(ClaimTypes.Role, "chef"));
                    }
                    else
                    {
                        HttpContext.Session.SetString("myRole", "reception");
                        claims.Add(new Claim(ClaimTypes.Role, "reception"));
                    }
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) && !returnUrl.NullToString().Contains("Account"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
               
                HttpContext.Session.SetString("mess", "Failed");
                return RedirectToAction("Index", "Login", new { returnUrl = returnUrl });
                #endregion
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
