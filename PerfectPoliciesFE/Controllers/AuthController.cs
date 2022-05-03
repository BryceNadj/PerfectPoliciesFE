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
        public IActionResult Login()
        {
            RouteValues = GetTempData();
            // SetupTempData(routeValues);
            
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

            //RouteValues = GetTempData();
            return Redirect();
        }

        public IActionResult Logout(string[] routeValues)
        {
            HttpContext.Session.Clear();

            // RouteValues = GetTempData();
            return Redirect();
        }

        #region Extra Methods
        private IActionResult Redirect()
        {
            if (TempData.Count.Equals(2)) // Action, Controller
            {
                return RedirectToAction(
                    TempData["Action"].ToString(), 
                    TempData["Controller"].ToString());
            }
            else if (TempData.Count.Equals(3)) // Action, Controller, QuizId
            {
                return RedirectToAction(
                    TempData["Action"].ToString(),
                    TempData["Controller"].ToString(), 
                    new { id = TempData["QuizId"].ToString() });
            }
            else if (TempData.Count.Equals(4)) // Action, Controller, QuizId, QuestionId
            {
                return RedirectToAction(
                    TempData["Action"].ToString(), 
                    TempData["Controller"].ToString(), 
                    new { quizId = TempData["QuizId"].ToString(), 
                        id = TempData["QuestionId"].ToString() });
            }

            return RedirectToAction("Index", "Home"); // Something happened so just go back to main front page
        }

        private void SetTempData(string[] routeValues)
        {
            TempData["Action"] = routeValues[0];
            TempData["Controller"] = routeValues[1];
            
            try
            { TempData["QuizId"] = routeValues[2]; }
            catch (Exception)
            { /* Id is not required for the requested view */ }
            
            try
            { TempData["QuestionId"] = routeValues[3]; }
            catch (Exception)
            { /* Id is not required for the requested view */ }

            TempData.Keep();
        }

        private string[] GetTempData()
        {
            string[] routeValues;
            if (TempData.Count.Equals(2))
            {
                routeValues = new string[] {
                    TempData["Action"].ToString(),
                    TempData["Controller"].ToString()
                };
            }
            else if (TempData.Count.Equals(3))
            { 
                routeValues = new string[] {
                    TempData["Action"].ToString(),
                    TempData["Controller"].ToString(),
                    TempData["QuizId"].ToString() 
                };
            }

            else if (TempData.Count.Equals(4))
            {
                routeValues = new string[] {
                    TempData["Action"].ToString(),
                    TempData["Controller"].ToString(),
                    TempData["QuizId"].ToString(),
                    TempData["QuestionId"].ToString() 
                };
            }
            else // Some kind of error happened
            { return null; }
            TempData.Keep();

            return routeValues;
        }
        #endregion
    }
}
