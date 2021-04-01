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
        private ConfigurationManagerReaderWriter _conf;

        private IEnumerable<PaymentViewModel> _payments;

        public InterestPlanViewModel(ConfigurationManagerReaderWriter configurationManagerReaderWriter, string title = null)
        {
            _conf = configurationManagerReaderWriter;

            Title = title;

            CalculateCommand = new DelegateCommand(() => Payments = Calculate());
            ResetCommand = new DelegateCommand(() => ResetAllInputValues());

            _startMonth = DateTime.Parse(_conf.GetValue(nameof(StartMonth)), CultureInfo.InvariantCulture);
            _years = int.Parse(_conf.GetValue(nameof(Years)), CultureInfo.InvariantCulture);
            _unscheduledRepaymentPercentage = double.Parse(_conf.GetValue(nameof(UnscheduledRepaymentPercentage)), CultureInfo.InvariantCulture);
            _borrowingPercentagePerYear = double.Parse(_conf.GetValue(nameof(BorrowingPercentagePerYear)), CultureInfo.InvariantCulture);
            _redemptionPercentage = double.Parse(_conf.GetValue(nameof(RedemptionPercentage)), CultureInfo.InvariantCulture);
            _loanAmount = double.Parse(_conf.GetValue(nameof(LoanAmount)), CultureInfo.InvariantCulture);

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

                    _conf.AddUpdateAppSettings(nameof(RedemptionPercentage), value.ToString());
                    CalculateCommand.Execute();
                }
            }
        }
        #endregion

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
                    CalculateCommand.Execute();
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
                    CalculateCommand.Execute();
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
        private int _years;
        public int Years
        {
            get { return _years; }
            set
            {
                if (SetProperty(ref _years, value))
                {
                    _conf.AddUpdateAppSettings(nameof(Years), value.ToString());
                    CalculateCommand.Execute();
                }
            }
        }

        #endregion

        #region BorrowingPercentagePerYear
        private double _borrowingPercentagePerYear;
        public double BorrowingPercentagePerYear
        {
            get { return _borrowingPercentagePerYear; }
            set
            {
                if (SetProperty(ref _borrowingPercentagePerYear, value))
                {
                    _conf.AddUpdateAppSettings(nameof(BorrowingPercentagePerYear), value.ToString());
                    CalculateCommand.Execute();
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
                    CalculateCommand.Execute();
                }
            }
        }
        #endregion

        #region Title
        private string _title;
        public string Title
        {
            get => _title;
            set => _ = SetProperty(ref _title, value);
        }
        #endregion
    }
}
