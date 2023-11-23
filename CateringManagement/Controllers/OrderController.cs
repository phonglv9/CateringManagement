using CateringManagement.Models.DTO;
using CateringManagement.Repository;
using DAL.Context;
using DAL.DomainClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CateringManagement.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        OrdersRepository _ordersRepo = new OrdersRepository();
        MealsRepository _mealsRepo = new MealsRepository();

        public OrderController()
        {
            _context = new ApplicationDbContext();
        }

        public IActionResult Index1(string? fromDate,string? toDate,string? sName,string? status)
        {
            var lstOrders = _context.Orders.ToList();
            if (sName != null)
            {
                lstOrders = lstOrders.Where(c => c.OrderCode.ToLower().Contains(sName.ToLower()) && c.IsDeleted == 0).ToList();
            }
            if (status == "0" || status == "1")
            {
                lstOrders = lstOrders.Where(c => ((int)c.Status) == Convert.ToInt32(status) && c.IsDeleted == 0).ToList();
            }
            if (fromDate != null && toDate == null)
            {
                lstOrders = lstOrders.Where(c => c.CreatedAt >= DateTime.Parse(fromDate) && c.IsDeleted == 0).ToList();
            }
            if (fromDate == null && toDate != null)
            {
                lstOrders = lstOrders.Where(c => c.CreatedAt <= DateTime.Parse(toDate) && c.IsDeleted == 0).ToList();
            }
            if (fromDate != null && toDate != null)
            {
                lstOrders = lstOrders.Where(c => c.CreatedAt <= DateTime.Parse(toDate) && c.CreatedAt <= DateTime.Parse(toDate) && c.IsDeleted == 0).ToList();
            }

            this.ViewData[nameof(sName)] = (object)sName;
            this.ViewData[nameof(status)] = (object)status;
            this.ViewData[nameof(fromDate)] = (object)fromDate;
            this.ViewData[nameof(toDate)] = (object)toDate;

            return View(lstOrders);
        }

        [HttpPost]
        public async Task<IActionResult> Deleted(string Id)
        {
            Orders order = await _context.Orders.FirstOrDefaultAsync(c=>c.Id == Guid.Parse(Id) && c.IsDeleted == 0);
            if(order == null) return Json(new { result = 0 });
            order.IsDeleted = 1;
            _context.Orders.Update(order);
            _context.SaveChanges();
            return Json(new { result = 1 });
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetListOrders()
        {
            var data = await _ordersRepo.GetAllData();
            return Json(data, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpGet]
        public async Task<IActionResult> GetSimpleListMeals()
        {
            var data = await _mealsRepo.GetAllData();
            return Json(new ResponseModel<List<MealDTO>> { Status = 1, Data = data });
        }
    }
}
