using PerfectPoliciesFE.Models.OptionModels;
using PerfectPoliciesFE.Models.QuestionModels;
using PerfectPoliciesFE.Models.QuizModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfectPoliciesFE.Models
{
    public class TestDatabase
    {
        public List<Quiz> Quizzes { get; set; }
        public List<Question> Questions { get; set; }
        public List<Option> Options { get; set; }

        public List<UserInfo> Users { get; set; }

        public TestDatabase()
        {
            Quizzes = new List<Quiz>
            {
                new Quiz { QuizId = 1, Title = "BeetleJuice", Topic = "English", Author = "Me", DateCreated = DateTime.UtcNow, PassingGrade = 5 },
            };


            Questions = new List<Question>
            {
                new Question { QuestionId = 1,  QuestionText = "How do you spell 'Red'?", Image = null, Topic = "English", QuizId = 1 },
                new Question { QuestionId = 2, QuestionText = "What colour is a carrot?", Image = null, Topic = "English", QuizId = 1 }
            };


            Options = new List<Option>
            {
                new Option { OptionId = 1, OptionText = "L-S-T-E-R", Order = "A", IsCorrect = false, QuestionId = 1 },
                new Option { OptionId = 2, OptionText = "16", Order = "B", IsCorrect = false, QuestionId = 1 },
                new Option { OptionId = 3, OptionText = "R-E-D", Order = "C", IsCorrect = true, QuestionId = 1 },

                new Option { OptionId = 4, OptionText = "Purple", Order = "A", IsCorrect = false, QuestionId = 2 },
                new Option { OptionId = 5, OptionText = "Orange", Order = "B", IsCorrect = true, QuestionId = 2 },
                new Option { OptionId = 6, OptionText = "Pineapple", Order = "C", IsCorrect = false, QuestionId = 2 },
                new Option { OptionId = 7, OptionText = "I don't know", Order = "D", IsCorrect = false, QuestionId = 2 }
            };
        }
    }
}
