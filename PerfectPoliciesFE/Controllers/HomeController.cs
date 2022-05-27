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

        /// <summary>
        /// Goes to the home page
        /// </summary>
        public IActionResult Index()
        {
            SetupTempData("Index", "Home");
            return View();
        }

        /// <summary>
        /// Goes to the privacy page
        /// </summary>
        public IActionResult Privacy()
        {
            SetupTempData("Privacy", "Home");
            return View();
        }

        /// <summary>
        /// Goes to the FAQ page
        /// </summary>
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

        /// <summary>
        /// Sets up the name of the action and controller in TempData so any view can redirect to the right action if it needs to
        /// </summary>
        /// <param name="action">The name of the action</param>
        /// <param name="controller">The name of the controller</param>
        private void SetupTempData(string action, string controller)
        {
            TempData.Clear();
            TempData["Action"] = action;
            TempData["Controller"] = controller;
            TempData.Keep();
        }
    }
}
