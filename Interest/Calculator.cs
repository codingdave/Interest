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

        public static double GetInterest(double reducedDebt, double BorrowingPercentage)
        {
            return reducedDebt * BorrowingPercentage;
        }
        public static double GetBorrowingPercentagePerYear(double BorrowingPercentagePerYear)
        {
            return BorrowingPercentagePerYear / 12 / 100;
        }

        public static double GetResidualDebt(double reducedDebt, double repayment)
        {
            return reducedDebt - repayment;
        }

        public static double GetReducedDebt(double initialDebt, double unscheduledRepayment)
        {
            return initialDebt - unscheduledRepayment;
        }
    }
}
