using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuizApp
{
    public class MonthlyBill
    {
        public MonthlyBill()
        {
            _WaterBill = 400;
            _GasBill = 800;
        }
        public long MonthlyBillID { get; set; }
        public DateTime BillDate { get; set; }
        public long PeopleID { get; set; }
        public long FlatID { get; set; }
        public double ElectricityBill { get; set; }
        //public double WaterBill { get; set; }
        //public double GasBill { get; set; }
        public double ActualBillAmount { get; set; }
        public double PaidBillAmount { get; set; }
        public double VehicleBill { get; set; }
        public double RentAmount { get; set; }
        public string  RentType { get; set; }
        public string ReciptNo { get; set; }

       
        private double _WaterBill;
        public double WaterBill
        {
            get { return _WaterBill; }
            set
            {               
                _WaterBill = value;
            }
        }
        private double _GasBill;
        public double GasBill
        {
            get { return _GasBill; }
            set
            {
                _GasBill = value;
            }
        }

       
    }
}
