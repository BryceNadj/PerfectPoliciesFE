using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PerfectPoliciesFE.Models;
using PerfectPoliciesFE.Services;

namespace PerfectPoliciesFE.Controllers
{
    public class AuthController : Controller
    {
        private readonly IApiRequest<UserInfo> _apiRequest;

        public AuthController(IApiRequest<UserInfo> apiRequest)
        {
            _apiRequest = apiRequest;
        }

        // GET: AuthController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserInfo userInfo)
        {
            try
            {
                _apiRequest.Create("Auth", userInfo);

                return Login(userInfo);
            }
            catch
            {
                return RedirectIActionResult();
            }
        }

        public IActionResult Login()
        {
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

            return RedirectIActionResult();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectIActionResult();
        }

        #region Extra Methods
        private IActionResult RedirectIActionResult()
        {
            TempData.Keep();
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
        
        private ActionResult RedirectActionResult()
        {
            TempData.Keep();
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
        #endregion
    }
}
