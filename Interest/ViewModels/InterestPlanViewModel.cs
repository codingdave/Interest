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

            //var now = DateTime.Now;
            //var startMonth = new DateTime(now.Year, now.Month, 1);
            //_conf.GenerateDefaultValues(nameof(StartMonth), startMonth.ToString(CultureInfo.InvariantCulture));
            //_conf.GenerateDefaultValues(nameof(Years), "15".ToString(CultureInfo.InvariantCulture));
            //_conf.GenerateDefaultValues(nameof(UnscheduledRepaymentPercentage), "5".ToString(CultureInfo.InvariantCulture));
            //_conf.GenerateDefaultValues(nameof(BorrowingPercentage), ".99".ToString(CultureInfo.InvariantCulture));
            //_conf.GenerateDefaultValues(nameof(RedemptionPercentage), "2.5".ToString(CultureInfo.InvariantCulture));
            //_conf.GenerateDefaultValues(nameof(LoanAmount), "250000".ToString(CultureInfo.InvariantCulture));

            ResetCommand = new DelegateCommand(() =>
                {
                    StartMonth = DateTime.Parse(_conf.GetValue(nameof(StartMonth)), CultureInfo.InvariantCulture);
                    Years = int.Parse(_conf.GetValue(nameof(Years)), CultureInfo.InvariantCulture);
                    UnscheduledRepaymentPercentage = double.Parse(_conf.GetValue(nameof(UnscheduledRepaymentPercentage)), CultureInfo.InvariantCulture);
                    BorrowingPercentage = double.Parse(_conf.GetValue(nameof(BorrowingPercentage)), CultureInfo.InvariantCulture);
                    RedemptionPercentage = double.Parse(_conf.GetValue(nameof(RedemptionPercentage)), CultureInfo.InvariantCulture);
                    LoanAmount = double.Parse(_conf.GetValue(nameof(LoanAmount)), CultureInfo.InvariantCulture);

                    InitialPayments = Initialize();
                    Payments = Initialize();
                });

            UpdateCommand = new DelegateCommand(() =>
            {
                Update();
            });

            ResetCommand.Execute();
        }

        public IEnumerable<PaymentViewModel> Initialize()
        {
            var ret = new List<PaymentViewModel>();

            var month = StartMonth;
            var endMonth = StartMonth.AddYears(Years);
            var monthlyPayment = RedemptionAmount;
            var residualDebt = LoanAmount;

            while (month < endMonth && residualDebt > 0)
            {
                month = month.AddMonths(1);
                var p = new PaymentViewModel(month, monthlyPayment, residualDebt, BorrowingPercentage, 0);
                ret.Add(p);
                residualDebt = p.ResidualDebt;
            }
            return ret;
        }

        public void Update()
        {
            var ret = new List<PaymentViewModel>();
            var month = StartMonth;
            var endMonth = StartMonth.AddYears(Years);
            var residualDebt = LoanAmount;

            int i = 0;
            var numPayments = Payments.Count();
            while (month < endMonth && residualDebt > 0)
            {
                month = month.AddMonths(1);
                var unscheduledRepayment = IsApplyAllUnscheduledRepayments && month.Month == StartMonth.AddMonths(1).Month
                    ? GetRequiredAmount(InitialPayments.First().Debt * UnscheduledRepaymentPercentage / 100.0, residualDebt)
                    : numPayments > i
                        ? Payments.ElementAt(i).UnscheduledRepayment
                        : 0.0;
                var p = new PaymentViewModel(month, RedemptionAmount, residualDebt, BorrowingPercentage, unscheduledRepayment);
                ret.Add(p);
                residualDebt = p.ResidualDebt;
                i++;
            }

            Payments = ret;
        }

        private static double GetRequiredAmount(double maximumAmount, double currentDebt)
        {
            return Math.Min(maximumAmount, currentDebt);
        }

        public IEnumerable<PaymentViewModel> InitialPayments { get; private set; }

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
                return LoanAmount * (BorrowingPercentage + RedemptionPercentage) / 100.0 / 12.0;
            }
            set
            {
                RedemptionPercentage = (value * 100.0 * 12.0 / LoanAmount) - BorrowingPercentage;
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

        #region BorrowingPercentage
        private double _BorrowingPercentage;
        public double BorrowingPercentage
        {
            get { return _BorrowingPercentage; }
            set
            {
                if (SetProperty(ref _BorrowingPercentage, value))
                {
                    _conf.AddUpdateAppSettings(nameof(BorrowingPercentage), value.ToString());
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

        public DelegateCommand ResetCommand { get; private set; }
        public DelegateCommand UpdateCommand { get; private set; }
    }
}
