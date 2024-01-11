using Algorithms.Utility.NumberWrapper;
using GA.Core.Utility;
using System;

namespace GA.ConsoleApp.Experiments
{
    internal class GAExperimentSettings<T> where T : struct, IComparable<T>
    {
        internal Func<GASettings, string> ResearchedParameterName { get; set; }

        internal (INumber<T> Min, INumber<T> Max) ResearchedParameterRange { get; set; }

        internal T ResearchedParameterIncrement { get; set; }

        internal int PopulationSize { get; set; }

        //use the same population for each iteration after changing researched parameter
        internal bool UseSameInitialPopulation { get; set; }

        internal GAOperationSettings SelectionSettings { get; set; }
        internal GAOperationSettings CrossoverSettings { get; set; }
        internal GAOperationSettings MutationSettings { get; set; }

        internal CrossoversEnum CrossoverType { get; set; }
        internal MutationsEnum MutationsType { get; set; }
        internal SelectionsEnum SelectionType { get; set; }

        //public ISelection Selection { get; set; }
        //public ICrossover Crossover { get; set; }
        //public IMutation Mutation { get; set; }

        internal int ControlRepeatingCount { get; set; } = 1;
    }
}
