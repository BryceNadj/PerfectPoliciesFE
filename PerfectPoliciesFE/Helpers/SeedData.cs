using PerfectPoliciesFE.Models;
using PerfectPoliciesFE.Models.OptionModels;
using PerfectPoliciesFE.Models.QuestionModels;
using PerfectPoliciesFE.Models.QuizModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfectPoliciesFE.Helpers
{
    public static class SeedData
    {

        public static List<Quiz> GenerateQuizzes() 
        {
            return new List<Quiz>
            {
                new Quiz { QuizId = 1, Title = "BeetleJuice", Topic = "English", Author = "Me", DateCreated = DateTime.UtcNow, PassingGrade = 5 },
            };
        }

        public static List<Question> GenerateQuestions()
        {
            return new List<Question>
            {
                new Question { QuestionId = 1,  QuestionText = "How do you spell 'Red'?", Image = null, Topic = "English", QuizId = 1 },
                new Question { QuestionId = 2, QuestionText = "What colour is a carrot?", Image = null, Topic = "English", QuizId = 1 }
            };
        }

        public static List<Option> GenerateOptions()
        {
            return new List<Option> 
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

        public static List<UserInfo> GenerateUsers()
        {
            return new List<UserInfo> 
            { 
                new UserInfo { Username = "a", Password = "a" }
            };
        }
    }
}