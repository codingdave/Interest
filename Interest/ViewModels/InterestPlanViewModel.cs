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
            });
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
            return Calculator.GetPaymentPlan(previousPayments: Payments,
                startMonth: StartMonth,
                years: Years,
                loan: LoanAmount,
                percentagePerYear: BorrowingPercentagePerYear,
                redemptionPercentage: RedemptionPercentage,
                redemptionFreeMonths: RedemptionFreeMonths,
                isApplyAllUnscheduledRepayments: IsApplyAllUnscheduledRepayments,
                unscheduledRepaymentPercentage: UnscheduledRepaymentPercentage,
                isFullRepayment: IsFullRepayment,
                updateCalculation: CalculateCommand.Execute);
        }

        public double RedemptionAmount
        {
            get { return Calculator.GetRedemptionAmount(LoanAmount, BorrowingPercentagePerYear, RedemptionPercentage); }
            set { RedemptionPercentage = Calculator.GetRedemptionPercentage(LoanAmount, BorrowingPercentagePerYear, value); }
        }

        #region RedemptionPercentage
        public double RedemptionPercentage
        {
            get { return Values._redemptionPercentage; }
            set
            {
                if (SetProperty(ref Values._redemptionPercentage, value))
                {
                    CalculateCommand.Execute();
                    RaisePropertyChanged(nameof(RedemptionAmount));
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

        #region IsApplyAllUnscheduledRepayments

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

        #endregion IsApplyAllUnscheduledRepayments

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
                    RaisePropertyChanged(nameof(RedemptionAmount));
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
                    RaisePropertyChanged(nameof(RedemptionAmount));
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