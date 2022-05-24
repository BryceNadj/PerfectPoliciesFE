using PerfectPoliciesFE.Models.QuestionModels;
using System.ComponentModel.DataAnnotations;

namespace PerfectPoliciesFE.Models.OptionModels
{
    public class Option
    {
        // Primary Key
        [Display(Name = "Option Id")]
        public int OptionId { get; set; }

        // Attributes
        [Display(Name = "Option Text")]
        public string OptionText { get; set; }

        public string Order { get; set; }

        [Display(Name = "Correct Answer")]
        public bool IsCorrect { get; set; }

        // Foreign Key
        [Display(Name = "Question Id")]
        public int QuestionId { get; set; }

        // Navigation Poperty
        public Question Question { get; set; }
    }
}
