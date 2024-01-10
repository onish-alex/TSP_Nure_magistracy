namespace Algorithms.Utility.NumberWrapper
{
    public class NumberDouble : INumber<double>
    {
        public double Value { get; private set; }

        public NumberDouble(double value)
        {
            Value = value;
        }

        public double AddStore(double added)
        {
            Value += added;
            return Value;
        }
    }
}
