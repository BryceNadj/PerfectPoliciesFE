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

        // GET: OptionController/OptionsByQuestionId/{questionId}?quizId={quizId}
        /// <summary>
        /// Gets a list of options where the QuestionId is equal the Id of the question that was selected
        /// </summary>
        /// <param name="id">The Id of the question</param>
        /// <returns>The option view</returns>
        public ActionResult OptionsByQuestionId(int id, int quizId)
        {
            List<Option> options = _apiRequest.GetAll(optionController); 

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

        /// <summary>
        /// Creates an option with the Id of a specified question
        /// </summary>
        /// <param name="id">The question id</param>
        /// <returns>The option create view</returns>
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
        /// <summary>
        /// Creates an option in the database
        /// </summary>
        /// <param name="option">The option data to create for</param>
        /// <returns>The option view</returns>
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

        // GET: OptionController/Details/5
        /// <summary>
        /// Gets details of an option
        /// </summary>
        /// <param name="id">Id of the option to get details for</param>
        /// <returns>The details page for an option</returns>
        public ActionResult Details(int id)
        {
            Option option = _apiRequest.GetSingle(optionController, id);
            Question question = _apiQuestionRequest.GetSingle(questionController, option.QuestionId);
            ViewBag.quizId = question.QuizId;

            SetupTempData(new string[] { "OptionsByQuestionId", optionController, question.QuizId.ToString(), option.QuestionId.ToString() });

            return View(option);
        }

        // GET: OptionController/Edit/5
        /// <summary>
        /// Edits an option by its Id
        /// </summary>
        /// <param name="id">The option id</param>
        /// <returns>The edit option view</returns>
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
        /// <summary>
        /// Edits the option in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="option"></param>
        /// <returns>The option view with the updated option</returns>
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
        /// <summary>
        /// Deletes a option by its Id
        /// </summary>
        /// <param name="id">The option Id</param>
        /// <returns>The delete option view</returns>
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
        /// <summary>
        /// Deletes the option in the database
        /// </summary>
        /// <param name="id">The Id of the option to be deleted</param>
        /// <returns>The option view now without the option that got deleted</returns>
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

        /// <summary>
        /// Places the values passed in through the routeValues param into TempData so any view can redirect to the right action if it needs to
        /// </summary>
        /// <param name="routeValues">The string[] containing the route values</param>
        private void SetupTempData(string[] routeValues)
        {
            TempData.Clear();

            TempData["Action"] = routeValues[0];
            TempData["Controller"] = routeValues[1];
            TempData["QuizId"] = routeValues[2];
            TempData["QuestionId"] = routeValues[3];

            TempData.Keep();
        }

        /// <summary>
        /// Inserts the action, controller, quizId and questionId into a string array to pass into the SetupTempData(string[]) method
        /// </summary>
        /// <param name="action">The name of the action</param>
        /// <param name="controller">The name of the controller</param>
        /// <param name="quizId">The quiz id</param>
        /// <param name="questionId">The question id</param>
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
