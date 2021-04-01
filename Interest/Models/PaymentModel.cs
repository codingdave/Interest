using System;

namespace Interest.Models
{
    public struct PaymentModel
    {
        public PaymentModel(DateTime month, InputValue<double> payment, double debt, double borrowingPercentagePerYear, InputValue<double> unscheduledRepayment, double reducedDebt, double interest)
        {
            if (payment.Value < 0) { throw new ArgumentOutOfRangeException("No payment given"); }
            if (debt <= 0) { throw new ArgumentOutOfRangeException("No Debt given"); }
            if (borrowingPercentagePerYear <= 0) { throw new ArgumentOutOfRangeException("No Borrowing Rate per year given"); }
            if (unscheduledRepayment.Value < 0) { throw new ArgumentOutOfRangeException("Negative unscheduled repayment given"); }
            if (interest < 0) { throw new ArgumentOutOfRangeException("No interest given"); }

            Date = month;
            Debt = debt;
            BorrowingPercentagePerYear = borrowingPercentagePerYear;
            BorrowingPercentage = Calculator.GetBorrowingPercentage(BorrowingPercentagePerYear);
            UnscheduledRepayment = unscheduledRepayment;
            ReducedDebt = reducedDebt;
            Interest = interest;

            Payment = new InputValue<double>(Math.Min(payment.Value, debt + Interest), payment.InputType);
            Repayment = Calculator.GetRepayment(Payment.Value, Interest);
            ResidualDebt = Calculator.GetResidualDebt(ReducedDebt, Repayment);
        }

        public DateTime Date;
        public InputValue<double> Payment;
        public double BorrowingPercentagePerYear;
        public double Debt;
        public InputValue<double> UnscheduledRepayment;
        public double Interest;

        public double BorrowingPercentage { get; }
        public double ReducedDebt { get; }
        public double Repayment { get; }
        public double ResidualDebt { get; }
    }
}
