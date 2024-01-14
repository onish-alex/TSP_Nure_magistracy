using System;

namespace Algorithms.Utility.NumberWrapper
{
	public abstract class NumberBase<T> : INumber<T> where T : struct, IComparable<T>
	{
		public T Value { get; protected set; }

		public abstract T AddStore(T added);

		protected NumberBase(T value)
		{
			Value = value;
		}
	}
}
