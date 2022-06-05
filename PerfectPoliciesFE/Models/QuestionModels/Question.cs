using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PerfectPoliciesFE.Models.OptionModels;
using PerfectPoliciesFE.Models.QuizModels;

namespace PerfectPoliciesFE.Models.QuestionModels
{
    public class Question
    {
        // Primary Key
        [Display(Name = "Question Id")]
        [MaxLength(100)]
        public int QuestionId { get; set; }

        // Attributes
        [MaxLength(100)]
        public string Topic { get; set; }

        [Display(Name = "Question Text")]
        [MaxLength(100)]
        public string QuestionText { get; set; }


#nullable enable
        [MaxLength(100)]
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
