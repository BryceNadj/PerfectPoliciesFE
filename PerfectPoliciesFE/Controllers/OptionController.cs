using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [HttpPost]
        public IActionResult Filter(IFormCollection collection)
        {
            var result = collection["optionDDL"].ToString();
            return RedirectToAction("Index", new { filter = result });
        }

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
            return View();
        }

        public ActionResult CreateForQuestion(int id)
        {
            Question question = _apiQuestionRequest.GetSingle(questionController, id);
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
            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }

            Option option = _apiRequest.GetSingle(optionController, id);
            Question question = _apiQuestionRequest.GetSingle(questionController, option.QuestionId);
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
                if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
                {
                    return RedirectToAction("Login", "Auth");
                }

                _apiRequest.Edit(optionController, option, id);

                Question question = _apiQuestionRequest.GetSingle(questionController, option.QuestionId);
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
            if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }

            Option option = _apiRequest.GetSingle(optionController, id);
            Question question = _apiQuestionRequest.GetSingle(questionController, option.QuestionId);
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
                if (!AuthenticationHelper.isAuthenticated(this.HttpContext))
                {
                    return RedirectToAction("Login", "Auth");
                }

                Option option = _apiRequest.GetSingle(optionController, id);
                Question question = _apiQuestionRequest.GetSingle(questionController, option.QuestionId);
                ViewBag.quizId = question.QuizId;

                _apiRequest.Delete(optionController, id);

                return RedirectToAction("OptionsByQuestionId", "Option", new { id = option.QuestionId, quizId = ViewBag.quizId });
            }
            catch
            {
                return View();
            }
        }
    }
}
