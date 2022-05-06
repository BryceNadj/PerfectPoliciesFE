using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using PerfectPoliciesFE.Helpers;
using PerfectPoliciesFE.Services;
using PerfectPoliciesFE.Models.QuizModels;
using PerfectPoliciesFE.Models.QuestionModels;

namespace PerfectPoliciesFE.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IApiRequest<Question> _apiRequest;
        private readonly IApiRequest<Quiz> _apiQuizRequest;
        private IWebHostEnvironment _environment;


        private readonly string questionController = "Question";
        // private readonly string quizController = "Quiz";

        // GET: QuestionController
        public QuestionController(IApiRequest<Question> apiRequest, IApiRequest<Quiz> apiQuizRequest, IWebHostEnvironment environment)
        {
            _apiRequest = apiRequest;
            _apiQuizRequest = apiQuizRequest;
            _environment = environment;
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

        // GET: QuestionController/QuestionsByQuizId/{quizId}
        public ActionResult QuestionsByQuizId(int id)
        {
            List<Question> questions = _apiRequest.GetAll(questionController); 
            var filteredList = questions.Where(c => c.QuizId.Equals(id)).ToList();

            SetupTempData(new string[] { 
                "QuestionsByQuizId", // Action
                questionController,  // Controller
                id.ToString() });    // QuizId

            return View("Index", filteredList);
        }

        // GET: QuestionController/Details/5
        public ActionResult Details(int id)
        {
            Question question = _apiRequest.GetSingle(questionController, id);
            ViewBag.quizId = question.QuizId;

            SetupTempData(new string[] { "QuestionsByQuizId", questionController, question.QuizId.ToString() });

            return View(question);
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
                string[] routeValues = SetupRouteValues("QuestionsByQuizId", questionController, id);

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
                string[] routeValues = SetupRouteValues("QuestionsByQuizId", questionController, question.QuizId);

                return RedirectToAction("Login", "Auth", new { routeValues = routeValues });
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
                string[] routeValues = SetupRouteValues("QuestionsByQuizId", questionController, question.QuizId);

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
                    // Quiz quiz = _apiQuizRequest.GetSingle(quizController, question.QuizId);

                    string[] routeValues = SetupRouteValues("QuestionsByQuizId", questionController, question.QuizId);

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
                string[] routeValues = SetupRouteValues("QuestionsByQuizId", questionController, question.QuizId);

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
                    string[] routeValues = SetupRouteValues("QuestionsByQuizId", questionController, question.QuizId);

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

        /*
        [BindProperty]
        public IFormFile Image { get; set; }
        public async Task OnPostAsync()
        {
            var file = Path.Combine(_environment.ContentRootPath, "wwwroot\\Uploads", Image.FileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Image.CopyToAsync(fileStream);
            }
        }
        */

        
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            // Retrieve folder path
            string folderRoot = Path.Combine(_environment.ContentRootPath, "wwwroot\\Uploads");

            // Combine filename and folder path
            string filePath = Path.Combine(folderRoot, file.FileName);

            try
            {
                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(new { success = true, message = "File Uploaded" });
            }
            catch (Exception e)
            {
                return BadRequest(new { success = false, message = e.Message });
            }
        }
        

        #region Extra Methods
        private void SetupTempData(string[] routeValues)
        {
            TempData.Clear();

            TempData["Action"] = routeValues[0];
            TempData["Controller"] = routeValues[1];
            TempData["QuizId"] = routeValues[2];

            TempData.Keep();
        }

        private string[] SetupRouteValues(string action, string controller, int quizId)
        {
            string[] routeValues = new string[] { 
                action, 
                controller, 
                quizId.ToString() };
            SetupTempData(routeValues);

            return routeValues;
        }
        #endregion
    }
}
