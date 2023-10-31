using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuizApp
{
    public class Answer
    {
        public long AnswerID { get; set; }
        public long QuestionID { get; set; }
        public long SetID { get; set; }
        public string AnswerNo { get; set; }
        public string Description { get; set; }
        public bool IsCorrect { get; set; }

    }
}
