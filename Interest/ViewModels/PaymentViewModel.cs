using Interest.Models;
using Prism.Mvvm;
using System;

namespace Interest.ViewModels
{

    public class PaymentViewModel : BindableBase
    {
        public PaymentViewModel(DateTime month, double monthlyPayment, double debt, double borrowingPercentagePerYear, UnscheduledRepayment unscheduledRepayment)
        {
            var reducedDebt = Calculator.GetReducedDebt(debt, unscheduledRepayment);
            var interest = Calculator.GetInterest(reducedDebt, borrowingPercentagePerYear);
            _payment = new Payment(month, monthlyPayment, debt, borrowingPercentagePerYear, unscheduledRepayment, reducedDebt, interest);
        }

        public PaymentViewModel(DateTime month, double debt, double BorrowingPercentagePerYear) // no repayment, interest cost only
        {
            var unscheduledRepayment = new UnscheduledRepayment(0, InputType.Auto);
            var reducedDebt = Calculator.GetReducedDebt(debt, unscheduledRepayment);
            var interest = Calculator.GetInterest(reducedDebt, BorrowingPercentagePerYear);
            _payment = new Payment(month, interest, debt, BorrowingPercentagePerYear, unscheduledRepayment, reducedDebt, interest);
        }

        Payment _payment;

        public DateTime Month
        {
            get => _payment.Month;
            set => _ = SetProperty(ref _payment.Month, value);
        }

        public double MonthlyPayment
        {
            get => _payment.MonthlyPayment;
            set => _ = SetProperty(ref _payment.MonthlyPayment, value);
        }

        public double Debt
        {
            get => _payment.Debt;
            set => _ = SetProperty(ref _payment.Debt, value);
        }

        public double BorrowingPercentagePerYear
        {
            get => _payment.BorrowingPercentagePerYear;
            set => _ = SetProperty(ref _payment.BorrowingPercentagePerYear, value);
        }

        public UnscheduledRepayment UnscheduledRepayment
        {
            get => _payment.UnscheduledRepayment;
            set => _ = SetProperty(ref _payment.UnscheduledRepayment, value);
        }

        public double Interest => _payment.Interest;
        public double BorrowingPercentage => _payment.BorrowingPercentage;
        public double Repayment => _payment.Repayment;
        public double ResidualDebt => _payment.ResidualDebt;
        public double ReducedDebt => _payment.ReducedDebt;
    }
}
