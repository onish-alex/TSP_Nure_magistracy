namespace Algorithms.Utility.NumberWrapper
{
    public class NumberInt : INumber<int>
    {
        public int Value { get; private set; }

        public NumberInt(int value)
        {
            Value = value;
        }

        public int AddStore(int added)
        {
            Value += added;
            return Value;
        }
    }
}
