namespace GA.Core.Utility
{

	public enum CrossoversEnum : int
	{
		Cyclic =						1,
		PartiallyMapped =				2,
		SinglePointOrdered =			3,
		TwoPointOrdered =				4,
		OrderBased =					5,
		InverOver =						6,

		//BitMask,

		ParallelCyclic =				7,
		ParallelPartiallyMapped =		8,
		ParallelSinglePointOrdered =	9,
		ParallelTwoPointOrdered =		10,
		ParallelOrderBased =			11,
		ParallelInverOver =				12,

		//ParallelBitMask,
	}

	public enum MutationsEnum : int
	{
		Inverse,
		Shift,
		Swap,

		ParallelInverse,
		ParallelShift,
		ParallelSwap,
	}

	public enum SelectionsEnum : int
	{
		RouletteWheel,
		Tournament,

		ParallelRouletteWheel,
		ParallelTournament,
	}
}
