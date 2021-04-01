using Interest.Models;
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
            var plan = new InterestPlanViewModel();

            var starMonth = DateTime.Now;
            var loanAmount = 100.0;
            var borrowingRate = 0.001;
            var monthlyPayment = 50.0;
            var years = 15;
            var unscheduledRepaymentPercentage = .05;

            plan.StartMonth = starMonth;
            plan.LoanAmount = loanAmount;
            plan.BorrowingRate = borrowingRate;
            plan.MonthlyPayment = monthlyPayment;
            plan.Years = years;
            plan.UnscheduledRepaymentPercentage = unscheduledRepaymentPercentage;

            Assert.Equal(years, plan.Years);
            Assert.Equal(starMonth, plan.StartMonth);
            Assert.Equal(borrowingRate, plan.BorrowingRate);
            Assert.Equal(monthlyPayment, plan.MonthlyPayment);
            Assert.Equal(loanAmount, plan.LoanAmount);
            Assert.Equal(unscheduledRepaymentPercentage, plan.UnscheduledRepaymentPercentage);
        }

        [Fact]
        public void UpdateKeepsProperties()
        {
            var plan = new InterestPlanViewModel();

            var starMonth = DateTime.Now;
            var loanAmount = 100.0;
            var borrowingRate = 0.001;
            var monthlyPayment = 50.0;
            var years = 15;
            var unscheduledRepaymentPercentage = .05;

            plan.StartMonth = starMonth;
            plan.LoanAmount = loanAmount;
            plan.BorrowingRate = borrowingRate;
            plan.MonthlyPayment = monthlyPayment;
            plan.Years = years;
            plan.UnscheduledRepaymentPercentage = unscheduledRepaymentPercentage;

            plan.Update();

            Assert.Equal(years, plan.Years);
            Assert.Equal(starMonth, plan.StartMonth);
            Assert.Equal(borrowingRate, plan.BorrowingRate);
            Assert.Equal(monthlyPayment, plan.MonthlyPayment);
            Assert.Equal(loanAmount, plan.LoanAmount);
            Assert.Equal(unscheduledRepaymentPercentage, plan.UnscheduledRepaymentPercentage);
        }


        [Fact]
        public void InitializeKeepsProperties()
        {
            var plan = new InterestPlanViewModel();

            var starMonth = DateTime.Now;
            var loanAmount = 100.0;
            var borrowingRate = 0.001;
            var monthlyPayment = 50.0;
            var years = 15;
            var unscheduledRepaymentPercentage = .05;

            plan.StartMonth = starMonth;
            plan.LoanAmount = loanAmount;
            plan.BorrowingRate = borrowingRate;
            plan.MonthlyPayment = monthlyPayment;
            plan.Years = years;
            plan.UnscheduledRepaymentPercentage = unscheduledRepaymentPercentage;

            plan.Initialize();

            Assert.Equal(years, plan.Years);
            Assert.Equal(starMonth, plan.StartMonth);
            Assert.Equal(borrowingRate, plan.BorrowingRate);
            Assert.Equal(monthlyPayment, plan.MonthlyPayment);
            Assert.Equal(loanAmount, plan.LoanAmount);
            Assert.Equal(unscheduledRepaymentPercentage, plan.UnscheduledRepaymentPercentage);
        }
    }
}
