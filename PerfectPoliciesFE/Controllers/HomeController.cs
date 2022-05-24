using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PerfectPoliciesFE.Models;

namespace PerfectPoliciesFE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            SetupTempData("Index", "Home");
            return View();
        }

        public IActionResult Privacy()
        {
            SetupTempData("Privacy", "Home");
            return View();
        }
        
        public IActionResult Help()
        {
            SetupTempData("Help", "Home");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void SetupTempData(string action, string controller)
        {
            TempData.Clear();
            TempData["Action"] = action;
            TempData["Controller"] = controller;
            TempData.Keep();
        }
    }
}
