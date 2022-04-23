using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PerfectPoliciesFE.Helpers;
using PerfectPoliciesFE.Services;
using PerfectPoliciesFE.Models.QuizModels;
//using PerfectPoliciesFE.Models.OptionModels;
using PerfectPoliciesFE.Models.QuestionModels;
using System.Collections.Generic;

namespace PerfectPoliciesFE.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IApiRequest<Question> _apiRequest;
        //private readonly IApiRequest<Option> _apiOptionRequest;
        private readonly IApiRequest<Quiz> _apiQuizRequest;

        private readonly string questionController = "Question";

        // GET: QuestionController
        public QuestionController(IApiRequest<Question> apiRequest,/* IApiRequest<Option> apiOptionRequest, */IApiRequest<Quiz> apiQuizRequest)
        {
            _apiRequest = apiRequest;
            // _apiOptionRequest = apiOptionRequest;
            _apiQuizRequest = apiQuizRequest;
        }

        // GET: QuestionController
        public ActionResult Index(string filter = "")
        {
            var questionList = _apiRequest.GetAll(questionController);

            var questionDDL = questionList.Select(c => new SelectListItem
            {
                Value = c.Topic,
                Text = c.Topic
            });

            ViewBag.QuestionDDL = questionDDL;

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

            return View(question);
        }

        public ActionResult QuestionsByQuizId(int id)
        {
            List<Question> questions = _apiRequest.GetAll(questionController); // GetAllForParentId(questionController, "QuestionsByQuizId", id);
            var filteredList = questions.Where(c => c.QuizId.Equals(id)).ToList();

            return View("Index", filteredList);
        }

        // GET: QuestionController/Create
        public ActionResult Create()
        {
            // Get a list of quizzes from the API
            var quizList = _apiQuizRequest.GetAll("Quiz");


            var quizDropDownListModel = quizList.Select(c => new SelectListItem
            {
                Text = c.Title,
                Value = c.QuizId.ToString()
            }).ToList();

            ViewBag.TeacherDropDown = quizDropDownListModel;

            return View();
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

                //QuestionService.CreateNewQuestion(question);

                return RedirectToAction("Index");
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

                return RedirectToAction(nameof(Index));
            }
            catch
            {
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

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
