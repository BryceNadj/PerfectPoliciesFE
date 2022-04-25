using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PerfectPoliciesFE.Helpers;
using PerfectPoliciesFE.Services;
using PerfectPoliciesFE.Models.QuizModels;
using PerfectPoliciesFE.Models.QuestionModels;
using System.Collections.Generic;

namespace PerfectPoliciesFE.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IApiRequest<Question> _apiRequest;
        private readonly IApiRequest<Quiz> _apiQuizRequest;

        private readonly string questionController = "Question";

        // GET: QuestionController
        public QuestionController(IApiRequest<Question> apiRequest, IApiRequest<Quiz> apiQuizRequest)
        {
            _apiRequest = apiRequest;
            _apiQuizRequest = apiQuizRequest;
        }

        // GET: QuestionController
        public ActionResult Index(string filter = "")
        {
            var questionList = _apiRequest.GetAll(questionController);

            if (!String.IsNullOrEmpty(filter))
            {
                var questionFilteredList = questionList.Where(c => c.Topic == filter);
                return View(questionFilteredList);
            }

            return View(questionList);
        }

        // GET: QuestionController/Details/5
        public ActionResult Details(int id)
        {
            Question question = _apiRequest.GetSingle(questionController, id);
            ViewBag.quizId = question.QuizId;
            return View(question);
        }

        public ActionResult QuestionsByQuizId(int id)
        {
            List<Question> questions = _apiRequest.GetAll(questionController); 
            var filteredList = questions.Where(c => c.QuizId.Equals(id)).ToList();

            return View("Index", filteredList);
        }

        // GET: QuestionController/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateForQuiz(int id)
        {
            QuestionCreate question = new QuestionCreate
            {
                QuizId = id
            };

            ViewBag.quizId = id;

            return View(question);
        }

        // POST: QuestionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuestionCreate question)
        {
            try
            {
                Question createdQuestion = new Question()
                {
                    Topic = question.Topic,
                    QuestionText = question.QuestionText,
                    Image = question.Image,
                    QuizId = question.QuizId
                };

                _apiRequest.Create(questionController, createdQuestion);
                ViewBag.quizId = createdQuestion.QuizId;

                return RedirectToAction("QuestionsByQuizId", "Question", new { id = question.QuizId });
            }
            catch
            {
                return View();
            }
        }

        // GET: QuestionController/Edit/5
        public ActionResult Edit(int id)
        {
            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }

            Question question = _apiRequest.GetSingle(questionController, id);
            ViewBag.quizId = id;

            return View(question);
        }

        // POST: QuestionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Question question)
        {
            try
            {
                if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
                {
                    return RedirectToAction("Login", "Auth");
                }

                _apiRequest.Edit(questionController, question, id);
                ViewBag.quizId = question.QuizId;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.quizId = question.QuizId;
                return View();
            }
        }

        // GET: QuestionController/Delete/5
        public ActionResult Delete(int id)
        {
            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }

            Question question = _apiRequest.GetSingle(questionController, id);
            ViewBag.quizId = question.QuizId;

            return View(question);
        }

        // POST: QuestionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {

                if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
                {
                    return RedirectToAction("Login", "Auth");
                }

                _apiRequest.Delete(questionController, id);
                ViewBag.quizId = id;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.quizId = id;
                return View();
            }
        }
    }
}
