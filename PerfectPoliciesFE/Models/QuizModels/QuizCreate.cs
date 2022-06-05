using System;
using System.ComponentModel.DataAnnotations;

namespace PerfectPoliciesFE.Models.QuizModels
{
    public class QuizCreate
    {
        // Attributes
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Topic { get; set; }

        [MaxLength(100)]
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
