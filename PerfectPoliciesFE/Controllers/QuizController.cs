using PerfectPoliciesFE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using PerfectPoliciesFE.Models.QuestionModels;
using PerfectPoliciesFE.Models.QuizModels;
using PerfectPoliciesFE.Helpers;
using System;

namespace PerfectPoliciesFE.Controllers
{
    public class QuizController : Controller
    {
        private readonly IApiRequest<Quiz> _apiRequest;
        private readonly IApiRequest<Question> _apiQuestionRequest;

        private readonly string quizController = "Quiz";

        public QuizController(IApiRequest<Quiz> apiRequest)
        {
            _apiRequest = apiRequest;
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
            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }

            var quizList = _apiRequest.GetAll(quizController);

            var quizDDL = quizList.Select(c => new SelectListItem
            {
                Value = c.Author,
                Text = c.Author
            });

            ViewBag.QuizDDL = quizDDL;

            if (!String.IsNullOrEmpty(filter))
            {
                var quizfilteredList = quizList.Where(c => c.Author == filter);
                return View(quizfilteredList);

            }
            return View(quizList);
        }

        // GET: QuizController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: QuizController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuizController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QuizController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: QuizController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
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
            return View();
        }

        // POST: QuizController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
