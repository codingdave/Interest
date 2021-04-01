using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;

namespace Interest.ViewModels
{
    public class InterestPlanViewModel : BindableBase
    {
        public InterestPlanViewModel(string title = null)
        {
            Title = title;

            _conf = new ConfigurationManagerReaderWriter();
            StartMonth = DateTime.Parse(_conf.GetValue(nameof(StartMonth)), CultureInfo.InvariantCulture);
            Years = int.Parse(_conf.GetValue(nameof(Years)), CultureInfo.InvariantCulture);
            UnscheduledRepaymentPercentage = double.Parse(_conf.GetValue(nameof(UnscheduledRepaymentPercentage)), CultureInfo.InvariantCulture);
            BorrowingPercentagePerYear = double.Parse(_conf.GetValue(nameof(BorrowingPercentagePerYear)), CultureInfo.InvariantCulture);
            RedemptionPercentage = double.Parse(_conf.GetValue(nameof(RedemptionPercentage)), CultureInfo.InvariantCulture);
            LoanAmount = double.Parse(_conf.GetValue(nameof(LoanAmount)), CultureInfo.InvariantCulture);

            CalculateCommand = new DelegateCommand(() =>
            {
                Payments = Calculate();
            });

            CalculateCommand.Execute();
        }

        public IEnumerable<PaymentViewModel> Calculate()
        {
            var ret = new List<PaymentViewModel>();

            var month = StartMonth;
            var endMonth = StartMonth.AddYears(Years);
            var monthlyPayment = RedemptionAmount;
            var residualDebt = LoanAmount;
            var redemptionFreeMonths = RedemptionFreeMonths;
            while (month < endMonth && residualDebt > 0)
            {
                month = month.AddMonths(1);

                PaymentViewModel p;
                if (ret.Count < redemptionFreeMonths)
                {
                    p = new PaymentViewModel(month, residualDebt, BorrowingPercentagePerYear);
                }
                else
                {
                    UnscheduledRepayment unscheduledRepayment = default;
                    if (IsApplyAllUnscheduledRepayments && month.Month == StartMonth.AddMonths(1).Month)
                    {
                        // on optimize we override everythings
                        unscheduledRepayment = new UnscheduledRepayment(GetRequiredAmount(LoanAmount * UnscheduledRepaymentPercentage / 100.0, residualDebt), InputType.Auto);
                    }
                    else
                    {
                        if (Payments?.Count() > ret.Count)
                        {
                            // else we keep manual edits
                            var o = Payments.ElementAt(ret.Count).UnscheduledRepayment;
                            if (o.InputType == InputType.Manual)
                            {
                                unscheduledRepayment = o;
                            }
                        }
                    }
                    p = new PaymentViewModel(month, monthlyPayment, residualDebt, BorrowingPercentagePerYear, unscheduledRepayment);
                }

                ret.Add(p);
                residualDebt = p.ResidualDebt;
            }
            return ret;
        }

        private static double GetRequiredAmount(double maximumAmount, double currentDebt)
        {
            return Math.Min(maximumAmount, currentDebt);
        }

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
                    _conf.AddUpdateAppSettings(nameof(RedemptionPercentage), value.ToString());
                }
            }
        }
        #endregion

        public double RedemptionAmount
        {
            get
            {
                return LoanAmount * (BorrowingPercentagePerYear + RedemptionPercentage) / 100.0 / 12.0;
            }
            set
            {
                RedemptionPercentage = (value * 100.0 * 12.0 / LoanAmount) - BorrowingPercentagePerYear;
            }
        }

        #region StartMonth
        private DateTime _startMonth;

        public DateTime StartMonth
        {
            get { return _startMonth; }
            set
            {
                if (SetProperty(ref _startMonth, value))
                {
                    _conf.AddUpdateAppSettings(nameof(StartMonth), value.ToString());
                }
            }
        }
        #endregion

        #region RedemptionFreeMonths
        private int _redemptionFreeMonths;

        public int RedemptionFreeMonths
        {
            get { return _redemptionFreeMonths; }
            set
            {
                if (SetProperty(ref _redemptionFreeMonths, value))
                {
                    _conf.AddUpdateAppSettings(nameof(RedemptionFreeMonths), value.ToString());
                }
            }
        }
        #endregion

        #region UnscheduledRepaymentPercentage
        private double _unscheduledRepaymentPercentage;

        public double UnscheduledRepaymentPercentage
        {
            get { return _unscheduledRepaymentPercentage; }
            set
            {
                if (SetProperty(ref _unscheduledRepaymentPercentage, value))
                {
                    _conf.AddUpdateAppSettings(nameof(UnscheduledRepaymentPercentage), value.ToString());
                }
            }
        }
        #endregion

        #region IsApplyAllÚnscheduledRepayments
        private bool _isApplyAllUnscheduledRepayments;

        public bool IsApplyAllUnscheduledRepayments
        {
            get { return _isApplyAllUnscheduledRepayments; }
            set { _isApplyAllUnscheduledRepayments = value; }
        }

        #endregion

        #region Years
        private int _years;
        public int Years
        {
            get { return _years; }
            set
            {
                if (SetProperty(ref _years, value))
                {
                    _conf.AddUpdateAppSettings(nameof(Years), value.ToString());
                }
            }
        }

        #endregion

        #region BorrowingPercentagePerYear
        private double _BorrowingPercentagePerYear;
        public double BorrowingPercentagePerYear
        {
            get { return _BorrowingPercentagePerYear; }
            set
            {
                if (SetProperty(ref _BorrowingPercentagePerYear, value))
                {
                    _conf.AddUpdateAppSettings(nameof(BorrowingPercentagePerYear), value.ToString());
                }
            }
        }
        #endregion

        #region LoanAmount
        private double _loanAmount;
        public double LoanAmount
        {
            get { return _loanAmount; }
            set
            {
                if (SetProperty(ref _loanAmount, value))
                {
                    _conf.AddUpdateAppSettings(nameof(LoanAmount), value.ToString());
                }
            }
        }
        #endregion


        private IEnumerable<PaymentViewModel> _payments;

        #region Title
        private string _title;
        public string Title
        {
            get => _title;
            set => _ = SetProperty(ref _title, value);
        }
        #endregion


        private ConfigurationManagerReaderWriter _conf;

        public DelegateCommand CalculateCommand { get; private set; }
    }
}
