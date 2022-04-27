using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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
        private readonly string quizController = "Quiz";

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

        // GET: OptionController/OptionsByQuestionId/{quizId}
        public ActionResult QuestionsByQuizId(int id)
        {
            List<Question> questions = _apiRequest.GetAll(questionController); 
            var filteredList = questions.Where(c => c.QuizId.Equals(id)).ToList();

            return View("Index", filteredList);
        }

        // GET: QuestionController/Create
        public ActionResult Create()
        {
            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        public ActionResult CreateForQuiz(int id)
        {
            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {

                string[] routeValues = new string[3] { "QuestionsByQuizId", questionController, id.ToString() };
                InsertRouteValuesIntoViewBags(routeValues);

                return RedirectToAction("Login", "Auth", new { routeValues = routeValues });
            }

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
            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }

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
            Question question = _apiRequest.GetSingle(questionController, id);
            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {
                Quiz quiz = _apiQuizRequest.GetSingle(quizController, question.QuizId);

                string[] routeValues = new string[3] { "QuestionsByQuizId", questionController, quiz.QuizId.ToString() };
                InsertRouteValuesIntoViewBags(routeValues);

                return RedirectToAction("Login", "Auth", new { routeValues = routeValues });
            }

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
                    Quiz quiz = _apiQuizRequest.GetSingle(quizController, question.QuizId);

                    string[] routeValues = new string[3] { "QuestionsByQuizId", questionController, quiz.QuizId.ToString() };
                    InsertRouteValuesIntoViewBags(routeValues);

                    return RedirectToAction("Login", "Auth", new { routeValues = routeValues });
                }

                _apiRequest.Edit(questionController, question, id);
                ViewBag.quizId = question.QuizId;

                return RedirectToAction("QuestionsByQuizId", "Question", new { id = question.QuizId });
            }
            catch
            {
                ViewBag.quizId = question.QuizId;
                return RedirectToAction("QuestionsByQuizId", "Question", new { id = question.QuizId });
            }
        }

        // GET: QuestionController/Delete/5
        public ActionResult Delete(int id)
        {
            Question question = _apiRequest.GetSingle(questionController, id);
            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {
                Quiz quiz = _apiQuizRequest.GetSingle(quizController, question.QuizId);

                string[] routeValues = new string[3] { "QuestionsByQuizId", questionController, quiz.QuizId.ToString() };
                InsertRouteValuesIntoViewBags(routeValues);

                return RedirectToAction("Login", "Auth", new { routeValues = routeValues });
            }

            ViewBag.quizId = question.QuizId;

            return View(question);
        }

        // POST: QuestionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            Question question = _apiRequest.GetSingle(questionController, id);
            try
            {
                if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
                {
                    string[] routeValues = new string[3] { "QuestionsByQuizId", questionController, question.QuizId.ToString() };
                    InsertRouteValuesIntoViewBags(routeValues);

                    return RedirectToAction("Login", "Auth", new { routeValues = routeValues });
                }


                _apiRequest.Delete(questionController, id);
                ViewBag.quizId = id;

                return RedirectToAction("QuestionsByQuizId", "Question", new { id = question.QuizId });
            }
            catch
            {
                ViewBag.quizId = id;
                return RedirectToAction("QuestionsByQuizId", "Question", new { id = question.QuizId });
            }
        }

        #region Extra Methods

        private void InsertRouteValuesIntoViewBags(string[] routeValues)
        {
            if (routeValues[0] == "null")
            /* Has to be "null" (if there is no action) because "" gets nulled automatically
             *   in the method params, which moves routeValues[1] to routeValues[0] (kinda cringe)
             * As long as I don't have an action called "null" this will be fine */
            { ViewBag.Action = null; }
            else
            { ViewBag.Action = routeValues[0]; }

            ViewBag.Controller = routeValues[1];

            try
            { ViewBag.QuizId = routeValues[2]; }
            catch (Exception)
            { /* QuizId is not required for the requested view */ }

            try
            { ViewBag.QuestionId = routeValues[3]; }
            catch (Exception)
            { /* QuestionId is not required for the requested view */ }
        }
        #endregion
    }
}
