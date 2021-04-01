using Interest.Models;
using Prism.Mvvm;
using System;

namespace Interest.ViewModels
{

    public class PaymentViewModel : BindableBase
    {
        public PaymentViewModel(DateTime month, double monthlyPayment, double initialDebt, double BorrowingPercentagePerYear, double unscheduledRepayment)
        {
            _payment = new Payment(month, monthlyPayment, initialDebt, BorrowingPercentagePerYear, unscheduledRepayment);
            if (ResidualDebt < 0) { throw new ArgumentOutOfRangeException("Residual Debt impossible"); }
        }

        Payment _payment;

        #region Month

        public DateTime Month
        {
            get { return _payment.Month; }
            set { SetProperty(ref _payment.Month, value); }
        }

        #endregion

        #region MonthlyPayment
        public double MonthlyPayment
        {
            get { return _payment.MonthlyPayment; }
            set { SetProperty(ref _payment.MonthlyPayment, value); }
        }

        #endregion

        #region InitialDebt

        public double InitialDebt
        {
            get { return _payment.InitialDebt; }
            set { SetProperty(ref _payment.InitialDebt, value); }
        }

        #endregion

        #region BorrowingPercentagePerYear
        public double BorrowingPercentagePerYear
        {
            get { return _payment.BorrowingPercentagePerYear; }
            set { SetProperty(ref _payment.BorrowingPercentagePerYear, value); }
        }

        #endregion

        #region UnscheduledRepayment
        public double UnscheduledRepayment
        {
            get { return _payment.UnscheduledRepayment; }
            set { SetProperty(ref _payment.UnscheduledRepayment, value); }
        }

        #endregion

        public double Interest => Calculator.GetInterest(ReducedDebt, BorrowingPercentage);

        public double BorrowingPercentage => Calculator.GetBorrowingPercentagePerYear(BorrowingPercentagePerYear);
        public double Repayment => Calculator.GetRepayment(MonthlyPayment, Interest);

        public double ResidualDebt => Calculator.GetResidualDebt(ReducedDebt, Repayment);
        public double ReducedDebt => Calculator.GetReducedDebt(InitialDebt, UnscheduledRepayment);

    }
}
