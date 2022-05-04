using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using PerfectPoliciesFE.Helpers;
using PerfectPoliciesFE.Services;
using PerfectPoliciesFE.Models.QuizModels;

namespace PerfectPoliciesFE.Controllers
{
    public class QuizController : Controller
    {
        private readonly IApiRequest<Quiz> _apiRequest;
        private readonly IWebHostEnvironment _environment;

        private readonly string quizController = "Quiz";

        public QuizController(IApiRequest<Quiz> apiRequest, IWebHostEnvironment environment)
        {
            _apiRequest = apiRequest;
            _environment = environment;
        }

        [HttpPost]
        public IActionResult Filter(IFormCollection collection)
        {
            var result = collection["quizDDL"].ToString();
            return RedirectToAction("Index", new { filter = result });
        }

        // GET: QuizController
        public ActionResult Index(string filter = "")
        {
            var quizList = _apiRequest.GetAll(quizController);

            if (!String.IsNullOrEmpty(filter))
            {
                var quizFilteredList = quizList.Where(c => c.Author == filter);
                return View(quizFilteredList);
            }

            SetupTempData(new string[] {
                "Index", // Action
                quizController // Controller
            });

            return View(quizList);
        }

        // GET: QuizController/Details/5
        public ActionResult Details(int id)
        {
            Quiz quiz = _apiRequest.GetSingle(quizController, id);

            return View(quiz);
        }

        // GET: QuizController/Create
        public ActionResult Create()
        {
            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {
                string[] routeValues = SetupRouteValues("Index", quizController);

                return RedirectToAction("Login", "Auth", new { routeValues = routeValues });
            }

            return View();
        }

        // POST: QuizController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuizCreate quiz)
        {
            try
            {
                Quiz createdQuiz = new Quiz()
                {
                    Title = quiz.Title,
                    Topic = quiz.Topic,
                    Author = quiz.Author,
                    DateCreated = DateTime.Now,
                    PassingGrade = quiz.PassingGrade
                };

                _apiRequest.Create(quizController, createdQuiz);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: QuizController/Edit/5
        public ActionResult Edit(int id)
        {
            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {
                string[] routeValues = SetupRouteValues("Index", quizController);

                return RedirectToAction("Login", "Auth", new { routeValues = routeValues });
            }

            Quiz quiz = _apiRequest.GetSingle(quizController, id);

            return View(quiz);
        }

        // POST: QuizController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Quiz quiz)
        {
            try
            {
                if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
                {
                    string[] routeValues = SetupRouteValues("Index", quizController);

                    return RedirectToAction("Login", "Auth", new { routeValues = routeValues });
                }

                _apiRequest.Edit(quizController, quiz, id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QuizController/Delete/5
        public ActionResult Delete(int id)
        {
            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {
                string[] routeValues = SetupRouteValues("Index", quizController);

                return RedirectToAction("Login", "Auth", new { routeValues = routeValues });
            }

            Quiz quiz = _apiRequest.GetSingle(quizController, id);

            return View(quiz);
        }

        // POST: QuizController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
                {
                    string[] routeValues = SetupRouteValues("Index", quizController);

                    return RedirectToAction("Login", "Auth", new { routeValues = routeValues });
                }

                _apiRequest.Delete(quizController, id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult FilterQuiz(IFormCollection collection)
        {
            string filterText = collection["topic"];
            var quizList = _apiRequest.GetAll(quizController);
            var filterList = quizList.Where(c => c.Topic.Contains(filterText)).ToList();

            return View("Index", filterList);
        }

        #region Extra Methods
        private void SetupTempData(string[] routeValues)
        {
            TempData.Clear();

            TempData["Action"] = routeValues[0];
            TempData["Controller"] = routeValues[1];

            TempData.Keep();
        }

        private string[] SetupRouteValues(string action, string controller)
        {
            string[] routeValues = new string[] { action, controller };
            SetupTempData(routeValues);

            return routeValues;
        }
        #endregion
    }
}
