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
        [MaxLength(100)]
        public int QuizId { get; set; }

        // Attributes
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Topic { get; set; }

        [MaxLength(100)]
        public string Author { get; set; }
        
        [Display(Name = "Date Created")]
        public DateTime? DateCreated { get; set; }

        [Display(Name = "Passing Grade")]
        public int PassingGrade { get; set; }

        // Navigation Property
        public ICollection<Question> Questions { get; set; }
    }
}
