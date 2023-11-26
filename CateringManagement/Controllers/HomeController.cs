using CateringManagement.Models;
using CateringManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace CateringManagement.Controllers
{
    [Authorize(Roles = "admin,storage,chef,reception")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ReportRepository _reportRepo = new ReportRepository();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task< IActionResult> GetReportData(DateTime dateStart, DateTime dateEnd)
        {
 
            var reports = await _reportRepo.GetDataReport(dateStart, dateEnd);

            return Json(reports);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}