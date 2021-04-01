using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Prism.Commands;
using Prism.Mvvm;

namespace Interest.ViewModels
{
    public class InterestPlanViewModel : BindableBase
    {
        public InterestPlanViewModel()
        {

            var now = DateTime.Now;
            _startMonth = new DateTime(now.Year, now.Month, 1);

            CreateCommand = new DelegateCommand(() =>
            {
                InitialPayments = Initialize();
                Payments = Initialize();
            });

            UpdateCommand = new DelegateCommand(() =>
            {
                Update();
            });

            CreateCommand.Execute();
        }

        public IEnumerable<PaymentViewModel> Initialize()
        {
            var ret = new List<PaymentViewModel>();

            var month = StartMonth;
            var endMonth = StartMonth.AddYears(Years);
            var monthlyPayment = MonthlyPayment;

            double unscheduledRepayment = 0;
            while (month < endMonth)
            {
                month = month.AddMonths(1);
                var residualDebt = ret.Any() ? ret.Last().ResidualDebt : LoanAmount;

                var remainingDebtPlusRate = residualDebt + residualDebt * BorrowingRate / 12.0;
                if (remainingDebtPlusRate >= monthlyPayment)
                {
                    var p = new PaymentViewModel(month, monthlyPayment, residualDebt, BorrowingRate, unscheduledRepayment);
                    ret.Add(p);
                }
                else
                {
                    monthlyPayment = remainingDebtPlusRate;
                    var p = new PaymentViewModel(month, monthlyPayment, residualDebt, BorrowingRate, unscheduledRepayment);
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
            foreach (var p in Payments)
            {
                if (currentDebt <= 0)
                {
                    break;
                }

                var unscheduledRepayment = p.UnscheduledRepayment;
                if (IsApplyAllUnscheduledRepayments && p.Month.Month == StartMonth.AddMonths(1).Month)
                {
                    unscheduledRepayment = GetRequiredAmount(InitialPayments.First().InitialDebt * UnscheduledRepaymentPercentage, currentDebt);
                }

                var payment = new PaymentViewModel(p.Month, GetRequiredAmount(MonthlyPayment, currentDebt - unscheduledRepayment), currentDebt, BorrowingRate, unscheduledRepayment);
                currentDebt = payment.ResidualDebt;
                ret.Add(payment);
            }

            Payments = ret;
        }

        private double GetRequiredAmount(double maximumAmount, double currentDebt)
        {
            var c = currentDebt;
            var nextRate = maximumAmount/* + (maximumAmount * BorrowingRate / 12)*/;
            double ret = c >= nextRate ? maximumAmount : c /*+ (c * BorrowingRate  / 12)*/;
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

        private DateTime _startMonth;

        public DateTime StartMonth
        {
            get { return _startMonth; }
            set { SetProperty(ref _startMonth, value); }
        }

        #region UnscheduledRepaymentPercentage
        private double _unscheduledRepaymentPercentage = 0.05;

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
        private int _years = 20;
        public int Years
        {
            get { return _years; }
            set { SetProperty(ref _years, value); }
        }

        #endregion
        private double _borrowingRate = 0.0084;
        public double BorrowingRate
        {
            get { return _borrowingRate; }
            set { SetProperty(ref _borrowingRate, value); }
        }

        private double _loanAmount = 381000;
        public double LoanAmount
        {
            get { return _loanAmount; }
            set { SetProperty(ref _loanAmount, value); }
        }

        private double _monthlyPayment = 1139.83;
        private IEnumerable<PaymentViewModel> _payments;

        public double MonthlyPayment
        {
            get { return _monthlyPayment; }
            set { SetProperty(ref _monthlyPayment, value); }
        }

        public DelegateCommand CreateCommand { get; private set; }
        public DelegateCommand UpdateCommand { get; private set; }
    }
}
