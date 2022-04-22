using System;

namespace PerfectPoliciesFE.Models.QuizModels
{
    public class QuizCreate
    {
        // Attributes
        public string Title { get; set; }
        public string Topic { get; set; }
        public string Author { get; set; }

        private DateTime? DateCreated;

        public DateTime? DateCreatedProperty
        {
            get { return DateTime.Now; }
            set { DateCreated = value; }
        }


        public int PassingGrade { get; set; }
    }
}
