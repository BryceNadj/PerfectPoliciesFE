namespace PerfectPoliciesFE.Models.QuestionModels
{
    public class QuestionCreate
    {
        // Attributes
        public string Topic { get; set; }
        public string QuestionText { get; set; }

        #nullable enable
        public string? Image { get; set; }
        #nullable disable

        public int QuizId { get; set; }
    }
}
