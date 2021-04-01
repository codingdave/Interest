using Interest.Commands;
using Interest.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interest.ViewModels
{
    public class InterestPlanViewModel : ViewModelBase
    {
        internal InterestPlanViewModelOptions Values { get; }
        private IEnumerable<PaymentViewModel> _payments;

        public InterestPlanViewModel() : this(InterestPlanViewModelOptions.GetDefault())
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                throw new InvalidOperationException("Design time only");
            }
        }

        public InterestPlanViewModel(InterestPlanViewModelOptions values)
        {
            Values = values;

            CalculateCommand = new DelegateCommand(() =>
            {
                if (!_isCalculateCommandRunning)
                {
                    _isCalculateCommandRunning = true;
                    Payments = Calculate();
                }
                _isCalculateCommandRunning = false;
            }
            );
            ResetCommand = new DelegateCommand(() => ResetAllInputValues());

            CalculateCommand.Execute();
        }

        private void ResetAllInputValues()
        {
            foreach (var p in Payments)
            {
                p.Payment = new InputValue<double>(p.Payment.Value, InputType.Auto);
                p.UnscheduledRepayment = new InputValue<double>(p.UnscheduledRepayment.Value, InputType.Auto);
            }
            CalculateCommand.Execute();
        }

        public DelegateCommand CalculateCommand { get; private set; }
        public DelegateCommand ResetCommand { get; }

        public IEnumerable<PaymentViewModel> Payments
        {
            get => _payments;
            set
            {
                if (SetProperty(ref _payments, value))
                {
                    RaisePropertyChanged(nameof(TotalInterest));
                    RaisePropertyChanged(nameof(ResidualDebt));
                    RaisePropertyChanged(nameof(RedemptionPercentage));
                }
            }
        }

        public double RedemptionAmount
        {
            get
            {
                return LoanAmount * (BorrowingPercentagePerYear + RedemptionPercentage) / 100.0 / 12.0;
            }
            set
            {
                RedemptionPercentage = (value * 100.0 * 12.0 / LoanAmount) - BorrowingPercentagePerYear;
                CalculateCommand.Execute();
            }
        }

        public double ResidualDebt
        {
            get
            {
                var ret = LoanAmount;
                if (Payments.Any())
                {
                    ret = Payments.Last().ResidualDebt;
                }
                return ret;
            }
        }

        public double TotalInterest
        {
            get
            {
                var i = 0.0;
                if (Payments != null)
                {
                    foreach (var p in Payments)
                    {
                        i += p.Interest;
                    }
                }
                return i;
            }
        }

        public IEnumerable<PaymentViewModel> Calculate()
        {
            var ret = new List<PaymentViewModel>();
            var date = StartMonth;
            var endMonth = StartMonth.AddYears(Years);
            var residualDebt = LoanAmount;
            var borrowingPercentagePerYear = BorrowingPercentagePerYear;
            PaymentViewModel p;

            InputValue<double> unscheduledRepayment = new InputValue<double>(0, InputType.Auto);
            InputValue<double> payment = default;
            var redemptionFreeMonths = RedemptionFreeMonths;
            while (date < endMonth && residualDebt > 0)
            {
                date = date.AddMonths(1);

                if (ret.Count < redemptionFreeMonths)
                {
                    // we only pay interest, no redemption
                    p = new PaymentViewModel(date, residualDebt, borrowingPercentagePerYear, CalculateCommand.Execute);
                }
                else
                {
                    if (IsFullRepayment)
                    {
                        // payment: 471.60
                        // interest_0: 64.17
                        // full repayment
                        var borrowingRatePerYear = borrowingPercentagePerYear / 100.0;
                        var totalInterestFactor = Math.Pow(1.0 + borrowingRatePerYear, Years - 1);
                        var totalnterestPercentage = totalInterestFactor / (totalInterestFactor - 1);
                        var interestRatePerYear = borrowingRatePerYear * totalnterestPercentage;
                        var interestPercentagePerYear = interestRatePerYear * 100;
                        payment = new InputValue<double>(
                            Calculator.GetInterestCostPerMonth(
                                Calculator.GetReducedDebt(residualDebt, unscheduledRepayment), interestPercentagePerYear), InputType.Auto);
                        var debt = Calculator.GetReducedDebt(residualDebt, unscheduledRepayment);

                        p = new PaymentViewModel(date, payment, debt, interestPercentagePerYear, unscheduledRepayment,
                            debt, interestPercentagePerYear,
                            CalculateCommand.Execute);
                        residualDebt = p.ResidualDebt;
                    }
                    else
                    {
                        if (Payments?.Count() > ret.Count)
                        {
                            // we keep manual modifications
                            var oldPayment = Payments.ElementAt(ret.Count);
                            unscheduledRepayment = oldPayment.UnscheduledRepayment.InputType == InputType.Manual ? oldPayment.UnscheduledRepayment : default;
                            payment = oldPayment.Payment.InputType == InputType.Manual ? oldPayment.Payment : new InputValue<double>(RedemptionAmount, InputType.Auto);
                        }

                        if (unscheduledRepayment.InputType == InputType.Auto && IsApplyAllUnscheduledRepayments && date.Month == StartMonth.AddMonths(1).Month)
                        {
                            // on optimize we override everythings
                            unscheduledRepayment = new InputValue<double>(LoanAmount * UnscheduledRepaymentPercentage / 100.0, InputType.Auto);
                        }
                        unscheduledRepayment = new InputValue<double>(Math.Min(unscheduledRepayment.Value, residualDebt), unscheduledRepayment.InputType);

                        p = new PaymentViewModel(date, payment, residualDebt, borrowingPercentagePerYear, unscheduledRepayment, CalculateCommand.Execute);
                    }
                }

                ret.Add(p);
                residualDebt = p.ResidualDebt;
            }
            return ret;
        }

        #region RedemptionPercentage

        public double RedemptionPercentage
        {
            get { return Values._redemptionPercentage; }
            set
            {
                if (SetProperty(ref Values._redemptionPercentage, value))
                {
                    RaisePropertyChanged(nameof(RedemptionAmount));

                    CalculateCommand.Execute();
                }
            }
        }

        #endregion RedemptionPercentage

        #region StartMonth

        public DateTime StartMonth
        {
            get { return Values.StartMonth; }
            set
            {
                if (SetProperty(ref Values._startMonth, value))
                {
                    CalculateCommand.Execute();
                }
            }
        }

        #endregion StartMonth

        #region RedemptionFreeMonths

        public int RedemptionFreeMonths
        {
            get { return Values.RedemptionFreeMonths; }
            set
            {
                if (SetProperty(ref Values._redemptionFreeMonths, value))
                {
                    CalculateCommand.Execute();
                }
            }
        }

        #endregion RedemptionFreeMonths

        #region UnscheduledRepaymentPercentage

        public double UnscheduledRepaymentPercentage
        {
            get { return Values.UnscheduledRepaymentPercentage; }
            set
            {
                if (SetProperty(ref Values._unscheduledRepaymentPercentage, value))
                {
                    CalculateCommand.Execute();
                }
            }
        }

        #endregion UnscheduledRepaymentPercentage

        #region IsApplyAllÚnscheduledRepayments

        private bool _isApplyAllUnscheduledRepayments;

        public bool IsApplyAllUnscheduledRepayments
        {
            get { return _isApplyAllUnscheduledRepayments; }
            set
            {
                if (SetProperty(ref _isApplyAllUnscheduledRepayments, value))
                {
                    CalculateCommand.Execute();
                }
            }
        }

        #endregion IsApplyAllÚnscheduledRepayments

        #region Years

        public int Years
        {
            get { return Values.Years; }
            set
            {
                if (SetProperty(ref Values._years, value))
                {
                    CalculateCommand.Execute();
                }
            }
        }

        #endregion Years

        #region BorrowingPercentagePerYear
        public double BorrowingPercentagePerYear
        {
            get
            {
                return IsFullRepayment ?
                  _payments.Last().BorrowingPercentagePerYear :
                  Values.BorrowingPercentagePerYear;
            }
            set
            {
                if (SetProperty(ref Values._borrowingPercentagePerYear, value))
                {
                    CalculateCommand.Execute();
                }
            }
        }
        #endregion BorrowingPercentagePerYear

        #region LoanAmount
        public double LoanAmount
        {
            get { return Values.LoanAmount; }
            set
            {
                if (SetProperty(ref Values._loanAmount, value))
                {
                    CalculateCommand.Execute();
                }
            }
        }
        #endregion LoanAmount

        #region Lender
        public string Lender
        {
            get => Values.Lender;
            set => _ = SetProperty(ref Values._lender, value);
        }
        #endregion Lender

        public override string ToString()
        {
            return Values.ToString();
        }

        private bool _isCalculateCommandRunning;

        #region IsFullRepayment
        private bool _isFullRepayment;

        public bool IsFullRepayment
        {
            get => _isFullRepayment;
            set
            {
                if (SetProperty(ref _isFullRepayment, value))
                {
                    CalculateCommand.Execute();
                    RaisePropertyChanged(nameof(BorrowingPercentagePerYear));
                }
            }
        }
        #endregion IsFullRepayment
    }
}