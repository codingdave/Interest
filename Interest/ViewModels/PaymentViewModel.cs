using Interest.Models;
using Prism.Mvvm;
using System;

namespace Interest.ViewModels
{

    public class PaymentViewModel : BindableBase
    {
        public PaymentViewModel(DateTime month, double monthlyPayment, double debt, double BorrowingPercentagePerYear, double unscheduledRepayment)
        {
            _payment = new Payment(month, monthlyPayment, debt, BorrowingPercentagePerYear, unscheduledRepayment);
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

        public double UnscheduledRepayment
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
