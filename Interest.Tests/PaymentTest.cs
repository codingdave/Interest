using Interest.Models;
using Interest.Types;
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
        public void GetProperties(double monthlyPayment, double initialDebt, double borrowing, double unscheduledRepaymentValue)
        {
            var date = new DateTime(2020, 04, 01);
            var unscheduledRepayment = new Currency(unscheduledRepaymentValue, InputKind.Auto);
            var initialDebt1 = new Currency(initialDebt);
            var borrowing1 = new Percentage(borrowing);
            var reducedDebt = Calculator.GetReducedDebt(initialDebt1, new Currency(unscheduledRepayment));
            var interest = Calculator.GetInterestCostPerMonth(reducedDebt, borrowing1);

            var p = new PaymentModel(date, new Currency(monthlyPayment, InputKind.Auto), initialDebt1, borrowing1, unscheduledRepayment, reducedDebt, interest);
            Assert.Equal(date, p.Date);
            Assert.Equal(monthlyPayment, p.Payment.Value);
        }

        [Fact]
        public void NegativeMonthlyPaymentThrows()
        {
            var date = new DateTime(2020, 04, 01);
            var monthlyPayment = -1.1;
            var unscheduledRepayment = new Currency(0, InputKind.Auto);
            Assert.Throws<ArgumentOutOfRangeException>(() => new PaymentModel(date, new Currency(monthlyPayment, InputKind.Auto), new Currency(100), new Percentage(.001), unscheduledRepayment, new Currency(5000), new Currency(1)));
        }

        [Fact]
        public void NegativeDebtThrows()
        {
            var date = new DateTime(2020, 04, 01);
            var monthlyPayment = 1.1;
            var debt = -1.1;
            var unscheduledRepayment = new Currency(0, InputKind.Auto);

            Assert.Throws<ArgumentOutOfRangeException>(() => new PaymentModel(date, new Currency(monthlyPayment, InputKind.Auto), new Currency(debt), new Percentage(.001), unscheduledRepayment, new Currency(5000), new Currency(1)));
        }

        [Fact]
        public void NegativeBorrowingPercentageThrows()
        {
            var date = new DateTime(2020, 04, 01);
            var monthlyPayment = 1.1;
            var debt = 1.1;
            var borrowing = -.0011;
            var unscheduledRepayment = new Currency(0, InputKind.Auto);

            Assert.Throws<ArgumentOutOfRangeException>(() => new PaymentModel(date, new Currency(monthlyPayment, InputKind.Auto), new Currency(debt), new Percentage(borrowing), unscheduledRepayment, new Currency(5000), new Currency(1)));
        }

        [Fact]
        public void NegativeUnscheduledRepaymentThrows()
        {
            var date = new DateTime(2020, 04, 01);
            var monthlyPayment = 1.1;
            var debt = 1.1;
            var borrowingPercentagePerYear = 1.1;
            var unscheduledRepayment = new Currency(-0.0011, InputKind.Auto);

            Assert.Throws<ArgumentOutOfRangeException>(() => new PaymentModel(date, new Currency(monthlyPayment, InputKind.Auto), new Currency(debt), new Percentage(borrowingPercentagePerYear), unscheduledRepayment, new Currency(5000), new Currency(1)));
        }

    }
}
