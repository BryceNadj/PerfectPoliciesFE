using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfectPoliciesFE.Helpers
{
    public class RouteValuesHelper
    {
        IHttpContextAccessor _httpContextAccessor;
        public RouteValuesHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Takes a string[] of the routevalues and inserts them into the session variables
        /// </summary>
        /// <param name="routeValues">The route values</param>
        public void SetupSessionVariables(string[] routeValues)
        {
            // Null all of the route values so anything that doesn't get changed here doesn't keep it's current value
            // eg. questionId is set to 7 and this method gets called, only changing the action, controller and quizId.
            //   The questionId would still be 7 and it may cause a problem
            _httpContextAccessor.HttpContext.Session.SetString("Action", "");
            _httpContextAccessor.HttpContext.Session.SetString("Controller", "");
            _httpContextAccessor.HttpContext.Session.SetString("QuizId", "");
            _httpContextAccessor.HttpContext.Session.SetString("QuestionId", "");



            string action = routeValues[0];
            string controller = routeValues[1];

            _httpContextAccessor.HttpContext.Session.SetString("Action", action);
            _httpContextAccessor.HttpContext.Session.SetString("Controller", controller);

            if (routeValues.Length.Equals(3))
            {
                string quizId = routeValues[2];
                _httpContextAccessor.HttpContext.Session.SetString("QuizId", quizId);
            }

            if (routeValues.Length.Equals(4))
            {
                string quizId = routeValues[2];
                string questionId = routeValues[3];

                _httpContextAccessor.HttpContext.Session.SetString("QuizId", quizId);
                _httpContextAccessor.HttpContext.Session.SetString("QuestionId", questionId);
            }


        }

        /// <summary>
        /// Inserts the action and controller into a string array to pass into the SetupSessionVariables(string[]) method
        /// </summary>
        /// <param name="action">The name of the action</param>
        /// <param name="controller">The name of the controller</param>
        public string[] SetupRouteValues(string action, string controller)
        {
            string[] routeValues = new string[] {
                action,
                controller
            };

            SetupSessionVariables(routeValues);

            return routeValues;
        }

        /// <summary>
        /// Inserts the action, controller and quizId into a string array to pass into the SetupSessionVariables(string[]) method
        /// </summary>
        /// <param name="action">The name of the action</param>
        /// <param name="controller">The name of the controller</param>
        /// <param name="quizId">The quiz id</param>
        public string[] SetupRouteValues(string action, string controller, int quizId)
        {
            string[] routeValues = new string[] {
                action,
                controller,
                quizId.ToString()
            };

            SetupSessionVariables(routeValues);

            return routeValues;
        }

        /// <summary>
        /// Inserts the action, controller, quizId and questionId into a string array to pass into the SetupSessionVariables(string[]) method
        /// </summary>
        /// <param name="action">The name of the action</param>
        /// <param name="controller">The name of the controller</param>
        /// <param name="quizId">The quiz id</param>
        /// <param name="questionId">The question id</param>
        public string[] SetupRouteValues(string action, string controller, int quizId, int questionId)
        {
            string[] routeValues = new string[] {
                action,
                controller,
                quizId.ToString(),
                questionId.ToString() };

            SetupSessionVariables(routeValues);

            return routeValues;
        }
    }
}
