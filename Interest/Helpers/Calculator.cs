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

        public static double GetInterest(double reducedDebt, double borrowingPercentagePerYear)
        {
            return reducedDebt * GetBorrowingPercentage(borrowingPercentagePerYear);
        }

        public static double GetBorrowingPercentage(double borrowingPercentagePerYear)
        {
            return borrowingPercentagePerYear / 12 / 100;
        }

        public static double GetResidualDebt(double reducedDebt, double repayment)
        {
            return Math.Max(reducedDebt - repayment, 0);
        }

        public static double GetReducedDebt(double initialDebt, InputValue<double> unscheduledRepayment)
        {
            return initialDebt - unscheduledRepayment.Value;
        }
    }
}
