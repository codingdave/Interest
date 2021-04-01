using System;

namespace Interest.Models
{
    public struct Payment
    {
        public Payment(DateTime month, double monthlyPayment, double initialDebt, double borrowingPercentagePerYear, double unscheduledRepayment)
        {
            if (monthlyPayment <= 0 && unscheduledRepayment <= 0) { throw new ArgumentOutOfRangeException("No mothly payment given"); }
            if (initialDebt < 0) { throw new ArgumentOutOfRangeException("No Debt given"); }
            if (borrowingPercentagePerYear <= 0) { throw new ArgumentOutOfRangeException("No Borrowing Rate per year given"); }
            if (unscheduledRepayment < 0) { throw new ArgumentOutOfRangeException("Negative unscheduled repayment given"); }

            Month = month;
            MonthlyPayment = monthlyPayment;
            InitialDebt = initialDebt;
            BorrowingPercentagePerYear = borrowingPercentagePerYear;
            UnscheduledRepayment = unscheduledRepayment;
        }

        public DateTime Month;
        public double MonthlyPayment;
        public double BorrowingPercentagePerYear;
        public double InitialDebt;
        public double UnscheduledRepayment;
    }
}
