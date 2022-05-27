using Microsoft.AspNetCore.Http;
using PerfectPoliciesFE.Models;
using PerfectPoliciesFE.Models.OptionModels;
using PerfectPoliciesFE.Models.QuestionModels;
using PerfectPoliciesFE.Models.QuizModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfectPoliciesFE.Services
{
    public class InMemoryRequest<T> : IApiRequest<T> where T : class
    {
        TestDatabase _db;
        public InMemoryRequest(TestDatabase db, IHttpContextAccessor accessor)
        {
            _db = db;
            // accessor.HttpContext.Session.SetString("Token", "TestingToken");
        }

        public List<T> GetAll(string controllerName)
        {
            switch (typeof(T).Name)
            {
                case nameof(Quiz):
                    return _db.Quizzes as List<T>;

                case nameof(Question):
                    foreach (var question in _db.Questions)
                    {
                        question.Quiz = _db.Quizzes.Where(c => c.QuizId == question.QuizId).FirstOrDefault();
                    }
                    return _db.Questions as List<T>;
                
                case nameof(Option):
                    foreach (var option in _db.Options)
                    {
                        option.Question = _db.Questions.Where(c => c.QuestionId == option.QuestionId).FirstOrDefault();
                    }
                    return _db.Options as List<T>;



                default:
                    return null;
            }
        }

        public T GetSingle(string controllerName, int id)
        {
            switch(typeof(T).Name)
            {
                case nameof(Quiz):
                    return _db.Quizzes.Where(c => c.QuizId == id).FirstOrDefault() as T;

                case nameof(Question):
                    return _db.Questions.Where(c => c.QuestionId == id).FirstOrDefault() as T;

                case nameof(Option):
                    return _db.Options.Where(c => c.OptionId == id).FirstOrDefault() as T;

                default:
                    return null;
            }
        }

        public T Create(string controllerName, T entity)
        {
            switch (typeof(T).Name)
            {
                case nameof(Quiz):
                    var quiz = entity as Quiz;
                    quiz.QuizId = _db.Quizzes.Count == 0 ? 1 : _db.Quizzes.OrderByDescending(c => c.QuizId).FirstOrDefault().QuizId + 1;
                    _db.Quizzes.Add(quiz);

                    return quiz as T;

                case nameof(Question):
                    var question = entity as Question;
                    question.QuizId = _db.Questions.Count == 0 ? 1 : _db.Questions.OrderByDescending(c => c.QuestionId).FirstOrDefault().QuestionId + 1;
                    _db.Questions.Add(question);

                    return question as T;

                case nameof(Option):
                    var option = entity as Option;
                    option.OptionId = _db.Options.Count == 0 ? 1 : _db.Options.OrderByDescending(c => c.OptionId).FirstOrDefault().OptionId + 1;
                    _db.Options.Add(option);

                    return option as T;
            }
            return entity;
        }

        public T Edit(string controllerName, T entity, int id)
        {
            switch (typeof(T).Name)
            {
                case nameof(Quiz):
                    var newQuiz = entity as Quiz;
                    var existingQuiz = _db.Quizzes.Where(c => c.QuizId == id).FirstOrDefault();

                    // Mapping
                    existingQuiz.Title = newQuiz.Title;
                    existingQuiz.Topic = newQuiz.Topic;
                    existingQuiz.DateCreated = newQuiz.DateCreated;
                    existingQuiz.PassingGrade = newQuiz.PassingGrade;

                    break;

                case nameof(Question):
                    var newQuestion = entity as Question;
                    var existingQuestion = _db.Questions.Where(c => c.QuestionId == id).FirstOrDefault();

                    // Mapping
                    existingQuestion.Topic = newQuestion.Topic;
                    existingQuestion.QuestionText = newQuestion.QuestionText;
                    existingQuestion.Image = newQuestion.Image;

                    break;

                default:
                    return null;
            }
            return null;
        }

        public void Delete(string controllerName, int id)
        {
            switch (typeof(T).Name)
            {
                case nameof(Quiz):
                    var quizEntity = _db.Quizzes.Where(c => c.QuizId == id).FirstOrDefault();
                    _db.Quizzes.Remove(quizEntity);
                    break;

                case nameof(Question):
                    var questionEntity = _db.Questions.Where(c => c.QuestionId == id).FirstOrDefault();
                    _db.Questions.Remove(questionEntity);
                    break;

                case nameof(Option):
                    var optionEntity = _db.Options.Where(c => c.OptionId == id).FirstOrDefault();
                    _db.Options.Remove(optionEntity);
                    break;

                default:
                    break;
            }
        }

        public List<T> GetAllForEndpoint(string endpoint)
        {
            if (endpoint.Contains("QuestionsByQuizId"))
            {
                //"Questions/QuestionsByQuizId/2"
                //[0]/[1]/[2]
                int quizId = int.Parse(endpoint.Split("/").LastOrDefault());
                return _db.Questions.Where(c => c.QuizId == quizId).FirstOrDefault() as List<T>;
            }

            if (endpoint.Contains("OptionsByQuestionId"))
            {
                //"Options/OptionsByQuestionId/4"
                //[0]/[1]/[2]
                string queryParams = endpoint.Split("/").LastOrDefault();
                int questionId = queryParams[0];

                return _db.Options.Where(c => c.QuestionId == questionId).FirstOrDefault() as List<T>;
            }

            return null;
        }

        public List<T> GetSingleForEndpoint(string endpoint)
        {
            throw new NotImplementedException();
        }
    }
}
