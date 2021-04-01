using Interest.Models;
using System;

namespace Interest.ViewModels
{
    public class PaymentViewModel : ViewModelBase
    {
        public PaymentViewModel(DateTime month, InputValue<double> payment, double debt, double borrowingPercentagePerYear, InputValue<double> unscheduledRepayment, Action<object> calculateCommandExecute)
        {
            _calculateCommandExecute = calculateCommandExecute;
            var reducedDebt = Calculator.GetReducedDebt(debt, unscheduledRepayment);
            var interest = Calculator.GetInterest(reducedDebt, borrowingPercentagePerYear);
            _paymentModel = new PaymentModel(month, payment, debt, borrowingPercentagePerYear, unscheduledRepayment, reducedDebt, interest);
        }

        public PaymentViewModel(DateTime month, double debt, double borrowingPercentagePerYear, Action<object> calculateCommandExecute) // no repayment, interest cost only
        {
            _calculateCommandExecute = calculateCommandExecute;

            var unscheduledRepayment = new InputValue<double>(0, InputType.Auto);
            var reducedDebt = Calculator.GetReducedDebt(debt, unscheduledRepayment);
            var interest = Calculator.GetInterest(reducedDebt, borrowingPercentagePerYear);
            _paymentModel = new PaymentModel(month, new InputValue<double>(interest, InputType.Auto), debt, borrowingPercentagePerYear, unscheduledRepayment, reducedDebt, interest);
        }

        PaymentModel _paymentModel;
        private Action<object> _calculateCommandExecute;

        public DateTime Month
        {
            get => _paymentModel.Month;
            set => _ = SetProperty(ref _paymentModel.Month, value);
        }

        public InputValue<double> Payment
        {
            get => _paymentModel.Payment;
            set
            {
                if (SetProperty(ref _paymentModel.Payment, value))
                {
                    _calculateCommandExecute?.Invoke(this);
                }
            }
        }

        public double Debt
        {
            get => _paymentModel.Debt;
            set
            {
                if (SetProperty(ref _paymentModel.Debt, value))
                {
                    _calculateCommandExecute?.Invoke(this);
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
                    _calculateCommandExecute?.Invoke(this);
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
                    _calculateCommandExecute?.Invoke(this);
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
