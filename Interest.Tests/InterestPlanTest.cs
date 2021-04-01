using Interest.Models;
using Interest.Options;
using Interest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Interest.Tests
{
    public class InterestPlanTest
    {
        [Fact]
        public void Properties()
        {
            var plan = new InterestPlanViewModel(InterestPlanViewModelOptions.GetDefault());

            var starMonth = DateTime.Now;
            var loanAmount = 100.0;
            var borrowingPercentagePerYear = 0.001;
            var monthlyPayment = 50.0;
            var years = 15;
            var unscheduledRepaymentPercentage = .05;

            plan.StartMonth = starMonth;
            plan.LoanAmount = loanAmount;
            plan.BorrowingPercentagePerYear = borrowingPercentagePerYear;
            plan.RedemptionAmount = monthlyPayment;
            plan.Years = years;
            plan.UnscheduledRepaymentPercentage = unscheduledRepaymentPercentage;

            Assert.Equal(years, plan.Years);
            Assert.Equal(starMonth, plan.StartMonth);
            Assert.Equal(borrowingPercentagePerYear, plan.BorrowingPercentagePerYear);
            Assert.Equal(monthlyPayment, plan.RedemptionAmount);
            Assert.Equal(loanAmount, plan.LoanAmount);
            Assert.Equal(unscheduledRepaymentPercentage, plan.UnscheduledRepaymentPercentage);
        }

        [Fact]
        public void UpdateKeepsProperties()
        {
            var plan = new InterestPlanViewModel(InterestPlanViewModelOptions.GetDefault());

            var starMonth = DateTime.Now;
            var loanAmount = 100.0;
            var borrowingPercentagePerYear = 0.001;
            var monthlyPayment = 50.0;
            var years = 15;
            var unscheduledRepaymentPercentage = .05;

            plan.StartMonth = starMonth;
            plan.LoanAmount = loanAmount;
            plan.BorrowingPercentagePerYear = borrowingPercentagePerYear;
            plan.RedemptionAmount = monthlyPayment;
            plan.Years = years;
            plan.UnscheduledRepaymentPercentage = unscheduledRepaymentPercentage;

            plan.Calculate();

            Assert.Equal(years, plan.Years);
            Assert.Equal(starMonth, plan.StartMonth);
            Assert.Equal(borrowingPercentagePerYear, plan.BorrowingPercentagePerYear);
            Assert.Equal(monthlyPayment, plan.RedemptionAmount);
            Assert.Equal(loanAmount, plan.LoanAmount);
            Assert.Equal(unscheduledRepaymentPercentage, plan.UnscheduledRepaymentPercentage);
        }


        [Fact]
        public void InitializeKeepsProperties()
        {
            var plan = new InterestPlanViewModel(InterestPlanViewModelOptions.GetDefault());

            var starMonth = DateTime.Now;
            var loanAmount = 100.0;
            var borrowingPercentagePerYear = 0.001;
            var monthlyPayment = 50.0;
            var years = 15;
            var unscheduledRepaymentPercentage = .05;

            plan.StartMonth = starMonth;
            plan.LoanAmount = loanAmount;
            plan.BorrowingPercentagePerYear = borrowingPercentagePerYear;
            plan.RedemptionAmount = monthlyPayment;
            plan.Years = years;
            plan.UnscheduledRepaymentPercentage = unscheduledRepaymentPercentage;

            plan.Calculate();

            Assert.Equal(years, plan.Years);
            Assert.Equal(starMonth, plan.StartMonth);
            Assert.Equal(borrowingPercentagePerYear, plan.BorrowingPercentagePerYear);
            Assert.Equal(monthlyPayment, plan.RedemptionAmount);
            Assert.Equal(loanAmount, plan.LoanAmount);
            Assert.Equal(unscheduledRepaymentPercentage, plan.UnscheduledRepaymentPercentage);
        }
    }
}
