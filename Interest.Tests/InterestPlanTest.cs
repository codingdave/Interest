using Interest.Options;
using Interest.ViewModels;
using System;
using Xunit;

namespace Interest.Tests
{
    public class InterestPlanTest
    {
        [Fact]
        public void Properties()
        {
            var plan = new InterestPlanViewModel(InterestPlanViewModelOption.GetExample1());

            var starMonth = DateTime.Now;
            var loanAmount = 100.0;
            var borrowingPercentagePerYear = 0.001;
            var monthlyPayment = 50.0;
            var years = 15;
            var unscheduledRepaymentPercentage = .05;
            var redemptionPercentage = Calculator.GetRedemptionPercentage(loanAmount, borrowingPercentagePerYear, monthlyPayment);

            plan.StartMonth = starMonth;
            plan.LoanAmount = loanAmount;
            plan.BorrowingPercentagePerYear = borrowingPercentagePerYear;
            plan.RedemptionPercentage = redemptionPercentage;
            plan.Years = years;
            plan.UnscheduledRepaymentPercentage = unscheduledRepaymentPercentage;

            Assert.Equal(years, plan.Years);
            Assert.Equal(starMonth, plan.StartMonth);
            Assert.Equal(borrowingPercentagePerYear, plan.BorrowingPercentagePerYear);
            Assert.Equal(monthlyPayment, Calculator.GetRedemptionAmount(loanAmount, borrowingPercentagePerYear, plan.RedemptionPercentage));
            Assert.Equal(loanAmount, plan.LoanAmount);
            Assert.Equal(unscheduledRepaymentPercentage, plan.UnscheduledRepaymentPercentage);
        }

        [Fact]
        public void UpdateKeepsProperties()
        {
            var plan = new InterestPlanViewModel(InterestPlanViewModelOption.GetExample1());

            var starMonth = DateTime.Now;
            var loanAmount = 100.0;
            var borrowingPercentagePerYear = 0.001;
            var monthlyPayment = 50.0;
            var years = 15;
            var unscheduledRepaymentPercentage = .05;
            var redemptionPercentage = Calculator.GetRedemptionPercentage(loanAmount, borrowingPercentagePerYear, monthlyPayment);


            plan.StartMonth = starMonth;
            plan.LoanAmount = loanAmount;
            plan.BorrowingPercentagePerYear = borrowingPercentagePerYear;
            plan.RedemptionPercentage = redemptionPercentage;
            plan.Years = years;
            plan.UnscheduledRepaymentPercentage = unscheduledRepaymentPercentage;

            // TODO: Do something or remove that
            var result = plan.Calculate(plan.Payments);

            Assert.Equal(years, plan.Years);
            Assert.Equal(starMonth, plan.StartMonth);
            Assert.Equal(borrowingPercentagePerYear, plan.BorrowingPercentagePerYear);
            Assert.Equal(monthlyPayment, Calculator.GetRedemptionAmount(loanAmount, borrowingPercentagePerYear, plan.RedemptionPercentage));
            Assert.Equal(loanAmount, plan.LoanAmount);
            Assert.Equal(unscheduledRepaymentPercentage, plan.UnscheduledRepaymentPercentage);
        }


        [Fact]
        public void InitializeKeepsProperties()
        {
            var plan = new InterestPlanViewModel(InterestPlanViewModelOption.GetExample1());

            var starMonth = DateTime.Now;
            var loanAmount = 100.0;
            var borrowingPercentagePerYear = 0.001;
            var monthlyPayment = 50.0;
            var years = 15;
            var unscheduledRepaymentPercentage = .05;
            var redemptionPercentage = Calculator.GetRedemptionPercentage(loanAmount, borrowingPercentagePerYear, monthlyPayment);

            plan.StartMonth = starMonth;
            plan.LoanAmount = loanAmount;
            plan.BorrowingPercentagePerYear = borrowingPercentagePerYear;
            plan.RedemptionPercentage = redemptionPercentage;
            plan.Years = years;
            plan.UnscheduledRepaymentPercentage = unscheduledRepaymentPercentage;

            // TODO: Do something or remove that
            var result = plan.Calculate(plan.Payments);

            Assert.Equal(years, plan.Years);
            Assert.Equal(starMonth, plan.StartMonth);
            Assert.Equal(borrowingPercentagePerYear, plan.BorrowingPercentagePerYear);
            Assert.Equal(monthlyPayment, Calculator.GetRedemptionAmount(loanAmount, borrowingPercentagePerYear, plan.RedemptionPercentage));
            Assert.Equal(loanAmount, plan.LoanAmount);
            Assert.Equal(unscheduledRepaymentPercentage, plan.UnscheduledRepaymentPercentage);
        }
    }
}
