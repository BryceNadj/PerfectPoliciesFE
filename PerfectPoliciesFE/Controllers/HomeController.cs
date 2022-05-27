using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PerfectPoliciesFE.Helpers;
using PerfectPoliciesFE.Models;

namespace PerfectPoliciesFE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RouteValuesHelper _routeValuesHelper;

        public HomeController(ILogger<HomeController> logger, RouteValuesHelper routeValuesHelper)
        {
            _logger = logger;
            _routeValuesHelper = routeValuesHelper;
        }

        /// <summary>
        /// Goes to the home page
        /// </summary>
        public IActionResult Index()
        {
            _routeValuesHelper.SetupRouteValues("Index", "Home");
            return View();
        }

        /// <summary>
        /// Goes to the privacy page
        /// </summary>
        public IActionResult Privacy()
        {
            _routeValuesHelper.SetupRouteValues("Privacy", "Home");
            return View();
        }

        /// <summary>
        /// Goes to the FAQ page
        /// </summary>
        public IActionResult Help()
        {
            _routeValuesHelper.SetupRouteValues("Help", "Home");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
