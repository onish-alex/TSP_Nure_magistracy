namespace Algorithms.Utility.NumberWrapper
{
	public class NumberInt : NumberBase<int>
	{
		public NumberInt(int value) : base(value) { }

		public override int AddStore(int added)
		{
			Value += added;
			return Value;
		}
	}
}
