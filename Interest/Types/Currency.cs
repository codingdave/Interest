using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interest.Types
{
    public class Currency
    {
        public Currency(double value = 0, InputKind kind = InputKind.Auto)
        {
            Value = value;
            Kind = kind;
        }

        public Currency(Currency interest) : this(interest.Value)
        {
        }

        public Currency(Currency interest, InputKind kind) : this(interest.Value, kind)
        {
        }

        public double Value { get; internal set; }
        public InputKind Kind { get; }

        public static Currency operator +(Currency a, Currency b) => new Currency(a.Value + b.Value);
        public static Currency operator -(Currency a, Currency b) => new Currency(a.Value - b.Value);

        public static Currency operator /(Currency a, Currency b) => new Currency(a.Value / b.Value);

        internal static Currency Min(Currency currency, Currency rhs, InputKind kind = InputKind.Auto)
        {
            if (currency is null)
            {
                throw new ArgumentNullException(nameof(currency));
            }

            if (rhs is null)
            {
                throw new ArgumentNullException(nameof(rhs));
            }

            return new Currency(Math.Min(currency.Value, rhs.Value), kind);
        }

        internal static Currency Min(Currency currency, double rhs)
        {
            if (currency is null)
            {
                throw new ArgumentNullException(nameof(currency));
            }

            return new Currency(Math.Min(currency.Value, rhs));
        }

        internal static Currency Max(Currency currency, Currency rhs)
        {
            if (currency is null)
            {
                throw new ArgumentNullException(nameof(currency));
            }

            if (rhs is null)
            {
                throw new ArgumentNullException(nameof(rhs));
            }

            return new Currency(Math.Max(currency.Value, rhs.Value));
        }

        internal static Currency Max(Currency currency, double rhs)
        {
            if (currency is null)
            {
                throw new ArgumentNullException(nameof(currency));
            }

            return new Currency(Math.Max(currency.Value, rhs));
        }

        public static Currency operator *(Currency a, double b) => new Currency(a.Value * b);

        public static bool operator >(Currency a, double b) => a.Value > b;
        public static bool operator >(Currency a, Currency b) => a.Value > b.Value;
        public static bool operator <(Currency a, double b) => a.Value < b;
        public static bool operator <(Currency a, Currency b) => a.Value < b.Value;
        public static bool operator >=(Currency a, double b) => a.Value >= b;
        public static bool operator >=(Currency a, Currency b) => a.Value >= b.Value;
        public static bool operator <=(Currency a, double b) => a.Value <= b;
        public static bool operator <=(Currency a, Currency b) => a.Value <= b.Value;
    }
}
