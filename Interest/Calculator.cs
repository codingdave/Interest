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

        public static double GetInterest(double reducedDebt, double borrowingRate)
        {
            return reducedDebt * borrowingRate;
        }
        public static double GetBorrowingRatePerYear(double borrowingRatePerYear)
        {
            return borrowingRatePerYear / 12;
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
