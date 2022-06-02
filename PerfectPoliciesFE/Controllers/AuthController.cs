using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PerfectPoliciesFE.Models;
using PerfectPoliciesFE.Services;
using PerfectPoliciesFE.Helpers;

namespace PerfectPoliciesFE.Controllers
{
    public class AuthController : Controller
    {
        HttpClient _client;
        private readonly IApiRequest<UserInfo> _apiRequest;
        private readonly RouteValuesHelper _routeValuesHelper;

        public AuthController(IHttpClientFactory factory, IApiRequest<UserInfo> apiRequest, RouteValuesHelper routeValuesHelper)
        {
            _client = factory.CreateClient("ApiClient");
            _apiRequest = apiRequest;
            _routeValuesHelper = routeValuesHelper;
        }

        // GET: AuthController/Create
        /// <returns>Create user account view</returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthController/Create
        /// <summary>
        /// Creates an account for the user and logs them in
        /// </summary>
        /// <param name="userInfo">The username and password to make the account with</param>
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
                ViewBag.Error = "Username already exists";
                return View();
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Logs the user in and generates a session token
        /// </summary>
        /// <param name="user">The user to log in</param>
        [HttpPost]
        public IActionResult Login(UserInfo user)
        {
            string token = "";

            // var response = _client.PostAsJsonAsync("Auth/GenerateToken", user).Result;
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

        /// <summary>
        /// Logs the user out
        /// </summary>
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Token");

            return RedirectIActionResult();
        }

        #region Extra Methods

        private IActionResult RedirectIActionResult()
        {
            string action;
            string controller;
            string quizId;
            string questionId;

            action = HttpContext.Session.GetString("Action");
            controller = HttpContext.Session.GetString("Controller");

            if (HttpContext.Session.GetString("QuestionId") != "")
            {
                quizId = HttpContext.Session.GetString("QuizId");
                questionId = HttpContext.Session.GetString("QuestionId");

                return RedirectToAction(action, controller, new { quizId = quizId, id = questionId });
            }
            else if (HttpContext.Session.GetString("QuizId") != "")
            {
                quizId = HttpContext.Session.GetString("QuizId");

                return RedirectToAction(action, controller, new { id = quizId });
            }

            return RedirectToAction(action, controller);
        }

        #endregion
    }
}
