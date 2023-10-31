using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuizApp
{
    public class DepositeExpense
    {
        public DepositeExpense()
        {
        }
        public long DepositeExpenseID { get; set; }
        public DateTime DDate { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string TType { get; set; }
        public string HeadType { get; set; }

    }
}
