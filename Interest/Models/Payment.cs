using System;

namespace Interest.Models
{
    public struct Payment
    {
        public Payment(DateTime month, double monthlyPayment, double debt, double borrowingPercentagePerYear, UnscheduledRepayment unscheduledRepayment, double reducedDebt, double interest)
        {
            if (monthlyPayment < 0) { throw new ArgumentOutOfRangeException("No mothly payment given"); }
            if (debt <= 0) { throw new ArgumentOutOfRangeException("No Debt given"); }
            if (borrowingPercentagePerYear <= 0) { throw new ArgumentOutOfRangeException("No Borrowing Rate per year given"); }
            if (unscheduledRepayment.Value < 0) { throw new ArgumentOutOfRangeException("Negative unscheduled repayment given"); }
            if (interest < 0) { throw new ArgumentOutOfRangeException("No interest given"); }

            Month = month;
            Debt = debt;
            BorrowingPercentagePerYear = borrowingPercentagePerYear;
            BorrowingPercentage = Calculator.GetBorrowingPercentage(BorrowingPercentagePerYear);
            UnscheduledRepayment = unscheduledRepayment;
            ReducedDebt = reducedDebt;
            Interest = interest;

            MonthlyPayment = Math.Min(monthlyPayment, debt + Interest);
            Repayment = Calculator.GetRepayment(MonthlyPayment, Interest);
            ResidualDebt = Calculator.GetResidualDebt(ReducedDebt, Repayment);
        }

        public DateTime Month;
        public double MonthlyPayment;
        public double BorrowingPercentagePerYear;
        public double Debt;
        public UnscheduledRepayment UnscheduledRepayment;
        public double Interest;

        public double BorrowingPercentage { get; }
        public double ReducedDebt { get; }
        public double Repayment { get; }
        public double ResidualDebt { get; }
    }
}
