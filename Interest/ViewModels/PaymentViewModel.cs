using Interest.Models;
using Interest.Types;
using System;

namespace Interest.ViewModels
{
    public class PaymentViewModel : ViewModelBase
    {
        public PaymentViewModel(DateTime date,
            Currency payment, Currency debt,
            Percentage borrowing,
            Currency unscheduledRepayment)
        {
            // standard repayment: based on necessary input the reduced debt and the interest are calculated
            var reducedDebt = Calculator.GetReducedDebt(debt, unscheduledRepayment);
            var interest = Calculator.GetInterestCostPerMonth(reducedDebt, borrowing);

            _paymentModel = new PaymentModel(date, payment, debt, borrowing, unscheduledRepayment, reducedDebt, interest);
        }

        public PaymentViewModel(DateTime date,
            Currency debt,
            Percentage borrowing)
        {
            // no repayment, interest cost only
            var unscheduledRepayment = new Currency(0, InputKind.Auto);
            var reducedDebt = Calculator.GetReducedDebt(debt, unscheduledRepayment);
            var interest = Calculator.GetInterestCostPerMonth(reducedDebt, borrowing);

            _paymentModel = new PaymentModel(date, interest, debt, borrowing, unscheduledRepayment, reducedDebt, interest);
        }

        public PaymentViewModel(DateTime date, 
            Currency payment, Currency debt,
            Percentage borrowing, 
            Currency unscheduledRepayment,
            Currency reducedDebt, Currency interestPerYear)
        {
            // full repayment
            _paymentModel = new PaymentModel(date, payment, debt, borrowing, unscheduledRepayment, reducedDebt, interestPerYear);
        }

        PaymentModel _paymentModel;

        public DateTime Date
        {
            get => _paymentModel.Date;
            set => _ = SetProperty(ref _paymentModel.Date, value);
        }

        public Currency Payment
        {
            get => _paymentModel.Payment;
            set
            {
                if (SetProperty(ref _paymentModel.Payment, value))
                {
                }
            }
        }

        public Percentage Borrowing
        {
            get => _paymentModel.Borrowing;
            set
            {
                if (SetProperty(ref _paymentModel.Borrowing, value))
                {
                }
            }
        }

        public Currency UnscheduledRepayment
        {
            get => _paymentModel.UnscheduledRepayment;
            set
            {
                if (SetProperty(ref _paymentModel.UnscheduledRepayment, value))
                {
                }
            }
        }

        public Currency Interest => _paymentModel.Interest;
        public Currency Repayment => _paymentModel.Repayment;
        public Currency ResidualDebt => _paymentModel.ResidualDebt;
        public Currency ReducedDebt => _paymentModel.ReducedDebt;
    }
}
