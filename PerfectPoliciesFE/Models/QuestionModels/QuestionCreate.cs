using System.ComponentModel.DataAnnotations;

namespace PerfectPoliciesFE.Models.QuestionModels
{
    public class QuestionCreate
    {
        // Attributes
        public string Topic { get; set; }

        [Display(Name = "Question Text")]
        public string QuestionText { get; set; }

        public string? Image { get; set; }

        [Display(Name = "Quiz Id")]
        public int QuizId { get; set; }
    }
}
