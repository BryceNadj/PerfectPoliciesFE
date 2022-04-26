using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PerfectPoliciesFE.Models;

namespace PerfectPoliciesFE.Controllers
{
    public class AuthController : Controller
    {
        private static string[] RouteValues;
        public IActionResult Login(string[] routeValues)
        {
            RouteValues = routeValues;
            try
            { InsertRouteValuesIntoViewBags(routeValues); }
            catch (Exception)
            { /* There are no RouteValues */
                ViewBag.Action = "Index";
                ViewBag.Controller = "Home";
            }
            
            return View(); 
        }

        [HttpPost]
        public IActionResult Login(UserInfo user)
        {
            string token = "";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44363/api/");

                var response = client.PostAsJsonAsync("Auth/GenerateToken", user).Result;

                if (response.IsSuccessStatusCode)
                {
                    // logged in
                    token = response.Content.ReadAsStringAsync().Result;

                    // Store the token in the session
                    HttpContext.Session.SetString("Token", token);
                }
                else
                {
                    // there was an issue logging in
                    ViewBag.Error = "The provided credentials were incorrect";
                    // potentially save a message to ViewBag and render in the view
                    return View();
                }
            }

            try
            { return RedirectToAction(RouteValues[0], RouteValues[1], new { id = RouteValues[2] }); }
            catch (Exception)
            { /* Id is not required for the requested view */ }

            try
            { return RedirectToAction(RouteValues[0], RouteValues[1]); }
            catch (Exception)
            { return RedirectToAction("Index", "Home"); }
        }

        public IActionResult Logout(string[] routeValues)
        {
            HttpContext.Session.Clear();

            try
            { return RedirectToAction(RouteValues[0], RouteValues[1], new { id = RouteValues[2] }); }
            catch (Exception)
            { /* Id is not required for the requested view */ }

            try
            { return RedirectToAction(RouteValues[0], RouteValues[1]); }
            catch (Exception)
            { return RedirectToAction("Index", "Home"); }
        }

        #region Extra Methods

        private void InsertRouteValuesIntoViewBags(string[] routeValues)
        {
            if (routeValues[0] == "null")
            // Has to be "null" (if there is no action) because "" gets nulled automatically in the method params, which moves routeValues[1] to routeValues[0] (kinda cringe)
            // As long as I don't have an action called "null" this will be fine
            {
                ViewBag.Action = null;
            }
            else
            {
                ViewBag.Action = routeValues[0];
            }

            ViewBag.Controller = routeValues[1];

            try
            {
                ViewBag.QuizId = routeValues[2];
            }
            catch (Exception)
            {
                // Id is not required for the requested view
            }
        }
        #endregion
    }
}
