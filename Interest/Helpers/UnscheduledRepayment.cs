namespace Interest
{

    public enum InputType
    {
        Auto,
        Manual
    }

    public struct UnscheduledRepayment
    {
        public UnscheduledRepayment(double value, InputType valueType)
        {
            Value = value;
            InputType = valueType;
        }

        public double Value;
        public InputType InputType;

        public override string ToString()
        {
            return string.Concat(Value, ", ", InputType.ToString());
        }
    }
}