using Interest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interest
{
    public static class Calculator
    {
        public static double GetRepayment(double monthlyPayment, double interest)
        {
            return monthlyPayment - interest;
        }

        public static double GetInterestCostPerMonth(double reducedDebt, double interestPercentagePerYear)
        {
            return reducedDebt * GetPercentagePerMonth(interestPercentagePerYear);
        }

        public static double GetPercentagePerMonth(double interestPercentagePerYear)
        {
            return interestPercentagePerYear / 12 / 100;
        }

        public static double GetResidualDebt(double reducedDebt, double repayment)
        {
            return Math.Max(reducedDebt - repayment, 0);
        }

        public static double GetReducedDebt(double initialDebt, InputValue<double> unscheduledRepayment)
        {
            return initialDebt - unscheduledRepayment.Value;
        }

        internal static IEnumerable<PaymentViewModel> GetPaymentPlan(
            IEnumerable<PaymentViewModel> previousPayments,
            DateTime startMonth, int years, double loan, double percentagePerYear,
            double unscheduledRepaymentPercentage,
            double redemptionAmount, int redemptionFreeMonths,
            bool isApplyAllUnscheduledRepayments, bool isFullRepayment,
            Action<object> updateCalculation)
        {
            var ret = new List<PaymentViewModel>();
            var date = startMonth;
            var endMonth = startMonth.AddYears(years);
            var residualDebt = loan;
            var borrowingPercentagePerYear = percentagePerYear;
            PaymentViewModel p;

            InputValue<double> unscheduledRepayment = new InputValue<double>(0, InputType.Auto);
            InputValue<double> payment = default;
            while (date < endMonth && residualDebt > 0)
            {
                date = date.AddMonths(1);

                if (ret.Count < redemptionFreeMonths)
                {
                    // we only pay interest, no redemption
                    p = new PaymentViewModel(date, residualDebt, borrowingPercentagePerYear, updateCalculation);
                }
                else
                {
                    if (isFullRepayment)
                    {
                        // payment: 471.60
                        // interest_0: 64.17
                        // full repayment
                        var borrowingRatePerYear = borrowingPercentagePerYear / 100.0;
                        var totalInterestFactor = Math.Pow(1.0 + borrowingRatePerYear, years);
                        var totalnterestPercentage = totalInterestFactor / (totalInterestFactor - 1);
                        var interestRatePerYear = borrowingRatePerYear * totalnterestPercentage;
                        var interestPercentagePerYear = interestRatePerYear * 100;
                        payment = new InputValue<double>(
                            GetInterestCostPerMonth(
                                GetReducedDebt(residualDebt, unscheduledRepayment), interestPercentagePerYear), InputType.Auto);
                        var debt = GetReducedDebt(residualDebt, unscheduledRepayment);

                        p = new PaymentViewModel(date, payment, debt, interestPercentagePerYear, unscheduledRepayment,
                            debt, interestPercentagePerYear,
                            updateCalculation);
                    }
                    else
                    {
                        if (previousPayments?.Count() > ret.Count)
                        {
                            // we keep manual modifications
                            var oldPayment = previousPayments.ElementAt(ret.Count);
                            unscheduledRepayment = oldPayment.UnscheduledRepayment.InputType == InputType.Manual ? oldPayment.UnscheduledRepayment : default;
                            payment = oldPayment.Payment.InputType == InputType.Manual ? oldPayment.Payment : new InputValue<double>(redemptionAmount, InputType.Auto);
                        }

                        if (unscheduledRepayment.InputType == InputType.Auto && isApplyAllUnscheduledRepayments && date.Month == startMonth.AddMonths(1).Month)
                        {
                            // on optimize we override everythings
                            unscheduledRepayment = new InputValue<double>(loan * unscheduledRepaymentPercentage / 100.0, InputType.Auto);
                        }
                        unscheduledRepayment = new InputValue<double>(Math.Min(unscheduledRepayment.Value, residualDebt), unscheduledRepayment.InputType);

                        p = new PaymentViewModel(date, payment, residualDebt, borrowingPercentagePerYear, unscheduledRepayment, updateCalculation);
                    }
                }

                ret.Add(p);
                residualDebt = p.ResidualDebt;
            }
            return ret;
        }
    }
}
