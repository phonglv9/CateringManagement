using CateringManagement.Models.DTO;
using CateringManagement.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CateringManagement.Controllers
{
    public class IngredientController : Controller
    {
        IngredientsRepository _ingredientsRepo = new IngredientsRepository();

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetListIngredients()
        {
            var data = await _ingredientsRepo.GetAllData();
            return Json(data, new System.Text.Json.JsonSerializerOptions());
        }
    }
}
