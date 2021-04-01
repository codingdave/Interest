using System;
using System.Collections.Generic;

namespace Interest.Options
{

    public class Rootobject
    {
        public List<InterestPlanViewModelOptions> InterestPlanViewModelOptions { get; set; } = new List<InterestPlanViewModelOptions>();
        public Logging Logging { get; set; } = new Logging();
    }

    public class Logging
    {
        public Dictionary<string, string> Loglevel { get; set; } = new Dictionary<string, string>();
    }

    public class InterestPlanViewModelOptions
    {
        public const string InterestPlan = "InterestPlan";

        public InterestPlanViewModelOptions()
        {
        }

        public static InterestPlanViewModelOptions GetDefault()
        {
            var i = new InterestPlanViewModelOptions();
            i.BorrowingPercentagePerYear = .99;
            i.LoanAmount = 250000;
            i.RedemptionFreeMonths = 0;
            i.RedemptionPercentage = 2.5;
            i.StartMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            i.Lender = "Unknown Lender";
            i.UnscheduledRepaymentPercentage = 5;
            i.Years = 15;
            return i;
        }

        public DateTime _startMonth;
        public int _years;
        public double _unscheduledRepaymentPercentage;
        public double _borrowingPercentagePerYear;
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

        public double BorrowingPercentagePerYear
        {
            get { return _borrowingPercentagePerYear; }
            set { _borrowingPercentagePerYear = value; }
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