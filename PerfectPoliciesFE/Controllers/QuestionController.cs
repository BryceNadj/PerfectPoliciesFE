﻿using System;
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

        // GET: QuestionController
        public QuestionController(IApiRequest<Question> apiRequest, IApiRequest<Quiz> apiQuizRequest, IWebHostEnvironment environment)
        {
            _apiRequest = apiRequest;
            _apiQuizRequest = apiQuizRequest;
            _environment = environment;
        }

        // GET: QuestionController/QuestionsByQuizId/{quizId}
        /// <summary>
        /// Gets a list of questions where the QuizId is equal the Id of the quiz that was selected
        /// </summary>
        /// <param name="id">The Id of the quiz</param>
        /// <returns>The question view</returns>
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

        /// <summary>
        /// Creates a question with the Id of a specified quiz
        /// </summary>
        /// <param name="id">The quiz id</param>
        /// <returns>The question createview</returns>
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
        /// <summary>
        /// Creates a question in the database
        /// </summary>
        /// <param name="question">The question data to create for</param>
        /// <returns>The question view</returns>
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

        // GET: QuestionController/Details/5
        /// <summary>
        /// Gets details of a question
        /// </summary>
        /// <param name="id">Id of the question to get details for</param>
        /// <returns>The details page for a question</returns>
        public ActionResult Details(int id)
        {
            Question question = _apiRequest.GetSingle(questionController, id);
            ViewBag.quizId = question.QuizId;

            SetupTempData(new string[] { "QuestionsByQuizId", questionController, question.QuizId.ToString() });

            return View(question);
        }

        // GET: QuestionController/Edit/5
        /// <summary>
        /// Edits a question by its Id
        /// </summary>
        /// <param name="id">The question id</param>
        /// <returns>The edit question view</returns>
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
        /// <summary>
        /// Edits the question in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="question"></param>
        /// <returns>The question view with the updated question</returns>
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
        /// <summary>
        /// Deletes a question by its Id
        /// </summary>
        /// <param name="id">The question Id</param>
        /// <returns>The delete question view</returns>
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
        /// <summary>
        /// Deletes the question in the database
        /// </summary>
        /// <param name="id">The Id of the question to be deleted</param>
        /// <returns>The question view now without the question that got deleted</returns>
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

            TempData.Keep();
        }

        /// <summary>
        /// Inserts the action, controller and quizId into a string array to pass into the SetupTempData(string[]) method
        /// </summary>
        /// <param name="action">The name of the action</param>
        /// <param name="controller">The name of the controller</param>
        /// <param name="quizId">The quiz id</param>
        private string[] SetupRouteValues(string action, string controller, int quizId)
        {
            string[] routeValues = new string[] { 
                action, 
                controller, 
                quizId.ToString() };
            SetupTempData(routeValues);

            return routeValues;
        }

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
        
        #endregion
    }
}
