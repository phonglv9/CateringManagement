using CateringManagement.Helper;
using CateringManagement.Models.DTO;
using CateringManagement.Repository;
using DAL.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CateringManagement.Controllers
{
    [Authorize(Roles = "admin,chef,reception")]
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuController()
        {
            _context = new ApplicationDbContext();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetListMeals(string keyword, Guid? categoryId)
        {
            var query = _context.Meals.Include(x => x.MealCategory).Where(x => x.IsDeleted == 0);

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(keyword));
            }

            if (categoryId != null)
            {
                query = query.Where(x => x.MealCategoryId == categoryId);
            }

            var data = await query.Select(x => new MealDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = decimal.Round(x.Price / (decimal)0.35, 2),
                    Category = x.MealCategory.Name,
                    Description = x.Description,
                    Image = MealHelper.GetMealImageSrc(x.Image)
                })
                .ToListAsync();
            return Json(new ResponseModel<List<MealDTO>> { Status = 1, Data = data });
        }

        [HttpGet]
        public async Task<IActionResult> GetListCategories()
        {
            var data = await _context.MealCategories.Include(x => x.Meals.Where(y => y.IsDeleted == 0))
                .Where(x => x.IsDeleted == 0)
                .Select(x => new MealCategoryWithCountDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    MealNumber = x.Meals.Count()
                })
                .ToListAsync();
            return Json(new ResponseModel<List<MealCategoryWithCountDTO>> { Status = 1, Data = data });
        }
    }
}
