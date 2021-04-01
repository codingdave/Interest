using System;

namespace Interest.Models
{
    public struct Payment
    {
        public Payment(DateTime month, double monthlyPayment, double debt, double borrowingPercentagePerYear, double unscheduledRepayment)
        {
            if (monthlyPayment <= 0) { throw new ArgumentOutOfRangeException("No mothly payment given"); }
            if (debt <= 0) { throw new ArgumentOutOfRangeException("No Debt given"); }
            if (borrowingPercentagePerYear <= 0) { throw new ArgumentOutOfRangeException("No Borrowing Rate per year given"); }
            if (unscheduledRepayment < 0) { throw new ArgumentOutOfRangeException("Negative unscheduled repayment given"); }

            Month = month;
            Debt = debt;
            BorrowingPercentagePerYear = borrowingPercentagePerYear;
            UnscheduledRepayment = unscheduledRepayment;

            BorrowingPercentage = Calculator.GetBorrowingPercentagePerYear(BorrowingPercentagePerYear);
            ReducedDebt = Calculator.GetReducedDebt(Debt, UnscheduledRepayment);
            Interest = Calculator.GetInterest(ReducedDebt, BorrowingPercentage);
            MonthlyPayment = Math.Min(monthlyPayment, debt + Interest);
            Repayment = Calculator.GetRepayment(MonthlyPayment, Interest);
            ResidualDebt = Calculator.GetResidualDebt(ReducedDebt, Repayment);
        }

        public DateTime Month;
        public double MonthlyPayment;
        public double BorrowingPercentagePerYear;
        public double Debt;
        public double UnscheduledRepayment;

        public double BorrowingPercentage { get; }
        public double ReducedDebt { get; }
        public double Interest { get; }
        public double Repayment { get; }
        public double ResidualDebt { get; }
    }
}
