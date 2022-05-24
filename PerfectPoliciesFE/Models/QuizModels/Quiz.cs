using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PerfectPoliciesFE.Models.QuestionModels;

namespace PerfectPoliciesFE.Models.QuizModels
{
    public class Quiz
    {
        // Primary Key
        [Display(Name = "Quiz Id")]
        public int QuizId { get; set; }

        // Attributes
        public string Title { get; set; }
        public string Topic { get; set; }
        public string Author { get; set; }
        
        [Display(Name = "Date Created")]
        public DateTime? DateCreated { get; set; }
        [Display(Name = "Passing Grade")]
        public int PassingGrade { get; set; }

        // Navigation Property
        public ICollection<Question> Questions { get; set; }
    }
}
