using DAL.Context;
using Microsoft.AspNetCore.Mvc;

namespace CateringManagement.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController()
        {
            _context = new ApplicationDbContext();
        }

        public IActionResult Index(string? fromDate,string? toDate,string? sName,string? status)
        {
            var lstOrders = _context.Orders.ToList();
            if (sName != null)
            {
                lstOrders = lstOrders.Where(c => c.CustomerName.ToLower().Trim().Contains(sName) && c.IsDeleted == 0).ToList();
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

        

    }
}
