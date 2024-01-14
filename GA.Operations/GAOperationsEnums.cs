namespace GA.Operations
{

	public enum CrossoversEnum : int
	{
		Cyclic,
		PartiallyMapped,
		SinglePointOrdered,
		TwoPointOrdered,

		BitMask,

		ParallelCyclic,
		ParallelPartiallyMapped,
		ParallelSinglePointOrdered,
		ParallelTwoPointOrdered,

		ParallelBitMask,
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
