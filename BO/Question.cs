using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuizApp
{
    public class Question
    {
        public Question()
        {
            Mark = 0;
        }
        public long QuestionID { get; set; }
        public long SetID { get; set; }
        public string Description { get; set; }
        public int Mark { get; set; }
        public int AnswerIndex { get; set; }
        
    }
}
