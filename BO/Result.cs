using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuizApp
{
    public class Result
    {
        public long ResultID { get; set; }
        public long Score { get; set; }
        public long NoofCurrect { get; set; }
        public long NoofWrong { get; set; }
        public long SetID { get; set; }
        public string CandidateName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Institution { get; set; }
        public bool IsAwarded { get; set; }
        public DateTime QuizDate { get; set; }

    }
}
