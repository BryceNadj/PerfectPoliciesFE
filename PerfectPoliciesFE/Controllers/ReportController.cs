using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PerfectPoliciesFE.Models;
using System.Collections.Generic;
using CsvHelper;
using ChartJSCore.Models;
using System.Linq;

namespace PerfectPoliciesFE.Controllers
{
    public class ReportController : Controller
    {
        HttpClient _client;
        public ReportController(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("ApiClient");
        }

        /// <summary>
        /// Gets the number of options for every question
        /// </summary>
        /// <returns>The OptionQuestionCount report view</returns>
        public IActionResult OptionQuestionCount()
        {
            int quizId = int.Parse(HttpContext.Session.GetString("QuizId"));

            var response = _client.GetAsync("Report/OptionQuestionCount").Result;

            List<OptionQuestionCount> optionQuestionCount = response.Content.ReadAsAsync<List<OptionQuestionCount>>().Result.Where(c => c.QuizId.Equals(quizId)).ToList();

            // serialise the report data and save in the session. 
            var jsonData = JsonSerializer.Serialize(optionQuestionCount);
            HttpContext.Session.SetString("ReportData", jsonData);

            // define the chart object itself
            Chart chart = new Chart();

            // define the type of chart
            chart.Type = Enums.ChartType.Bar;
            Data data = new Data();
            data.Labels = optionQuestionCount.Select(c => c.QuestionText).ToList();
            BarDataset barData = new BarDataset()
            {
                Label = "Options per question",
                Data = optionQuestionCount.Select(c => (double?)c.OptionCount).ToList()
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(barData);
            chart.Data = data;
            ViewData["chart"] = chart;

            return View();
        }

        /// <summary>
        /// Exports data
        /// </summary>
        /// <returns>A file that the users web browser will automatically download which contains the chart data</returns>
        public IActionResult ExportData()
        {
            // Get the data we need to export
            var jsonData = HttpContext.Session.GetString("ReportData");
            var reportData = JsonSerializer.Deserialize<List<OptionQuestionCount>>(jsonData);

            // Create an empty memory stream
            var stream = new MemoryStream();

            // Generate the CSV data
            using (var writeFile = new StreamWriter(stream, leaveOpen: true))
            {
                // Configuration of the CSV Writer
                var csv = new CsvWriter(writeFile, CultureInfo.CurrentCulture, leaveOpen: true);

                // Write the csv data to the memory stream
                csv.WriteRecords(reportData);
            }

            // Reset stream position
            stream.Position = 0;

            // CSV MIME type: "text/csv"

            // Return the memory stream as a file
            return File(stream, "application/octet-stream", $"QuizId_{HttpContext.Session.GetString("QuizId")}_OptionCountForQuestions_{DateTime.Now.ToString("ddMMMyy_HHmmss")}.csv");
        }
    }
}
