using System;
using System.Collections.Generic;
using PerfectPoliciesFE.Models.QuestionModels;

namespace PerfectPoliciesFE.Models.QuizModels
{
    public class Quiz
    {
        // Primary Key
        public int QuizId { get; set; }

        // Attributes
        public string Title { get; set; }
        public string Topic { get; set; }
        public string Author { get; set; }
        public DateTime? DateCreated { get; set; }
        public int PassingGrade { get; set; }

        // Navigation Property
        public ICollection<Question> Questions { get; set; }
    }
}
