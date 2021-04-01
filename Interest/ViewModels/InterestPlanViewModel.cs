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

        public InterestPlanViewModel() : this(new InterestPlanViewModelOptions())
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                throw new InvalidOperationException("Design time only");
            }
        }

        public InterestPlanViewModel(InterestPlanViewModelOptions values)
        {
            Values = values;

            CalculateCommand = new DelegateCommand(() => Payments = Calculate());
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

            var month = StartMonth;
            var endMonth = StartMonth.AddYears(Years);
            InputValue<double> unscheduledRepayment = default;
            InputValue<double> payment = default;
            var residualDebt = LoanAmount;
            var redemptionFreeMonths = RedemptionFreeMonths;
            while (month < endMonth && residualDebt > 0)
            {
                month = month.AddMonths(1);

                PaymentViewModel p;
                if (ret.Count < redemptionFreeMonths)
                {
                    // we only pay interest, no redemption
                    p = new PaymentViewModel(month, residualDebt, BorrowingPercentagePerYear, CalculateCommand.Execute);
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

                    if (unscheduledRepayment.InputType == InputType.Auto && IsApplyAllUnscheduledRepayments && month.Month == StartMonth.AddMonths(1).Month)
                    {
                        // on optimize we override everythings
                        unscheduledRepayment = new InputValue<double>(LoanAmount * UnscheduledRepaymentPercentage / 100.0, InputType.Auto);
                    }
                    unscheduledRepayment = new InputValue<double>(Math.Min(unscheduledRepayment.Value, residualDebt), unscheduledRepayment.InputType);

                    p = new PaymentViewModel(month, payment, residualDebt, BorrowingPercentagePerYear, unscheduledRepayment, CalculateCommand.Execute);
                }

                ret.Add(p);
                residualDebt = p.ResidualDebt;
            }
            return ret;
        }

        #region RedemptionPercentage
        private double _redemptionPercentage;

        public double RedemptionPercentage
        {
            get { return _redemptionPercentage; }
            set
            {
                if (SetProperty(ref _redemptionPercentage, value))
                {
                    RaisePropertyChanged(nameof(RedemptionAmount));

                    CalculateCommand.Execute();
                }
            }
        }
        #endregion

        #region StartMonth
        public DateTime StartMonth
        {
            get { return Values.StartMonth; }
            set
            {
                if (SetProperty(ref Values.StartMonth, value))
                {
                    CalculateCommand.Execute();
                }
            }
        }
        #endregion

        #region RedemptionFreeMonths
        public int RedemptionFreeMonths
        {
            get { return Values.RedemptionFreeMonths; }
            set
            {
                if (SetProperty(ref Values.RedemptionFreeMonths, value))
                {
                    CalculateCommand.Execute();
                }
            }
        }
        #endregion

        #region UnscheduledRepaymentPercentage
        public double UnscheduledRepaymentPercentage
        {
            get { return Values.UnscheduledRepaymentPercentage; }
            set
            {
                if (SetProperty(ref Values.UnscheduledRepaymentPercentage, value))
                {
                    CalculateCommand.Execute();
                }
            }
        }
        #endregion

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
        #endregion

        #region Years
        public int Years
        {
            get { return Values.Years; }
            set
            {
                if (SetProperty(ref Values.Years, value))
                {
                    CalculateCommand.Execute();
                }
            }
        }
        #endregion

        #region BorrowingPercentagePerYear
        public double BorrowingPercentagePerYear
        {
            get { return Values.BorrowingPercentagePerYear; }
            set
            {
                if (SetProperty(ref Values.BorrowingPercentagePerYear, value))
                {
                    CalculateCommand.Execute();
                }
            }
        }
        #endregion

        #region LoanAmount
        public double LoanAmount
        {
            get { return Values.LoanAmount; }
            set
            {
                if (SetProperty(ref Values.LoanAmount, value))
                {
                    CalculateCommand.Execute();
                }
            }
        }
        #endregion

        #region Title
        public string Title
        {
            get => Values.Title;
            set => _ = SetProperty(ref Values.Title, value);
        }
        #endregion
    }
}
