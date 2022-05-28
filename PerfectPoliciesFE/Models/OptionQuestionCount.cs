using System.ComponentModel.DataAnnotations;

namespace PerfectPoliciesFE.Models
{
    public class OptionQuestionCount
    {
        [Display(Name = "Question Text")]
        public string QuestionText { get; set; }

        [Display(Name = "Option Count")]
        public int OptionCount { get; set; }

        [Display(Name = "Quiz Id")]
        public int QuizId { get; set; }
    }
}
