using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuizApp
{
    public class MonthlyExpense
    {
        public long ExpenseMonthID { get; set; }
        public DateTime ExpenseMonth { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string ftype { get; set; }
    }
}
