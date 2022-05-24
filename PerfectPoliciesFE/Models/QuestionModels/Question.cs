﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PerfectPoliciesFE.Models.OptionModels;
using PerfectPoliciesFE.Models.QuizModels;

namespace PerfectPoliciesFE.Models.QuestionModels
{
    public class Question
    {
        // Primary Key
        [Display(Name = "Question Id")]
        public int QuestionId { get; set; }

        // Attributes
        public string Topic { get; set; }
        [Display(Name = "Question Text")]
        public string QuestionText { get; set; }
        
        #nullable enable
        public string? Image { get; set; }
        #nullable disable

        // Foreign Key
        [Display(Name = "Quiz Id")]
        public int QuizId { get; set; }

        // Navigation Property
        public Quiz Quiz { get; set; }
        public ICollection<Option> Options { get; set; }
    }
}
