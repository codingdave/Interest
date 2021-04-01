using Interest.Models;
using System;
using Xunit;

namespace Interest.Tests
{
    public class PaymentTest
    {
        [Theory]
        [InlineData(1000.0, 10000.0, 0.01, 100.0)]
        [InlineData(1000.0, 10000.0, 0.001, 100.0)]
        [InlineData(1000.0, 10000.0, 0.01, 0.0)]
        [InlineData(10000.0, 10000.0, 0.05, 0)]
        [InlineData(10000.0, 10000.0, 0.001, 0)]
        public void GetProperties(double monthlyPayment, double initialDebt, double BorrowingPercentagePerYear, double unscheduledRepaymentValue)
        {
            var date = new DateTime(2020, 04, 01);
            var unscheduledRepayment = new UnscheduledRepayment(unscheduledRepaymentValue, false);
            var reducedDebt = Calculator.GetReducedDebt(initialDebt, unscheduledRepayment);
            var interest = Calculator.GetInterest(reducedDebt, BorrowingPercentagePerYear);

            var p = new Payment(date, monthlyPayment, initialDebt, BorrowingPercentagePerYear, unscheduledRepayment, reducedDebt, interest);
            Assert.Equal(date, p.Month);
            Assert.Equal(monthlyPayment, p.MonthlyPayment);
            Assert.Equal(initialDebt, p.Debt);
        }

        [Fact]
        public void NegativeMonthlyPaymentThrows()
        {
            var date = new DateTime(2020, 04, 01);
            var monthlyPayment = -1.1;
            var unscheduledRepayment = new UnscheduledRepayment(0, false);
            Assert.Throws<ArgumentOutOfRangeException>(() => new Payment(date, monthlyPayment, 100, .001, unscheduledRepayment, 5000, 1));
        }

        [Fact]
        public void NegativeDebtThrows()
        {
            var date = new DateTime(2020, 04, 01);
            var monthlyPayment = 1.1;
            var debt = -1.1;
            var unscheduledRepayment = new UnscheduledRepayment(0, false);

            Assert.Throws<ArgumentOutOfRangeException>(() => new Payment(date, monthlyPayment, debt, .001, unscheduledRepayment, 5000, 1));
        }

        [Fact]
        public void NegativeBorrowingPercentageThrows()
        {
            var date = new DateTime(2020, 04, 01);
            var monthlyPayment = 1.1;
            var debt = 1.1;
            var borrowingPercentagePerYear = -.0011;
            var unscheduledRepayment = new UnscheduledRepayment(0, false);

            Assert.Throws<ArgumentOutOfRangeException>(() => new Payment(date, monthlyPayment, debt, borrowingPercentagePerYear, unscheduledRepayment, 5000, 1));
        }

        [Fact]
        public void NegativeUnscheduledRepaymentThrows()
        {
            var date = new DateTime(2020, 04, 01);
            var monthlyPayment = 1.1;
            var debt = 1.1;
            var borrowingPercentagePerYear = 1.1;
            var unscheduledRepayment = new UnscheduledRepayment(-0.0011, false);

            Assert.Throws<ArgumentOutOfRangeException>(() => new Payment(date, monthlyPayment, debt, borrowingPercentagePerYear, unscheduledRepayment, 5000, 1));
        }

    }
}
