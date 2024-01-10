using Algorithms.Utility.NumberWrapper;
using GA.Core.Operations.Crossovers;
using GA.Core.Operations.Mutations;
using GA.Core.Operations.Selections;
using GA.Core.Utility;
using GA.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.ConsoleApp
{
    public class GAExperimentSettings<T> where T : struct, IComparable<T>
    {
        public Func<GASettings, string> ResearchedParameterName { get; set; }
        
        public (INumber<T> Min, INumber<T> Max) ResearchedParameterRange { get; set; }

        public INumber<T> ResearchedParameterIncrement { get; set; }

        public int PopulationSize { get; set; }
        
        //use the same population for each iteration after changing researched parameter
        public bool UseSameInitialPopulation { get; set; }

        public GAOperationSettings SelectionSettings { get; set; }
        public GAOperationSettings CrossoverSettings { get; set; }
        public GAOperationSettings MutationSettings { get; set; }

        public CrossoversEnum CrossoverType { get; set; }
        public MutationsEnum MutationsType { get; set; }
        public SelectionsEnum SelectionType { get; set; }

        //public ISelection Selection { get; set; }
        //public ICrossover Crossover { get; set; }
        //public IMutation Mutation { get; set; }
    }
}
