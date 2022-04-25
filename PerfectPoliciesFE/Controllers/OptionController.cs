using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PerfectPoliciesFE.Helpers;
using PerfectPoliciesFE.Services;
using PerfectPoliciesFE.Models.QuizModels;
using PerfectPoliciesFE.Models.OptionModels;
using PerfectPoliciesFE.Models.QuestionModels;
using System.Collections.Generic;

namespace PerfectPoliciesFE.Controllers
{
    public class OptionController : Controller
    {
        private readonly IApiRequest<Option> _apiRequest;

        private readonly string optionController = "Option";

        // GET: OptionController
        public OptionController(IApiRequest<Option> apiRequest)
        {
            _apiRequest = apiRequest;
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

            var optionDDL = optionList.Select(c => new SelectListItem
            {
                Value = c.OptionText
            });

            ViewBag.OptionDDL = optionDDL;

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

            return View(option);
        }

        public ActionResult OptionsByQuestionId(int id)
        {
            List<Option> options = _apiRequest.GetAll(optionController); 
            var filteredList = options.Where(c => c.QuestionId.Equals(id)).ToList();

            return View("Index", filteredList);
        }

        // GET: OptionController/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateForQuestion(int id)
        {
            OptionCreate option = new OptionCreate
            {
                QuestionId = id
            };
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

                _apiRequest.Create(optionController, createdOption);

                return RedirectToAction("OptionsByQuestionId", "Option", new { id = option.QuestionId });
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

                _apiRequest.Delete(optionController, id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
