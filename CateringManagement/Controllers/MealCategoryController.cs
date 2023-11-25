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
    public class MealCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        MealCategoriesRepository _mealCategoriesRepo = new MealCategoriesRepository();

        public MealCategoryController()
        {
            _context = new ApplicationDbContext();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetListMealCategories()
        {
            var list = await _mealCategoriesRepo.GetAllData();
            var data = list.Select(x => new MealCategoryDTO
            {
                Id = x.Id,
                Name = x.Name,
                CanDelete = x.Meals != null && x.Meals.Any() ? 0 : 1,
            }).ToList();
            return Json(data, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpPost]
        public async Task<IActionResult> AddMealCategory([FromBody] MealCategoryCreateRequest request)
        {
            var userSesion = HttpContext.Session.GetObjectFromJson<Users>("userLogin");

            // validate
            if (string.IsNullOrEmpty(request.Name))
            {
                return Json(new ResponseModel { Status = 0, Mess = "Please enter the name" });
            }

            var newMealCategory = new MealCategories
            {
                Name = request.Name,
                CreatedBy = userSesion.Id,
                UpdatedBy = userSesion.Id
            };

            try
            {
                await _mealCategoriesRepo.Create(newMealCategory);
                return Json(new ResponseModel { Status = 1, Mess = "Add meal category successfully" });
            }
            catch (Exception)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Add meal category failed" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDetail(Guid id)
        {
            var category = await _context.MealCategories.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0);
            if (category == null)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Category not found" });
            }

            var data = new MealCategoryDetailDTO
            {
                Id = id,
                Name = category.Name,
            };
            return Json(new ResponseModel<MealCategoryDetailDTO> { Status = 1, Data = data });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMealCategory([FromBody] MealCategoryUpdateRequest request, Guid id)
        {
            var userSesion = HttpContext.Session.GetObjectFromJson<Users>("userLogin");

            var category = await _context.MealCategories.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0);
            if (category == null)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Category not found" });
            }

            // validate
            if (string.IsNullOrEmpty(request.Name))
            {
                return Json(new ResponseModel { Status = 0, Mess = "Please enter the name" });
            }

            category.Name = request.Name;
            category.UpdatedBy = userSesion.Id;
            category.UpdatedAt = DateTime.Now;

            try
            {
                await _mealCategoriesRepo.Update(category);
                return Json(new ResponseModel { Status = 1, Mess = "Update meal category successfully" });
            }
            catch (Exception)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Update meal category failed" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMealCategory(Guid id)
        {
            var userSesion = HttpContext.Session.GetObjectFromJson<Users>("userLogin");

            var category = await _context.MealCategories.Include(x => x.Meals).FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0);
            if (category == null)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Category not found" });
            }

            if (category.Meals.Any())
            {
                return Json(new ResponseModel { Status = 0, Mess = "Category can not be deleted" });
            }

            category.IsDeleted = 1;
            category.UpdatedBy = userSesion.Id;
            category.UpdatedAt = DateTime.Now;

            try
            {
                await _mealCategoriesRepo.Update(category);
                return Json(new ResponseModel { Status = 1, Mess = "Delete meal category successfully" });
            }
            catch (Exception)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Delete meal category failed" });
            }
        }
    }
}
