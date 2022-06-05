using PerfectPoliciesFE.Models.QuestionModels;
using System.ComponentModel.DataAnnotations;

namespace PerfectPoliciesFE.Models.OptionModels
{
    public class OptionCreate
    {
        // Attributes
        [Display(Name = "Option Text")]
        [MaxLength(100)]
        public string OptionText { get; set; }

        [MaxLength(2)]
        public string Order { get; set; }

        [Display(Name = "Answer Correct")]
        [MaxLength(100)]
        public bool IsCorrect { get; set; }

        [Display(Name = "Question Id")]
        [MaxLength(100)]
        public int QuestionId { get; set; }
    }
}
