namespace Interest
{
    public struct InputValue<T>
    {
        public InputValue(T value, InputType valueType)
        {
            Value = value;
            InputType = valueType;
        }

        public T Value;
        public InputType InputType;

        public override string ToString()
        {
            return string.Concat(Value, ", ", InputType.ToString());
        }
    }
}