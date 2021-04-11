namespace Interest.Types
{
    public record Percentage
    {
        public Percentage(Currency currency) : this(currency.Value)
        {
        }

        public Percentage(double perYear, InputKind inputKind = InputKind.Auto)
        {
            PerYear = perYear;
            PerMonth = perYear / 12.0;
            InputKind = inputKind;
        }

        public double PerYear { get; }

        public double PerMonth { get; }

        public double PerYearAsFraction => PerYear / 100.0;
        public double PerMonthAsFraction => PerMonth / 100.0;

        public InputKind InputKind;

        public override string ToString()
        {
            return string.Concat(PerYear, ", ", InputKind.ToString());
        }

        public static Percentage operator -(Percentage a, Percentage b)
        {
            return new Percentage(a.PerYear - b.PerYear);
        }
    }
}
