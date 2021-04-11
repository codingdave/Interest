using Interest.Options;
using Interest.Types;
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
            var loan = new Currency(100.0);
            var borrowing = new Percentage(0.001);
            var monthlyPayment = new Currency(50.0);
            var years = 15;
            var unscheduledRepayment = new Percentage(.05);
            var redemptionPercentage = Calculator.GetRedemptionPercentage(loan, borrowing, monthlyPayment);

            plan.StartMonth = starMonth;
            plan.LoanAmount = loan;
            plan.Borrowing = borrowing;
            plan.RedemptionRate = redemptionPercentage;
            plan.Years = years;
            plan.UnscheduledRepayment = unscheduledRepayment;

            Assert.Equal(years, plan.Years);
            Assert.Equal(starMonth, plan.StartMonth);
            Assert.Equal(borrowing, plan.Borrowing);
            Assert.Equal(monthlyPayment, Calculator.GetRedemptionAmount(loan, borrowing, plan.RedemptionRate));
            Assert.Equal(loan, plan.LoanAmount);
            Assert.Equal(unscheduledRepayment, plan.UnscheduledRepayment);
        }

        [Fact]
        public void UpdateKeepsProperties()
        {
            var plan = new InterestPlanViewModel(InterestPlanViewModelOption.GetExample1());

            var starMonth = DateTime.Now;
            var loanAmount = new Currency(100.0);
            var borrowingPercentage = new Percentage(0.001);
            var monthlyPayment = new Currency(50.0);
            var years = 15;
            var unscheduledRepayment = new Percentage(.05);
            var redemptionPercentage = Calculator.GetRedemptionPercentage(loanAmount, borrowingPercentage, monthlyPayment);


            plan.StartMonth = starMonth;
            plan.LoanAmount = loanAmount;
            plan.Borrowing = borrowingPercentage;
            plan.RedemptionRate = redemptionPercentage;
            plan.Years = years;
            plan.UnscheduledRepayment = unscheduledRepayment;

            // TODO: Do something or remove that
            var result = plan.Calculate(plan.Payments);

            Assert.Equal(years, plan.Years);
            Assert.Equal(starMonth, plan.StartMonth);
            Assert.Equal(borrowingPercentage, plan.Borrowing);
            Assert.Equal(monthlyPayment, Calculator.GetRedemptionAmount(loanAmount, borrowingPercentage, plan.RedemptionRate));
            Assert.Equal(loanAmount, plan.LoanAmount);
            Assert.Equal(unscheduledRepayment, plan.UnscheduledRepayment);
        }


        [Fact]
        public void InitializeKeepsProperties()
        {
            var plan = new InterestPlanViewModel(InterestPlanViewModelOption.GetExample1());

            var starMonth = DateTime.Now;
            var loanAmount = new Currency(100.0);
            var borrowingPercentage = new Percentage(0.001);
            var monthlyPayment = new Currency(50.0);
            var years = 15;
            var unscheduledRepayment = new Percentage(.05);
            var redemptionPercentage = Calculator.GetRedemptionPercentage(loanAmount, borrowingPercentage, monthlyPayment);

            plan.StartMonth = starMonth;
            plan.LoanAmount = loanAmount;
            plan.Borrowing = borrowingPercentage;
            plan.RedemptionRate = redemptionPercentage;
            plan.Years = years;
            plan.UnscheduledRepayment = unscheduledRepayment;

            // TODO: Do something or remove that
            var result = plan.Calculate(plan.Payments);

            Assert.Equal(years, plan.Years);
            Assert.Equal(starMonth, plan.StartMonth);
            Assert.Equal(borrowingPercentage, plan.Borrowing);
            Assert.Equal(monthlyPayment, Calculator.GetRedemptionAmount(loanAmount, borrowingPercentage, plan.RedemptionRate));
            Assert.Equal(loanAmount, plan.LoanAmount);
            Assert.Equal(unscheduledRepayment, plan.UnscheduledRepayment);
        }
    }
}
