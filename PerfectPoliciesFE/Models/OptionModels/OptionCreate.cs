using PerfectPoliciesFE.Models.QuestionModels;
using System.ComponentModel.DataAnnotations;

namespace PerfectPoliciesFE.Models.OptionModels
{
    public class OptionCreate
    {
        // Attributes
        [Display(Name = "Option Text")]
        public string OptionText { get; set; }

        public string Order { get; set; }

        [Display(Name = "Answer Correct")]
        public bool IsCorrect { get; set; }

        [Display(Name = "Question Id")]
        public int QuestionId { get; set; }
    }
}
