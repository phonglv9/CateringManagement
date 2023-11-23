using CateringManagement.Models.DTO;
using CateringManagement.Models.Requests;
using CateringManagement.Repository;
using DAL.DomainClass;
using Microsoft.AspNetCore.Mvc;

namespace CateringManagement.Controllers
{
    public class MealController : Controller
    {
        MealsRepository _mealsRepo = new MealsRepository();
        IngredientsRepository _ingredientsRepo = new IngredientsRepository();

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetListMeals()
        {
            var data = await _mealsRepo.GetAllData();
            return Json(data, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpGet]
        public async Task<IActionResult> GetSimpleListIngredients()
        {
            var list = await _ingredientsRepo.GetAllData();
            var data = list.Select(x => new SimpleIngredientDTO
            {
                Id = x.Id,
                Name = x.Name,
                UnitPrice = x.UnitPrice,
                Unit = x.Unit
            }).ToList();
            return Json(new ResponseModel<List<SimpleIngredientDTO>> { Status = 1, Data = data });
        }

        //[HttpPost]
        //public async Task<IActionResult> AddMeal([FromBody] MealCreateRequest request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Json(new ResponseModel { Status = 0, Mess = "Invalid request" });
        //    }

        //    var ingredient = new Ingredients
        //    {
        //        Name = request.Name,
        //        Unit = request.Unit,
        //        PriceUnit = request.PriceUnit
        //    };

        //    try
        //    {
        //        await _ingredientsRepo.Create(ingredient);
        //        return Json(new ResponseModel { Status = 1, Mess = "Add ingredient successfully" });
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new ResponseModel { Status = 0, Mess = "Add ingredient failed" });
        //    }
        //}
    }
}
