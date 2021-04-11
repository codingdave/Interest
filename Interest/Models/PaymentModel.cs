using Interest.Types;
using System;

namespace Interest.Models
{
    public struct PaymentModel
    {
        public PaymentModel(DateTime date,
            Currency payment, Currency debt,
            Percentage borrowing,
            Currency unscheduledRepayment, Currency reducedDebt,
            Currency interestPerYear)
        {
            if (payment < 0) { throw new ArgumentOutOfRangeException("No payment given"); }
            if (debt <= 0) { throw new ArgumentOutOfRangeException("No Debt given"); }
            if (borrowing.PerYear <= 0) { throw new ArgumentOutOfRangeException("No Borrowing Rate per year given"); }
            if (unscheduledRepayment < 0) { throw new ArgumentOutOfRangeException("Negative unscheduled repayment given"); }
            if (interestPerYear < 0) { throw new ArgumentOutOfRangeException("No interest given"); }

            Date = date;
            Borrowing = borrowing;

            UnscheduledRepayment = unscheduledRepayment;
            ReducedDebt = reducedDebt;
            Interest = interestPerYear;

            Payment = Currency.Min(payment, debt + Interest);
            Repayment = Calculator.GetRepayment(Payment, Interest);
            ResidualDebt = Calculator.GetResidualDebt(ReducedDebt, Repayment);
        }

        public DateTime Date;
        public Currency Payment;
        public Percentage Borrowing;
        public Currency UnscheduledRepayment;
        public Currency Interest;

        public Currency ReducedDebt { get; }
        public Currency Repayment { get; }
        public Currency ResidualDebt { get; }
    }
}
