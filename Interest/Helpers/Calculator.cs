using Interest.Types;
using Interest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interest
{
    public static class Calculator
    {
        public static Currency GetRepayment(Currency totalPayment, Currency interest)
        {
            return totalPayment - interest;
        }

        public static Currency GetInterestCostPerMonth(Currency reducedDebt, Percentage interest)
        {
            return reducedDebt * interest.PerMonthAsFraction;
        }

        public static Currency GetResidualDebt(Currency reducedDebt, Currency repayment)
        {
            return Currency.Max(reducedDebt - repayment, 0);
        }

        public static Currency GetReducedDebt(Currency initialDebt, Currency unscheduledRepayment)
        {
            return initialDebt - unscheduledRepayment;
        }

        public static IEnumerable<PaymentViewModel> GetPaymentPlan(
            IEnumerable<PaymentViewModel> previousPayments,
            DateTime startMonth, int years, Currency loan,
            Percentage borrowing,
            Percentage redemption, int redemptionFreeMonths,
            bool isApplyAllUnscheduledRepayments,
            Percentage unscheduledRepaymentPercentage,
            bool isFullRepayment)
        {
            var ret = new List<PaymentViewModel>();
            var date = startMonth.AddMonths(1);
            var endMonth = startMonth.AddYears(years);
            var residualDebt = loan;
            PaymentViewModel p;
            var redemptionAmount = GetRedemptionAmount(loan, borrowing, redemption);
            var index = 1;
            var unscheduledRepayment = new Currency();
            while (date < endMonth && residualDebt.Value > 0)
            {
                if (ret.Count < redemptionFreeMonths)
                {
                    // we only pay interest, no redemption
                    p = new PaymentViewModel(index++, date, residualDebt, borrowing);
                }
                else
                {
                    Currency payment;
                    if (isFullRepayment)
                    {
                        // payment: 471.60
                        // interest_0: 64.17
                        // full repayment
                        var borrowingRatePerYear = borrowing.PerYearAsFraction;
                        var totalInterestFactor = Math.Pow(1.0 + borrowingRatePerYear, years);
                        var totalnterestPercentage = totalInterestFactor / (totalInterestFactor - 1);
                        var interestRatePerYear = borrowingRatePerYear * totalnterestPercentage;
                        var interestPercentage = new Percentage(interestRatePerYear * 100);
                        payment = GetInterestCostPerMonth(GetReducedDebt(residualDebt, unscheduledRepayment), interestPercentage);
                        var debt = GetReducedDebt(residualDebt, unscheduledRepayment);
                        var reducedDebt = debt;
                        p = new PaymentViewModel(index++, date, payment, debt, interestPercentage, unscheduledRepayment,
                                                 reducedDebt, payment * 12);
                    }
                    else
                    {
                        if (previousPayments?.Count() > ret.Count)
                        {
                            // we keep manual modifications
                            var oldPayment = previousPayments.ElementAt(ret.Count);
                            unscheduledRepayment = oldPayment.UnscheduledRepayment.Kind == InputKind.Manual ? oldPayment.UnscheduledRepayment : new Currency();
                            payment = oldPayment.Payment.Kind == InputKind.Manual ? oldPayment.Payment : new Currency(redemptionAmount);
                        }
                        else
                        {
                            payment = new Currency(redemptionAmount);
                        }

                        if (unscheduledRepayment.Kind == InputKind.Auto && isApplyAllUnscheduledRepayments && date.Month == startMonth.AddMonths(1).Month)
                        {
                            // on optimize we override everythings
                            unscheduledRepayment = new Currency(loan * unscheduledRepaymentPercentage.PerMonthAsFraction);
                        }
                        unscheduledRepayment = Currency.Min(unscheduledRepayment, residualDebt, unscheduledRepayment.Kind);

                        p = new PaymentViewModel(index++, date, payment, residualDebt, borrowing, unscheduledRepayment);
                    }
                }

                ret.Add(p);
                residualDebt = p.ResidualDebt;
                date = date.AddMonths(1);
            }
            return ret;
        }

        public static Currency GetRedemptionAmount(
            Currency loan,
            Percentage percentage,
            Percentage redemption)
        {
            return loan * (percentage.PerMonthAsFraction + redemption.PerMonthAsFraction);
        }

        public static Percentage GetRedemptionPercentage(Currency loan, Percentage percentage, Currency redemptionAmount)
        {
            return new Percentage(redemptionAmount / loan * 100.0 * 12.0) - percentage;
        }
    }
}
