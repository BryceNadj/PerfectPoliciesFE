using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PerfectPoliciesFE.Helpers;
using PerfectPoliciesFE.Services;
using PerfectPoliciesFE.Models.OptionModels;
using PerfectPoliciesFE.Models.QuestionModels;

namespace PerfectPoliciesFE.Controllers
{
    public class OptionController : Controller
    {
        private readonly IApiRequest<Option> _apiRequest;
        private readonly IApiRequest<Question> _apiQuestionRequest;

        private readonly string optionController = "Option";
        private readonly string questionController = "Question";

        // GET: OptionController
        public OptionController(IApiRequest<Option> apiRequest, IApiRequest<Question> apiQuestionRequest)
        {
            _apiRequest = apiRequest;
            _apiQuestionRequest = apiQuestionRequest;
        }
        /*
        [HttpPost]
        public IActionResult Filter(IFormCollection collection)
        {
            var result = collection["optionDDL"].ToString();
            return RedirectToAction("Index", new { filter = result });
        }
        */

        
        // GET: OptionController
        public ActionResult Index(string filter = "")
        {
            var optionList = _apiRequest.GetAll(optionController);

            if (!String.IsNullOrEmpty(filter))
            {
                var optionFilteredList = optionList.Where(c => c.OptionText == filter);
                return View(optionFilteredList);
            }

            return View(optionList);
        }

        // GET: OptionController/OptionsByQuestionId/{questionId}?quizId={quizId}
        public ActionResult OptionsByQuestionId(int id, int quizId)
        {
#if TEST
            List<Option> options = _apiRequest.GetAllForEndpoint("OptionsByQuestionId");
#else
            List<Option> options = _apiRequest.GetAll(optionController); 
#endif
            var filteredOptionList = options.Where(c => c.QuestionId.Equals(id)).ToList();

            ViewBag.quizId = quizId;
            SetupTempData(new string[] { 
                "OptionsByQuestionId", // Action
                optionController, // Controller
                quizId.ToString(), // QuizId
                id.ToString() // QuestionId
            });      

            return View("Index", filteredOptionList);
        }

        // GET: OptionController/Details/5
        public ActionResult Details(int id)
        {
            Option option = _apiRequest.GetSingle(optionController, id);
            Question question = _apiQuestionRequest.GetSingle(questionController, option.QuestionId);
            ViewBag.quizId = question.QuizId;

            SetupTempData(new string[] { "OptionsByQuestionId", optionController, question.QuizId.ToString(), option.QuestionId.ToString() });

            return View(option);
        }

        // GET: OptionController/Create
        public ActionResult Create()
        {
            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        public ActionResult CreateForQuestion(int id)
        {
            Question question = _apiQuestionRequest.GetSingle(questionController, id);

            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {

                string[] routeValues = SetupRouteValues("OptionsByQuestionId", optionController, question.QuizId, id);

                return RedirectToAction("Login", "Auth", new { routeValues = routeValues });
            }

            OptionCreate option = new OptionCreate
            {
                QuestionId = id
            };

            ViewBag.quizId = question.QuizId;

            return View(option);
        }

        // POST: OptionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OptionCreate option)
        {
            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {
                Question question = _apiQuestionRequest.GetSingle(questionController, option.QuestionId);
                string[] routeValues = SetupRouteValues("QuestionsByQuizId", optionController, question.QuizId, question.QuestionId);

                return RedirectToAction("Login", "Auth", new { routeValues = routeValues });
            }

            try
            {
                Option createdOption = new Option()
                {
                    OptionText = option.OptionText,
                    Order = option.Order,
                    IsCorrect = option.IsCorrect,
                    QuestionId = option.QuestionId
                };

                Question question = _apiQuestionRequest.GetSingle(questionController, createdOption.QuestionId);
                ViewBag.quizId = question.QuizId;

                _apiRequest.Create(optionController, createdOption);

                return RedirectToAction("OptionsByQuestionId", "Option", new { id = option.QuestionId, quizId = question.QuizId });
            }
            catch
            {
                return View();
            }
        }

        // GET: OptionController/Edit/5
        public ActionResult Edit(int id)
        {
            Option option = _apiRequest.GetSingle(optionController, id);
            Question question = _apiQuestionRequest.GetSingle(questionController, option.QuestionId);

            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {
                string[] routeValues = SetupRouteValues("OptionsByQuestionId", optionController, question.QuizId, option.QuestionId);

                return RedirectToAction("Login", "Auth", new { routeValues = routeValues });
            }

            ViewBag.quizId = question.QuestionId;

            return View(option);
        }

        // POST: OptionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Option option)
        {
            try
            {
                Question question = _apiQuestionRequest.GetSingle(questionController, option.QuestionId);

                if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
                {
                    string[] routeValues = SetupRouteValues("OptionsByQuestionId", optionController, question.QuizId, option.QuestionId);

                    return RedirectToAction("Login", "Auth", new { routeValues = routeValues });
                }

                _apiRequest.Edit(optionController, option, id);

                ViewBag.quizId = question.QuizId;

                return RedirectToAction("OptionsByQuestionId", "Option", new { id = option.QuestionId, quizId = question.QuizId });
            }
            catch
            {
                return View();
            }
        }

        // GET: OptionController/Delete/5
        public ActionResult Delete(int id)
        {
            Option option = _apiRequest.GetSingle(optionController, id);
            Question question = _apiQuestionRequest.GetSingle(questionController, option.QuestionId);

            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {
                string[] routeValues = SetupRouteValues("OptionsByQuestionId", optionController, question.QuizId, option.QuestionId);

                return RedirectToAction("Login", "Auth", new { routeValues = routeValues });
            }

            ViewBag.quizId = question.QuizId;

            return View(option);
        }

        // POST: OptionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Option option = _apiRequest.GetSingle(optionController, id);
                Question question = _apiQuestionRequest.GetSingle(questionController, option.QuestionId);

                if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
                {
                    string[] routeValues = SetupRouteValues("OptionsByQuestionId", optionController, question.QuizId, option.QuestionId);

                    return RedirectToAction("Login", "Auth", new { routeValues = routeValues });
                }

                ViewBag.quizId = question.QuizId;

                _apiRequest.Delete(optionController, id);

                return RedirectToAction("OptionsByQuestionId", "Option", new { id = option.QuestionId, quizId = ViewBag.quizId });
            }
            catch
            {
                return View();
            }
        }

#region Extra Methods
        private void SetupTempData(string[] routeValues)
        {
            TempData.Clear();

            TempData["Action"] = routeValues[0];
            TempData["Controller"] = routeValues[1];
            TempData["QuizId"] = routeValues[2];
            TempData["QuestionId"] = routeValues[3];

            TempData.Keep();
        }

        private string[] SetupRouteValues(string action, string controller, int quizId, int questionId)
        {
            string[] routeValues = new string[] { 
                action, 
                controller, 
                quizId.ToString(), 
                questionId.ToString() };

            SetupTempData(routeValues);

            return routeValues;
        }
#endregion
    }
}
