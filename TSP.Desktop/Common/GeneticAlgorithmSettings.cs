using GA.Core.Utility;

namespace TSP.Desktop.Common
{
    public class GeneticAlgorithmSettings
    {
        public CrossoverType CrossoverType { get; set; }
        public SelectionType SelectionType { get; set; }
        public MutationType MutationType { get; set; }

        public int PopulationSize { get; set; }

        public GASettings GASettings { get; set; }
    }

    public enum CrossoverType
    {
        SinglePointOrdered,
        ParallelSinglePointOrdered,
        TwoPointOrdered,
        ParallelTwoPointOrdered,
        Cyclic,
        ParallelCyclic,
        PartiallyMapped,
        ParallelPartiallyMapped,
        BitMask,
    }

    public enum SelectionType
    {
        Tournament,
        ParallelTournament,
        RouletteWheel,
        ParallelRouletteWheel,
    }

    public enum MutationType
    {
        Inverse,
        ParallelInverse,
        Shift,
        ParallelShift,
        Swap,
        ParallelSwap,
    }
}