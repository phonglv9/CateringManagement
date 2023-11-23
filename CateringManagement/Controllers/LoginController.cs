using CateringManagement.Helper;
using CateringManagement.Models.DTO;
using CateringManagement.Repository;
using DAL.Context;
using DAL.DomainClass;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;


namespace CateringManagement.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        UsersRepository _userRepo = new UsersRepository();

        public LoginController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("mess")))
                ViewData["Mess"] = HttpContext.Session.GetString("mess");
            HttpContext.Session.Remove("mess");
            ViewBag.returnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserInfoDTO user)
        {
            try
            {
                #region Đăng nhập khi đã tạo đc user
                //--Kiểm tra dữ liệu đầu vào
                if (String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.password))
                {
                    return Json(new { result = 3 });// Không để rỗng
                }
                var Employees = _context.Users.ToList();
                Users Emp = Employees.FirstOrDefault(c => c.Email.ToLower() == user.Email.ToLower() && c.Password == user.password && c.Status == 1);

                if (Emp == null)
                {
                    return Json(new { result = 0 });// Không thành công
                }

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
                    else if (Emp.Role == DAL.Enums.UserPosition.Storage)
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
                    return Json(new { result = 1 });//Thành công
                }

                HttpContext.Session.SetString("mess", "Failed");
                return Json(new { result = 2 }); // Lỗi
                #endregion
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize(Roles = "admin, storage, chef, reception")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("userLogin");
            HttpContext.Session.Remove("myRole");
            return Redirect("/");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userRepo.GetUserEmail(email);
            if (user == null) return Json(new { result = 0 });

            #region Lưu chức vụ
            string role = "";
            if (user.Role == DAL.Enums.UserPosition.Admin)
            {
                role = "Admin";
            }
            else if (user.Role == DAL.Enums.UserPosition.Storage)
            {
                role = "Storage";
            }
            else if (user.Role == DAL.Enums.UserPosition.Chef)
            {
                role = "Chef";
            }
            else
            {
                role = "Reception";
            }
            #endregion
            var codeToken = GenerateRandomCode(6);
            user.Password = codeToken;
            await _userRepo.Update(user);
            #region Subject and body
            var subject = "Reset your password";
            var body = $@"
                            <html>
                            <head>
                                <style>
                                    body {{
                                        font-family: Arial, sans-serif;
                                        margin: 0;
                                        padding: 0;
                                    }}
                                    .container {{
                                        max-width: 600px;
                                        margin: 0 auto;
                                        padding: 20px;
                                        background-color: #f7f7f7;
                                    }}
                                    .header {{
                                        background-color: #3498db;
                                        color: #fff;
                                        text-align: center;
                                        padding: 10px 0;
                                    }}
                                    .content {{
                                        background-color: #fff;
                                        padding: 20px;
                                        border-radius: 5px;
                                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                                    }}
                                    .footer {{
                                        text-align: center;
                                        margin-top: 20px;
                                        color: #888;
                                    }}
                                </style>
                            </head>
                            <body>
                                <div class='container'>
                                    <div class='header'>
                                        <h1>Reset your password!</h1>
                                    </div>
                                    <div class='content'>
                                        <p>Hi, {user.LastName + " " + user.FirstName}!</p>
                                        <p>Thank you for responding. We have received your information about forgotten password.</p>
                                        <p>We have supported changing passwords:</p>
                                        <ul>
                                            <li><strong>Position:</strong> {role}</li>
                                            <li><strong>Email:</strong> {user.Email}</li>
                                            <li><strong>New password:</strong> {codeToken}</li>
                                        </ul>
                                        <p>Please contact us if changing your password is not successful.</p>
                                        <p>Thank you and have a nice day!</p>
                                    </div>
                                    <div class='footer'>
                                        <p>© 2023 Your Company. All rights reserved.</p>
                                    </div>
                                </div>
                            </body>
                            </html>";
            #endregion

            if (SendMail(email, subject, body))
            {
                return Json(new { result = 1 });//Gửi mail thành công
            }
            else
            {
                return Json(new { result = 2 });//Gửi không thành công
            }
        }

        private string GenerateRandomCode(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public bool SendMail(string to, string subject, string body)
        {
            try
            {
                string fromEmail = "cateringmanagement2023@gmail.com";
                string password = "hqlo uqlv qjmb wzzm";
                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromEmail);
                message.Subject = subject;
                message.To.Add(new MailAddress(to));
                message.Body = body;
                message.IsBodyHtml = true;
                var smtpClient = new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential(fromEmail, password),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                };
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
