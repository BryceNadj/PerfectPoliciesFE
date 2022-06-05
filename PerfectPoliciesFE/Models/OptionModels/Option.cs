using PerfectPoliciesFE.Models.QuestionModels;
using System.ComponentModel.DataAnnotations;

namespace PerfectPoliciesFE.Models.OptionModels
{
    public class Option
    {
        // Primary Key
        [Display(Name = "Option Id")]
        [MaxLength(100)]
        public int OptionId { get; set; }

        // Attributes
        [Display(Name = "Option Text")]
        [MaxLength(100)]
        public string OptionText { get; set; }

        [MaxLength(2)]
        public string Order { get; set; }

        [Display(Name = "Correct Answer")]
        [MaxLength(100)]
        public bool IsCorrect { get; set; }

        // Foreign Key
        [Display(Name = "Question Id")]
        [MaxLength(100)]
        public int QuestionId { get; set; }

        // Navigation Poperty
        public Question Question { get; set; }
    }
}
