﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace PerfectPoliciesFE.Services
{
    public class ApiRequest<T> : IApiRequest<T>
    {
        private static HttpClient _client;
        // required to gain access to the context
        private readonly HttpContext _httpContext;

        public ApiRequest(IHttpContextAccessor httpContextAccessor, IHttpClientFactory factory)
        {
            // injecting a reference to the current context
            _httpContext = httpContextAccessor.HttpContext;

            _client = factory.CreateClient("ApiClient");

            if (_client == null)
            {
                _client = new HttpClient();
                _client.BaseAddress = new Uri("https://perfectpoliciesapibrycenadj.azurewebsites.net/api/");
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }


            // if true, a token exists in the session
            if (_httpContext.Session.GetString("Token") != null)
            {
                // add the token to the HttpClient 
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _httpContext.Session.GetString("Token"));
            }
        }

        public T Create(string controllerName, T entity)
        {
            HttpResponseMessage response = _client.PostAsJsonAsync(controllerName, entity).Result;

            var responseEntity = response.Content.ReadAsAsync<T>().Result;

            return responseEntity;
        }
            
        public List<T> GetAll(string controllerName)
        {
            HttpResponseMessage response = _client.GetAsync(controllerName).Result;

            var entityResult = response.Content.ReadAsAsync<List<T>>().Result;

            return entityResult;
        }

        public T GetSingle(string controllerName, int id)
        {
            HttpResponseMessage response = _client.GetAsync($"{controllerName}/{id}").Result;

            var entityResult = response.Content.ReadAsAsync<T>().Result;

            return entityResult;
        }

        public T Edit(string controllerName, T entity, int id)
        {
            HttpResponseMessage response = _client.PutAsJsonAsync($"{controllerName}/{id}", entity).Result;

            var responseEntity = response.Content.ReadAsAsync<T>().Result;

            return responseEntity;
        }

        public void Delete(string controllerName, int id)
        {
            HttpResponseMessage response = _client.DeleteAsync($"{controllerName}/{id}").Result;
        }

        /// <summary>
        /// Retrieves a list of items where the foreign key matches the provided Id
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="endpointName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<T> GetAllForParentId(string controllerName, string endpointName, int id)
        {
            var response = _client.GetAsync($"{controllerName}/{endpointName}/{id}").Result;

            var responseEntities = response.Content.ReadAsAsync<List<T>>().Result;

            return responseEntities;
        }

        public List<T> GetAllForEndpoint(string endpoint)
        {
            throw new NotImplementedException();
        }

        public List<T> GetSingleForEndpoint(string endpoint)
        {
            throw new NotImplementedException();
        }
    }
}
