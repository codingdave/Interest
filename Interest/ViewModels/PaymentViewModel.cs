using Interest.Models;
using System;

namespace Interest.ViewModels
{
    public class PaymentViewModel : ViewModelBase
    {
        public PaymentViewModel(DateTime date, InputValue<double> payment, double debt,
            double borrowingPercentagePerYear, InputValue<double> unscheduledRepayment)
        {
            // standard repayment: based on necessary input the reduced debt and the interest are calculated
            var reducedDebt = Calculator.GetReducedDebt(debt, unscheduledRepayment);
            var interest = Calculator.GetInterestCostPerMonth(reducedDebt, borrowingPercentagePerYear);

            _paymentModel = new PaymentModel(date, payment, debt, borrowingPercentagePerYear, unscheduledRepayment, reducedDebt, interest);
        }

        public PaymentViewModel(DateTime date, double debt,
            double borrowingPercentagePerYear)
        {
            // no repayment, interest cost only
            var unscheduledRepayment = new InputValue<double>(0, InputType.Auto);
            var reducedDebt = Calculator.GetReducedDebt(debt, unscheduledRepayment);
            var interest = Calculator.GetInterestCostPerMonth(reducedDebt, borrowingPercentagePerYear);
           
            _paymentModel = new PaymentModel(date, new InputValue<double>(interest, InputType.Auto), debt, borrowingPercentagePerYear, unscheduledRepayment, reducedDebt, interest);
        }

        public PaymentViewModel(DateTime date, InputValue<double> payment, double debt,
            double borrowingPercentagePerYear, InputValue<double> unscheduledRepayment,
            double reducedDebt, double interestPerYear)
        {
            // full repayment
            _paymentModel = new PaymentModel(date, payment, debt, borrowingPercentagePerYear, unscheduledRepayment, reducedDebt, interestPerYear);
        }

        PaymentModel _paymentModel;

        public DateTime Date
        {
            get => _paymentModel.Date;
            set => _ = SetProperty(ref _paymentModel.Date, value);
        }

        public InputValue<double> Payment
        {
            get => _paymentModel.Payment;
            set
            {
                if (SetProperty(ref _paymentModel.Payment, value))
                {
                }
            }
        }

        public double BorrowingPercentagePerYear
        {
            get => _paymentModel.BorrowingPercentagePerYear;
            set
            {
                if (SetProperty(ref _paymentModel.BorrowingPercentagePerYear, value))
                {
                }
            }
        }

        public InputValue<double> UnscheduledRepayment
        {
            get => _paymentModel.UnscheduledRepayment;
            set
            {
                if (SetProperty(ref _paymentModel.UnscheduledRepayment, value))
                {
                }
            }
        }

        public double Interest => _paymentModel.Interest;
        public double BorrowingPercentage => _paymentModel.BorrowingPercentage;
        public double Repayment => _paymentModel.Repayment;
        public double ResidualDebt => _paymentModel.ResidualDebt;
        public double ReducedDebt => _paymentModel.ReducedDebt;
    }
}
