using PerfectPoliciesFE.Models.OptionModels;
using PerfectPoliciesFE.Models.QuestionModels;
using PerfectPoliciesFE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PerfectPoliciesFE.Controllers
{
    public class OptionController : Controller
    {
        private readonly IApiRequest<Option> _apiRequest;
        private readonly IApiRequest<Question> _apiQuestionRequest;

        private readonly string optionController = "Option";

        // GET: OptionController
        public ActionResult Index()
        {
            return View();
        }

        // GET: OptionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OptionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OptionController/Create
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

        // GET: OptionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OptionController/Edit/5
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

        // GET: OptionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OptionController/Delete/5
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
