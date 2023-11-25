using CateringManagement.Helper;
using CateringManagement.Models.DTO;
using CateringManagement.Models.Requests;
using CateringManagement.Repository;
using DAL.Context;
using DAL.DomainClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CateringManagement.Controllers
{
    [Authorize(Roles = "admin,chef")]
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

        [HttpGet]
        public async Task<IActionResult> GetListCategories()
        {
            var data = await _context.MealCategories.Where(x => x.IsDeleted == 0)
                .Select(x => new MealCategoryDetailDTO
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
            return Json(new ResponseModel<List<MealCategoryDetailDTO>> { Status = 1, Data = data });
        }

        [HttpPost]
        public async Task<IActionResult> AddMeal([FromBody] MealCreateRequest request)
        {
            var userSesion = HttpContext.Session.GetObjectFromJson<Users>("userLogin");

            var category = await _context.MealCategories.FirstOrDefaultAsync(x => x.Id == request.CategoryId && x.IsDeleted == 0);
            if (category == null)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Category not found" });
            }

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

            var quantityCount = request.Ingredients.Count(x => x.Quantity > 0);
            if (quantityCount < ingredients.Count)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Invalid quantity" });
            }

            List<MealIngredients> ingredientData = new();
            decimal price = 0;
            foreach (var item in request.Ingredients)
            {
                var ingredient = ingredients.First(x => x.Id == item.IngredientId);
                ingredientData.Add(new MealIngredients
                {
                    IngredientId = ingredient.Id,
                    Quantity = item.Quantity,
                    CreatedBy = userSesion.Id,
                    UpdatedBy = userSesion.Id
                });
                price += item.Quantity * ingredient.PriceUnit;
            }
            var newMeal = new Meals
            {
                Name = request.Name,
                MealCategoryId = request.CategoryId,
                Price = price,
                Description = request.Description,
                Image = "",
                MealIngredients = ingredientData,
                CreatedBy = userSesion.Id,
                UpdatedBy = userSesion.Id
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
            var meal = await _context.Meals.Include(x => x.MealCategory)
                .Include(x => x.MealIngredients.Where(y => y.IsDeleted == 0))
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
                CategoryId = meal.MealCategoryId,
                CategoryName = meal.MealCategory.Name,
                Description = meal.Description,
                Ingredients = ingredientData,
            };
            return Json(new ResponseModel<MealDetailDTO> { Status = 1, Data = data });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMeal([FromBody] MealUpdateRequest request, Guid id)
        {
            var userSesion = HttpContext.Session.GetObjectFromJson<Users>("userLogin");

            var meal = await _context.Meals.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0);
            if (meal == null)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Meal not found" });
            }

            var category = await _context.MealCategories.FirstOrDefaultAsync(x => x.Id == request.CategoryId && x.IsDeleted == 0);
            if (category == null)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Category not found" });
            }

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

            var quantityCount = request.Ingredients.Count(x => x.Quantity > 0);
            if (quantityCount < ingredients.Count)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Invalid quantity" });
            }

            List<MealIngredients> ingredientData = new();
            var oldMealIngredients = await _context.MealIngredients.Where(x => x.MealId == id && x.IsDeleted == 0).ToListAsync();

            decimal price = 0;
            foreach (var item in request.Ingredients)
            {
                var ingredient = ingredients.First(x => x.Id == item.IngredientId);
                ingredientData.Add(new MealIngredients
                {
                    MealId = meal.Id,
                    IngredientId = ingredient.Id,
                    Quantity = item.Quantity,
                    CreatedBy = userSesion.Id,
                    UpdatedBy = userSesion.Id
                });
                price += item.Quantity * ingredient.PriceUnit;
            }

            meal.Name = request.Name;
            meal.MealCategoryId = request.CategoryId;
            meal.Price = price;
            meal.Description = request.Description;
            meal.UpdatedBy = userSesion.Id;

            try
            {
                await _mealsRepo.DeleteMealIngredientList(oldMealIngredients, userSesion.Id);
                await _mealsRepo.InsertMealIngredientList(ingredientData);
                await _mealsRepo.Update(meal);

                return Json(new ResponseModel { Status = 1, Mess = "Update meal successfully" });
            }
            catch (Exception)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Update meal failed" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMeal(Guid id)
        {
            var userSesion = HttpContext.Session.GetObjectFromJson<Users>("userLogin");

            var meal = await _context.Meals.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0);
            if (meal == null)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Meal not found" });
            }

            meal.IsDeleted = 1;
            meal.UpdatedBy = userSesion.Id;

            var mealIngredients = await _context.MealIngredients.Where(x => x.MealId == id && x.IsDeleted == 0).ToListAsync();

            try
            {
                await _mealsRepo.Update(meal);
                await _mealsRepo.DeleteMealIngredientList(mealIngredients, userSesion.Id);
                return Json(new ResponseModel { Status = 1, Mess = "Delete meal successfully" });
            }
            catch (Exception e)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Delete meal failed" });
            }
        }
    }
}
