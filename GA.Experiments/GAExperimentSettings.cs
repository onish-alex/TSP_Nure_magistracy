using Algorithms.Utility.NumberWrapper;
using GA.Core.Utility;
using GA.Operations;
using System;

namespace GA.Experiments
{
    public class GAExperimentSettings<T> where T : struct, IComparable<T>
    {
        public string ResearchedParameterName { get; set; }

        public (INumber<T> Min, INumber<T> Max) ResearchedParameterRange { get; set; }

        public T ResearchedParameterIncrement { get; set; }

        public int PopulationSize { get; set; }

        //use the same population for each iteration after changing researched parameter
        public bool UseSameInitialPopulation { get; set; }

        public GAOperationSettings SelectionSettings { get; set; }
        public GAOperationSettings CrossoverSettings { get; set; }
        public GAOperationSettings MutationSettings { get; set; }

        public CrossoversEnum CrossoverType { get; set; }
        public MutationsEnum MutationsType { get; set; }
        public SelectionsEnum SelectionType { get; set; }

        public int ControlRepeatingCount { get; set; } = 1;
    }
}
