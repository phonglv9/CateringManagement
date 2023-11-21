using CateringManagement.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CateringManagement.Controllers
{
    public class UsersController : Controller
    {
        UsersRepository _userRepo = new UsersRepository();
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
    }
}
