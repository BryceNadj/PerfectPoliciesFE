using System;
using System.ComponentModel.DataAnnotations;

namespace PerfectPoliciesFE.Models.QuizModels
{
    public class QuizCreate
    {
        // Attributes
        public string Title { get; set; }
        public string Topic { get; set; }
        public string Author { get; set; }

        [Display(Name = "Date Created")]
        private DateTime? DateCreated;

        [Display(Name = "Date Created")]
        public DateTime? DateCreatedProperty
        {
            get { return DateTime.Now; }
            set { DateCreated = value; }
        }

        [Display(Name = "Passing Grade")]
        public int PassingGrade { get; set; }
    }
}
