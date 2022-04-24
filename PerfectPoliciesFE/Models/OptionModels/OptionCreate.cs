using PerfectPoliciesFE.Models.QuestionModels;

namespace PerfectPoliciesFE.Models.OptionModels
{
    public class OptionCreate
    {
        // Attributes
        public string OptionText { get; set; }
        public string Order { get; set; }
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
    }
}
