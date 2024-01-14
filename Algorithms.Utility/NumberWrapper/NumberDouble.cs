namespace Algorithms.Utility.NumberWrapper
{
	public class NumberDouble : NumberBase<double>
	{
		public NumberDouble(double value) : base(value) { }

		public override double AddStore(double added)
		{
			Value += added;
			return Value;
		}
	}
}
