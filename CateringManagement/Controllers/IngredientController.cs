using CateringManagement.Models.DTO;
using CateringManagement.Models.Requests;
using CateringManagement.Repository;
using DAL.DomainClass;
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

        [HttpPost]
        public async Task<IActionResult> AddIngredient([FromBody] IngredientCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Invalid request" });
            }

            var ingredient = new Ingredients
            {
                Name = request.Name,
                Unit = request.Unit,
                PriceUnit = request.PriceUnit
            };

            var result = await _ingredientsRepo.Create(ingredient);
            if (result == 0)
            {

                return Json(new ResponseModel { Status = 0, Mess = "Add ingredient failed" });
                
            }
            return Json(new ResponseModel { Status = 1, Mess = "Add ingredient Successfully" });
        }
    }
}
