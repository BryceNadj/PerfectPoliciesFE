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
        /// <summary>
        /// Gets all quizzes
        /// </summary>
        /// <param name="filter">An optional filter to be applied</param>
        /// <returns>A list of quizzes</returns>
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

        // GET: QuizController/Create
        /// <summary>
        /// Creates a quiz
        /// </summary>
        /// <returns>The quiz create view</returns>
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
        /// <summary>
        /// Creates a quiz in the database
        /// </summary>
        /// <param name="quiz">The quiz data to create for</param>
        /// <returns>The quiz view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuizCreate quiz)
        {
            try
            {
                if (quiz.Title == null || 
                    quiz.Topic == null || 
                    quiz.Author == null)
                {
                    ViewBag.Error = "The Topic, Author or Passing Grade field/s were empty. The must be filled in.";
                    return View();
                }

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

        // GET: QuizController/Details/5
<<<<<<< HEAD
        /// <summary>
        /// Gets details of a quiz
        /// </summary>
        /// <param name="id">Id of the quiz to get details for</param>
        /// <returns>The details page for a quiz</returns>
=======
>>>>>>> d9aec12de57858ab2b0ff1f4850fdc05b75c2b45
        public ActionResult Details(int id)
        {
            Quiz quiz = _apiRequest.GetSingle(quizController, id);

            return View(quiz);
        }

        // GET: QuizController/Edit/5
        /// <summary>
        /// Edits a quiz by its Id
        /// </summary>
        /// <param name="id">The quiz id</param>
        /// <returns>The edit quiz view</returns>
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
        /// <summary>
        /// Edits the quiz in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quiz"></param>
        /// <returns>The quiz view with the updated quiz</returns>
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
        /// <summary>
        /// Deletes a quiz by its Id
        /// </summary>
        /// <param name="id">The quiz Id</param>
        /// <returns>The delete quiz view</returns>
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
        /// <summary>
        /// Deletes the quiz in the database
        /// </summary>
        /// <param name="id">The Id of the quiz to be deleted</param>
        /// <returns>The quiz view now without the quiz that got deleted</returns>
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

        #region Extra Methods

        [HttpPost]
        public IActionResult FilterQuiz(IFormCollection collection)
        {
            string filterText = collection["topic"];
            var quizList = _apiRequest.GetAll(quizController);
            var filterList = quizList.Where(c => c.Topic.Contains(filterText)).ToList();

            return View("Index", filterList);
        }

        /// <summary>
        /// Places the values passed in through the routeValues param into TempData so any view can redirect to the right action if it needs to
        /// </summary>
        /// <param name="routeValues">The string[] containing the route values</param>
        private void SetupTempData(string[] routeValues)
        {
            TempData.Clear();

            TempData["Action"] = routeValues[0];
            TempData["Controller"] = routeValues[1];

            TempData.Keep();
        }

        /// <summary>
        /// Inserts the action and controller into a string array to pass into the SetupTempData(string[]) method
        /// </summary>
        /// <param name="action">The name of the action</param>
        /// <param name="controller">The name of the controller</param>
        private string[] SetupRouteValues(string action, string controller)
        {
            string[] routeValues = new string[] { action, controller };
            SetupTempData(routeValues);

            return routeValues;
        }
        #endregion
    }
}
