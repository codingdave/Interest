using Interest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Interest.Tests
{
    public class CalculatorTest
    {
        [Theory]
        [InlineData(20)]
        public void StandardPlan(int years)
        {
            IEnumerable<PaymentViewModel> previousPayments = new List<PaymentViewModel>();
            var payments = Calculator.GetPaymentPlan(
                previousPayments: previousPayments,
                startMonth: DateTime.Now,
                years: years,
                loan: 100000,
                percentagePerYear: .77,
                redemptionPercentage: 5,
                redemptionFreeMonths: 12,
                isApplyAllUnscheduledRepayments: false,
                unscheduledRepaymentPercentage: 5,
                isFullRepayment: false).ToList();

            Assert.True(payments.Count <= years * 12, "payment plan has at most as many entries as months but can stop before if mortage is payed back");
            // interest increases, residual dept decreases
            for (int i = 1; i < payments.Count; ++i)
            {
                var p_prev = payments[i - 1];
                var p_now = payments[i];

                if (p_prev.Repayment > 0)
                {
                    Assert.True(p_prev.ResidualDebt > p_now.ResidualDebt, "Expected previous debt to be greater than next debt.");
                    Assert.True(p_prev.Interest > p_now.Interest, "Expected previous interest to be greater than next interest.");
                }
            }
        }
    }
}
