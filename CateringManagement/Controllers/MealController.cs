using CateringManagement.Helper;
using CateringManagement.Models.DTO;
using CateringManagement.Models.Requests;
using CateringManagement.Repository;
using DAL.Context;
using DAL.DomainClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CateringManagement.Controllers
{
    public class MealController : Controller
    {
        private readonly ApplicationDbContext _context;

        MealsRepository _mealsRepo = new MealsRepository();
        IngredientsRepository _ingredientsRepo = new IngredientsRepository();

        public MealController()
        {
            _context = new ApplicationDbContext();
        }

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

        [HttpPost]
        public async Task<IActionResult> AddMeal([FromBody] MealCreateRequest request)
        {
            // validate
            if (string.IsNullOrEmpty(request.Name))
            {
                return Json(new ResponseModel { Status = 0, Mess = "Please enter the name" });
            }

            if (!request.Ingredients.Any())
            {
                return Json(new ResponseModel { Status = 0, Mess = "Please add the ingredient" });
            }

            var ingredientIds = request.Ingredients.Select(x => x.IngredientId).ToList();
            var ingredients = await _context.Ingredients.Where(x => ingredientIds.Contains(x.Id) && x.IsDeleted == 0).ToListAsync();
            if (ingredients.Count < ingredientIds.Count)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Ingredient not found" });
            }

            List<MealIngredients> ingredientData = new();
            decimal price = 0;
            foreach (var item in request.Ingredients)
            {
                var ingredient = ingredients.First(x => x.Id == item.IngredientId);
                ingredientData.Add(new MealIngredients
                {
                    IngredientId = ingredient.Id,
                    Quantity = item.Quantity
                });
                price += item.Quantity * ingredient.PriceUnit;
            }
            var newMeal = new Meals
            {
                Name = request.Name,
                Price = price,
                MealIngredients = ingredientData
            };

            try
            {
                await _mealsRepo.Create(newMeal);
                return Json(new ResponseModel { Status = 1, Mess = "Add meal successfully" });
            }
            catch (Exception)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Add meal failed" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDetail(Guid id)
        {
            var meal = await _context.Meals.Include(x => x.MealIngredients.Where(y => y.IsDeleted == 0))
                .ThenInclude(x => x.Ingredient)
                .Where(x => x.Id == id && x.IsDeleted == 0)
                .FirstOrDefaultAsync();
            if (meal == null)
            {
                return Json(new ResponseModel<MealDetailDTO> { Status = 0, Mess = "Meal not found" });
            }

            List<MealIngredientDTO> ingredientData = new();
            decimal totalPrice = 0;
            foreach (var item in meal.MealIngredients)
            {
                decimal price = item.Quantity * item.Ingredient.PriceUnit;
                ingredientData.Add(new MealIngredientDTO
                {
                    IngredientId = item.IngredientId,
                    Name = item.Ingredient.Name,
                    Unit = IngredientHelper.GetUnitName(item.Ingredient.Unit),
                    UnitPrice = item.Ingredient.PriceUnit,
                    Quantity = item.Quantity,
                    TotalPrice = item.Quantity * item.Ingredient.PriceUnit
                });
                totalPrice += price;
            }
            var data = new MealDetailDTO
            {
                Id = id,
                Name = meal.Name,
                Price = totalPrice,
                Ingredients = ingredientData
            };
            return Json(new ResponseModel<MealDetailDTO> { Status = 1, Data = data });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMeal([FromBody] MealUpdateRequest request, Guid id)
        {
            var meal = await _context.Meals.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0);
            if (meal == null)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Meal not found" });
            }
            var oldMealIngredients = await _context.MealIngredients.Where(x => x.MealId == id && x.IsDeleted == 0).ToListAsync();

            // validate
            if (string.IsNullOrEmpty(request.Name))
            {
                return Json(new ResponseModel { Status = 0, Mess = "Please enter the name" });
            }

            if (!request.Ingredients.Any())
            {
                return Json(new ResponseModel { Status = 0, Mess = "Please add the ingredient" });
            }

            var ingredientIds = request.Ingredients.Select(x => x.IngredientId).ToList();
            var ingredients = await _context.Ingredients.Where(x => ingredientIds.Contains(x.Id) && x.IsDeleted == 0).ToListAsync();
            if (ingredients.Count < ingredientIds.Count)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Ingredient not found" });
            }

            List<MealIngredients> ingredientData = new();
            decimal price = 0;
            foreach (var item in request.Ingredients)
            {
                var ingredient = ingredients.First(x => x.Id == item.IngredientId);
                ingredientData.Add(new MealIngredients
                {
                    IngredientId = ingredient.Id,
                    Quantity = item.Quantity
                });
                price += item.Quantity * ingredient.PriceUnit;
            }

            meal.Name = request.Name;
            meal.Price = price;
            meal.MealIngredients = null;

            try
            {
                await _mealsRepo.DeleteMealIngredientList(oldMealIngredients);
                await _mealsRepo.InsertMealIngredientList(ingredientData);
                await _mealsRepo.Update(meal);
                return Json(new ResponseModel { Status = 1, Mess = "Update meal successfully" });
            }
            catch (Exception e)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Update meal failed" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMeal(Guid id)
        {
            var meal = await _context.Meals.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0);
            if (meal == null)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Meal not found" });
            }

            meal.IsDeleted = 1;

            try
            {
                await _mealsRepo.Update(meal);
                return Json(new ResponseModel { Status = 1, Mess = "Delete meal successfully" });
            }
            catch (Exception e)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Delete meal failed" });
            }
        }
    }
}
