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
        public void GetProperties(double monthlyPayment, double initialDebt, double borrowingPercentagePerYear, double unscheduledRepaymentValue)
        {
            var date = new DateTime(2020, 04, 01);
            var unscheduledRepayment = new InputValue<double>(unscheduledRepaymentValue, InputType.Auto);
            var reducedDebt = Calculator.GetReducedDebt(initialDebt, unscheduledRepayment);
            var interest = Calculator.GetInterestCostPerMonth(reducedDebt, borrowingPercentagePerYear);

            var p = new PaymentModel(date, new InputValue<double>(monthlyPayment, InputType.Auto), initialDebt, borrowingPercentagePerYear, unscheduledRepayment, reducedDebt, interest);
            Assert.Equal(date, p.Date);
            Assert.Equal(monthlyPayment, p.Payment.Value);
        }

        [Fact]
        public void NegativeMonthlyPaymentThrows()
        {
            var date = new DateTime(2020, 04, 01);
            var monthlyPayment = -1.1;
            var unscheduledRepayment = new InputValue<double>(0, InputType.Auto);
            Assert.Throws<ArgumentOutOfRangeException>(() => new PaymentModel(date, new InputValue<double>(monthlyPayment, InputType.Auto), 100, .001, unscheduledRepayment, 5000, 1));
        }

        [Fact]
        public void NegativeDebtThrows()
        {
            var date = new DateTime(2020, 04, 01);
            var monthlyPayment = 1.1;
            var debt = -1.1;
            var unscheduledRepayment = new InputValue<double>(0, InputType.Auto);

            Assert.Throws<ArgumentOutOfRangeException>(() => new PaymentModel(date, new InputValue<double>(monthlyPayment, InputType.Auto), debt, .001, unscheduledRepayment, 5000, 1));
        }

        [Fact]
        public void NegativeBorrowingPercentageThrows()
        {
            var date = new DateTime(2020, 04, 01);
            var monthlyPayment = 1.1;
            var debt = 1.1;
            var borrowingPercentagePerYear = -.0011;
            var unscheduledRepayment = new InputValue<double>(0, InputType.Auto);

            Assert.Throws<ArgumentOutOfRangeException>(() => new PaymentModel(date, new InputValue<double>(monthlyPayment, InputType.Auto), debt, borrowingPercentagePerYear, unscheduledRepayment, 5000, 1));
        }

        [Fact]
        public void NegativeUnscheduledRepaymentThrows()
        {
            var date = new DateTime(2020, 04, 01);
            var monthlyPayment = 1.1;
            var debt = 1.1;
            var borrowingPercentagePerYear = 1.1;
            var unscheduledRepayment = new InputValue<double>(-0.0011, InputType.Auto);

            Assert.Throws<ArgumentOutOfRangeException>(() => new PaymentModel(date, new InputValue<double>(monthlyPayment, InputType.Auto), debt, borrowingPercentagePerYear, unscheduledRepayment, 5000, 1));
        }

    }
}
