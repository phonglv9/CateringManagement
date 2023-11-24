using CateringManagement.Common;
using CateringManagement.Helper;
using CateringManagement.Models.DTO;
using CateringManagement.Models.Requests;
using CateringManagement.Repository;
using DAL.Context;
using DAL.DomainClass;
using DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace CateringManagement.Controllers
{
    [Authorize(Roles = "admin,chef,reception")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        OrdersRepository _ordersRepo = new OrdersRepository();
        MealsRepository _mealsRepo = new MealsRepository();

        public OrderController()
        {
            _context = new ApplicationDbContext();
        }

        //public IActionResult Index1(string? fromDate,string? toDate,string? sName,string? status)
        //{
        //    var lstOrders = _context.Orders.ToList();
        //    if (sName != null)
        //    {
        //        lstOrders = lstOrders.Where(c => c.OrderCode.ToLower().Contains(sName.ToLower()) && c.IsDeleted == 0).ToList();
        //    }
        //    if (status == "0" || status == "1")
        //    {
        //        lstOrders = lstOrders.Where(c => ((int)c.Status) == Convert.ToInt32(status) && c.IsDeleted == 0).ToList();
        //    }
        //    if (fromDate != null && toDate == null)
        //    {
        //        lstOrders = lstOrders.Where(c => c.CreatedAt >= DateTime.Parse(fromDate) && c.IsDeleted == 0).ToList();
        //    }
        //    if (fromDate == null && toDate != null)
        //    {
        //        lstOrders = lstOrders.Where(c => c.CreatedAt <= DateTime.Parse(toDate) && c.IsDeleted == 0).ToList();
        //    }
        //    if (fromDate != null && toDate != null)
        //    {
        //        lstOrders = lstOrders.Where(c => c.CreatedAt <= DateTime.Parse(toDate) && c.CreatedAt <= DateTime.Parse(toDate) && c.IsDeleted == 0).ToList();
        //    }

        //    this.ViewData[nameof(sName)] = (object)sName;
        //    this.ViewData[nameof(status)] = (object)status;
        //    this.ViewData[nameof(fromDate)] = (object)fromDate;
        //    this.ViewData[nameof(toDate)] = (object)toDate;

        //    return View(lstOrders);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Deleted(string Id)
        //{
        //    Orders order = await _context.Orders.FirstOrDefaultAsync(c=>c.Id == Guid.Parse(Id) && c.IsDeleted == 0);
        //    if(order == null) return Json(new { result = 0 });
        //    order.IsDeleted = 1;
        //    _context.Orders.Update(order);
        //    _context.SaveChanges();
        //    return Json(new { result = 1 });
        //}

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

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderCreateRequest request)
        {
            var userSesion = HttpContext.Session.GetObjectFromJson<Users>("userLogin");

            // validate
            if (string.IsNullOrEmpty(request.CustomerName))
            {
                return Json(new ResponseModel { Status = 0, Mess = "Please enter the customer name" });
            }
            if (string.IsNullOrEmpty(request.CustomerPhone))
            {
                return Json(new ResponseModel { Status = 0, Mess = "Please enter the customer phone" });
            }

            if (!request.Meals.Any())
            {
                return Json(new ResponseModel { Status = 0, Mess = "Please add the meal" });
            }

            var mealIds = request.Meals.Select(x => x.MealId).ToList();
            var meals = await _context.Meals.Where(x => mealIds.Contains(x.Id) && x.IsDeleted == 0).ToListAsync();
            if (meals.Count < mealIds.Count)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Meal not found" });
            }

            List<OrderDetail> mealData = new();
            decimal price = 0;
            foreach (var item in request.Meals)
            {
                var meal = meals.First(x => x.Id == item.MealId);
                var mealTotalPrice = item.Quantity * meal.Price;
                mealData.Add(new OrderDetail
                {
                    MealId = meal.Id,
                    Quantity = item.Quantity,
                    TotalPrice = mealTotalPrice,
                    CreatedBy = userSesion.Id,
                    UpdatedBy = userSesion.Id
                });
                price += mealTotalPrice;
            }
            var newOrder = new Orders
            {
                OrderCode = Commons.GenerateRandomCode(6),
                CustomerName = request.CustomerName,
                CustomerPhone = request.CustomerPhone,
                Status = OrderStatus.InProgress,
                PickupTime = request.PickupDate,
                TotalPrice = decimal.Round(price, 2),
                SellPrice = decimal.Round(price / (decimal)0.35, 2),
                OrderDetails = mealData,
                CreatedBy = userSesion.Id,
                UpdatedBy = userSesion.Id
            };

            try
            {
                await _ordersRepo.Create(newOrder);
                return Json(new ResponseModel { Status = 1, Mess = "Add order successfully" });
            }
            catch (Exception)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Add order failed" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDetail(Guid id)
        {
            var order = await _context.Orders.Include(x => x.OrderDetails.Where(y => y.IsDeleted == 0))
                .ThenInclude(x => x.Meal)
                .Where(x => x.Id == id && x.IsDeleted == 0)
                .FirstOrDefaultAsync();
            if (order == null)
            {
                return Json(new ResponseModel<OrderDetailDTO> { Status = 0, Mess = "Order not found" });
            }

            List<OrderMealDTO> mealData = new();
            foreach (var item in order.OrderDetails)
            {
                mealData.Add(new OrderMealDTO
                {
                    MealId = item.MealId,
                    Name = item.Meal.Name,
                    Quantity = item.Quantity,
                    Price = item.Meal.Price,
                    TotalPrice = item.TotalPrice
                });
            }
            var data = new OrderDetailDTO
            {
                Id = id,
                Code = order.OrderCode,
                CustomerName = order.CustomerName,
                CustomerPhone = order.CustomerPhone,
                Status = OrderHelper.GetOrderStatus(order.Status),
                CreatedTime = TextUtils.ConvertDateTimeToString(order.CreatedAt),
                PickupDate = TextUtils.ConvertDateToString(order.PickupTime),
                PickupDateDT = order.PickupTime,
                TotalPrice = decimal.Round(order.TotalPrice, 2),
                SellPrice = decimal.Round(order.SellPrice, 2),
                Meals = mealData
            };
            return Json(new ResponseModel<OrderDetailDTO> { Status = 1, Data = data });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderUpdateRequest request, Guid id)
        {
            var userSesion = HttpContext.Session.GetObjectFromJson<Users>("userLogin");

            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0);
            if (order == null)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Order not found" });
            }

            // validate
            if (string.IsNullOrEmpty(request.CustomerName))
            {
                return Json(new ResponseModel { Status = 0, Mess = "Please enter the customer name" });
            }
            if (string.IsNullOrEmpty(request.CustomerPhone))
            {
                return Json(new ResponseModel { Status = 0, Mess = "Please enter the customer phone" });
            }

            if (!request.Meals.Any())
            {
                return Json(new ResponseModel { Status = 0, Mess = "Please add the meal" });
            }

            var mealIds = request.Meals.Select(x => x.MealId).ToList();
            var meals = await _context.Meals.Where(x => mealIds.Contains(x.Id) && x.IsDeleted == 0).ToListAsync();
            if (meals.Count < mealIds.Count)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Meal not found" });
            }

            List<OrderDetail> mealData = new();
            var oldOrderMeals = await _context.OrderDetails.Where(x => x.OrderId == id && x.IsDeleted == 0).ToListAsync();

            decimal price = 0;
            foreach (var item in request.Meals)
            {
                var meal = meals.First(x => x.Id == item.MealId);
                var mealTotalPrice = item.Quantity * meal.Price;
                mealData.Add(new OrderDetail
                {
                    OrderId = order.Id,
                    MealId = meal.Id,
                    Quantity = item.Quantity,
                    TotalPrice = mealTotalPrice,
                    CreatedBy = userSesion.Id,
                    UpdatedBy = userSesion.Id
                });
                price += mealTotalPrice;
            }

            order.CustomerName = request.CustomerName;
            order.CustomerPhone = request.CustomerPhone;
            order.PickupTime = request.PickupDate;
            order.TotalPrice = decimal.Round(price, 2);
            order.SellPrice = decimal.Round(price / (decimal)0.35, 2);
            order.UpdatedBy = userSesion.Id;

            try
            {
                await _ordersRepo.DeleteOrderMealList(oldOrderMeals, userSesion.Id);
                await _ordersRepo.InsertOrderMealList(mealData);
                await _ordersRepo.Update(order);

                return Json(new ResponseModel { Status = 1, Mess = "Update order successfully" });
            }
            catch (Exception)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Update order failed" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var userSesion = HttpContext.Session.GetObjectFromJson<Users>("userLogin");

            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0);
            if (order == null)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Order not found" });
            }

            order.IsDeleted = 1;
            order.UpdatedBy = userSesion.Id;

            var orderDetails = await _context.OrderDetails.Where(x => x.OrderId == id && x.IsDeleted == 0).ToListAsync();

            try
            {
                await _ordersRepo.Update(order);
                await _ordersRepo.DeleteOrderMealList(orderDetails, userSesion.Id);
                return Json(new ResponseModel { Status = 1, Mess = "Delete order successfully" });
            }
            catch (Exception e)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Delete order failed" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrder(Guid id)
        {
            var userSesion = HttpContext.Session.GetObjectFromJson<Users>("userLogin");

            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0);
            if (order == null)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Order not found" });
            }

            order.Status = OrderStatus.Done;
            order.UpdatedBy = userSesion.Id;

            try
            {
                await _ordersRepo.Update(order);
                return Json(new ResponseModel { Status = 1, Mess = "Complete order successfully" });
            }
            catch (Exception e)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Complete order failed" });
            }
        }
    }
}
