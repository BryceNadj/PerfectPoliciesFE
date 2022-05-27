using System.ComponentModel.DataAnnotations;

namespace PerfectPoliciesFE.Models.QuestionModels
{
    public class QuestionCreate
    {
        // Attributes
        public string Topic { get; set; }

        [Display(Name = "Question Text")]
        public string QuestionText { get; set; }

#pragma warning disable CS8632 // Nullable attribute
        public string? Image { get; set; }
#pragma warning restore CS8632

        [Display(Name = "Quiz Id")]
        public int QuizId { get; set; }
    }
}
