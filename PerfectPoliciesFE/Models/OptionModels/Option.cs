using PerfectPoliciesFE.Models.QuestionModels;

namespace PerfectPoliciesFE.Models.OptionModels
{
    public class Option
    {
        // Primary Key
        public int OptionId { get; set; }

        // Attributes
        public string OptionText { get; set; }
        public string Order { get; set; }
        public bool IsCorrect { get; set; }

        // Foreign Key
        public int QuestionId { get; set; }

        // Navigation Poperty
        public Question Question { get; set; }
    }
}
