using System.ComponentModel.DataAnnotations;

namespace PerfectPoliciesFE.Models.QuestionModels
{
    public class QuestionCreate
    {
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

        [Display(Name = "Quiz Id")]
        public int QuizId { get; set; }
    }
}
