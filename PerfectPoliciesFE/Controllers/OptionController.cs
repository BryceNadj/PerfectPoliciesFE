using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PerfectPoliciesFE.Helpers;
using PerfectPoliciesFE.Services;
using PerfectPoliciesFE.Models.OptionModels;
using PerfectPoliciesFE.Models.QuestionModels;
using System.Collections.Generic;

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

        // GET: OptionController/Details/5
        public ActionResult Details(int id)
        {
            Option option = _apiRequest.GetSingle(optionController, id);
            Question question = _apiQuestionRequest.GetSingle(questionController, option.QuestionId);
            ViewBag.quizId = question.QuizId;

            return View(option);
        }

        // GET: OptionController/OptionsByQuestionId/{questionId}?quizId={quizId}
        public ActionResult OptionsByQuestionId(int id, int quizId)
        {
            List<Option> options = _apiRequest.GetAll(optionController); 
            var filteredOptionList = options.Where(c => c.QuestionId.Equals(id)).ToList();

            ViewBag.quizId = quizId;

            return View("Index", filteredOptionList);
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

                string[] routeValues = new string[4] { "OptionsByQuestionId", optionController, id.ToString(), question.QuizId.ToString() };
                InsertRouteValuesIntoViewBags(routeValues);

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

                return RedirectToAction("OptionsByQuestionId", "Option", new { id = option.QuestionId, quizId = ViewBag.quizId });
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

                string[] routeValues = new string[4] { "OptionsByQuestionId", optionController, id.ToString(), question.QuizId.ToString() };
                InsertRouteValuesIntoViewBags(routeValues);

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

                    string[] routeValues = new string[4] { "OptionsByQuestionId", optionController, id.ToString(), question.QuizId.ToString() };
                    InsertRouteValuesIntoViewBags(routeValues);

                    return RedirectToAction("Login", "Auth", new { routeValues = routeValues });
                }

                _apiRequest.Edit(optionController, option, id);

                ViewBag.quizId = question.QuizId;

                return RedirectToAction(nameof(Index));
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

                string[] routeValues = new string[4] { "OptionsByQuestionId", optionController, id.ToString(), question.QuizId.ToString() };
                InsertRouteValuesIntoViewBags(routeValues);

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

                    string[] routeValues = new string[4] { "OptionsByQuestionId", optionController, id.ToString(), question.QuizId.ToString() };
                    InsertRouteValuesIntoViewBags(routeValues);

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
