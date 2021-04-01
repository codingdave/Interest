using System;

namespace Interest.Options
{
    public class InterestPlanViewModelOptions
    {
        public const string InterestPlan = "InterestPlan";

        public InterestPlanViewModelOptions()
        {
            BorrowingPercentagePerYear = .99;
            LoanAmount = 250000;
            RedemptionFreeMonths = 12;
            RedemptionPercentage = 2.5;
            StartMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            Title = "No Title";
            UnscheduledRepaymentPercentage = 5;
            Years = 15;
        }

        public DateTime StartMonth;
        public int Years;
        public double UnscheduledRepaymentPercentage;
        public double BorrowingPercentagePerYear;
        public double RedemptionPercentage;
        public double LoanAmount;
        public string Title;
        public int RedemptionFreeMonths;
    }
}