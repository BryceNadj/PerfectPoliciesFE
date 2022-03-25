using System.Collections.Generic;
using PerfectPoliciesFE.Models.OptionModels;
using PerfectPoliciesFE.Models.QuizModels;

namespace PerfectPoliciesFE.Models.QuestionModels
{
    public class QuestionCreate
    {
        // Attributes
        public string Topic { get; set; }
        public string QuestionText { get; set; }
        public string? Image { get; set; }
    }
}
