using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;

namespace Interest.ViewModels
{
    public class InterestPlanViewModel : BindableBase
    {
        public InterestPlanViewModel()
        {
            ResetCommand = new DelegateCommand(() =>
            {
                var now = DateTime.Now;
                StartMonth = new DateTime(now.Year, now.Month, 1);

                Years = 20;
                UnscheduledRepaymentPercentage = 5;
                BorrowingPercentage = 0.84;
                RedemptionPercentage = 2.75;
                LoanAmount = 381000;

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

            while (month < endMonth)
            {
                month = month.AddMonths(1);
                var residualDebt = ret.Any() ? ret.Last().ResidualDebt : LoanAmount;

                var remainingDebtPlusRate = residualDebt + residualDebt * BorrowingPercentage / 12.0;
                if (remainingDebtPlusRate >= monthlyPayment)
                {
                    var p = new PaymentViewModel(month, monthlyPayment, residualDebt, BorrowingPercentage, 0);
                    ret.Add(p);
                }
                else
                {
                    monthlyPayment = remainingDebtPlusRate;
                    var p = new PaymentViewModel(month, monthlyPayment, residualDebt, BorrowingPercentage, 0);
                    ret.Add(p);
                    break;
                }
            }
            return ret;
        }

        public void Update()
        {
            var ret = new List<PaymentViewModel>();
            var currentDebt = LoanAmount;
            var month = StartMonth;
            var endMonth = StartMonth.AddYears(Years);
            int i = 0;
            var numPayments = Payments.Count();
            while (month < endMonth && currentDebt > 0)
            {
                month = month.AddMonths(1);
                var unscheduledRepayment = IsApplyAllUnscheduledRepayments && month.Month == StartMonth.AddMonths(1).Month
                    ? GetRequiredAmount(InitialPayments.First().InitialDebt * UnscheduledRepaymentPercentage / 100.0, currentDebt)
                    : numPayments >= i
                        ? Payments.ElementAt(i).UnscheduledRepayment
                        : 0.0;
                var payment = new PaymentViewModel(month, GetRequiredAmount(RedemptionAmount, currentDebt - unscheduledRepayment), currentDebt, BorrowingPercentage, unscheduledRepayment);
                currentDebt = payment.ResidualDebt;
                ret.Add(payment);
                i++;
            }

            Payments = ret;
        }

        private static double GetRequiredAmount(double maximumAmount, double currentDebt)
        {
            var c = currentDebt;
            double ret = c >= maximumAmount ? maximumAmount : c;
            return ret;
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
            set { SetProperty(ref _startMonth, value); }
        }
        #endregion

        #region UnscheduledRepaymentPercentage
        private double _unscheduledRepaymentPercentage;

        public double UnscheduledRepaymentPercentage
        {
            get { return _unscheduledRepaymentPercentage; }
            set { _unscheduledRepaymentPercentage = value; }
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
            set { SetProperty(ref _years, value); }
        }

        #endregion

        #region BorrowingPercentage
        private double _BorrowingPercentage;
        public double BorrowingPercentage
        {
            get { return _BorrowingPercentage; }
            set { SetProperty(ref _BorrowingPercentage, value); }
        }
        #endregion

        #region LoanAmount
        private double _loanAmount;
        public double LoanAmount
        {
            get { return _loanAmount; }
            set { SetProperty(ref _loanAmount, value); }
        }
        #endregion


        private IEnumerable<PaymentViewModel> _payments;
        public DelegateCommand ResetCommand { get; private set; }
        public DelegateCommand UpdateCommand { get; private set; }
    }
}
