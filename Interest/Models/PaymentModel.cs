using System;

namespace Interest.Models
{
    public struct PaymentModel
    {
        public PaymentModel(DateTime date, InputValue<double> payment, double debt, double borrowingPercentagePerYear, InputValue<double> unscheduledRepayment, double reducedDebt, double interestPerYear)
        {
            if (payment.Value < 0) { throw new ArgumentOutOfRangeException("No payment given"); }
            if (debt <= 0) { throw new ArgumentOutOfRangeException("No Debt given"); }
            if (borrowingPercentagePerYear <= 0) { throw new ArgumentOutOfRangeException("No Borrowing Rate per year given"); }
            if (unscheduledRepayment.Value < 0) { throw new ArgumentOutOfRangeException("Negative unscheduled repayment given"); }
            if (interestPerYear < 0) { throw new ArgumentOutOfRangeException("No interest given"); }

            Date = date;
            BorrowingPercentagePerYear = borrowingPercentagePerYear;
            BorrowingPercentage = BorrowingPercentagePerYear / 12.0;
            UnscheduledRepayment = unscheduledRepayment;
            ReducedDebt = reducedDebt;
            Interest = interestPerYear;

            Payment = new InputValue<double>(Math.Min(payment.Value, debt + Interest), payment.InputType);
            Repayment = Calculator.GetRepayment(Payment.Value, Interest);
            ResidualDebt = Calculator.GetResidualDebt(ReducedDebt, Repayment);
        }

        public DateTime Date;
        public InputValue<double> Payment;
        public double BorrowingPercentagePerYear;
        public InputValue<double> UnscheduledRepayment;
        public double Interest;

        public double BorrowingPercentage { get; }
        public double ReducedDebt { get; }
        public double Repayment { get; }
        public double ResidualDebt { get; }
    }
}
