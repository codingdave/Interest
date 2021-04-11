using System;

namespace Interest.Options
{

    public class InterestPlanViewModelOption
    {
        public static InterestPlanViewModelOption GetExample1()
        {
            return new InterestPlanViewModelOption
            {
                BorrowingPercentage = .77,
                LoanAmount = 100000,
                RedemptionFreeMonths = 12,
                RedemptionPercentage = 2.5,
                StartMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                Lender = "Example 1",
                UnscheduledRepaymentPercentage = 5,
                Years = 20
            };
        }

        public static InterestPlanViewModelOption GetExample2()
        {
            return new InterestPlanViewModelOption
            {
                BorrowingPercentage = .99,
                LoanAmount = 250000,
                RedemptionFreeMonths = 0,
                RedemptionPercentage = 2.5,
                StartMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                Lender = "Example 2",
                UnscheduledRepaymentPercentage = 5,
                Years = 15
            };
        }

        public DateTime _startMonth;
        public int _years;
        public double _unscheduledRepaymentPercentage;
        public double _borrowingPercentage;
        public double _redemptionPercentage;
        public double _loanAmount;
        public string _lender;
        public int _redemptionFreeMonths;
        public DateTime StartMonth
        {
            get { return _startMonth; }
            set { _startMonth = value; }
        }

        public int Years
        {
            get { return _years; }
            set { _years = value; }
        }

        public double UnscheduledRepaymentPercentage
        {
            get { return _unscheduledRepaymentPercentage; }
            set { _unscheduledRepaymentPercentage = value; }
        }

        public double BorrowingPercentage
        {
            get { return _borrowingPercentage; }
            set { _borrowingPercentage = value; }
        }

        public double RedemptionPercentage
        {
            get { return _redemptionPercentage; }
            set { _redemptionPercentage = value; }
        }

        public double LoanAmount
        {
            get { return _loanAmount; }
            set { _loanAmount = value; }
        }

        public string Lender
        {
            get { return _lender; }
            set { _lender = value; }
        }

        public int RedemptionFreeMonths
        {
            get { return _redemptionFreeMonths; }
            set { _redemptionFreeMonths = value; }
        }

        public override string ToString()
        {
            return Lender;
        }
    }
}