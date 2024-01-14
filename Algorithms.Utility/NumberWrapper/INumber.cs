using System;


namespace Algorithms.Utility.NumberWrapper
{
	public interface INumber<T> where T : struct, IComparable<T>
	{
		T Value { get; }

		T AddStore(T added);
	}
}
