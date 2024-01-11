using GA.Core.Models;
using System;
using System.Collections.Generic;

namespace GA.ConsoleApp.Experiments
{
    internal class ExperimentResult<TNode>
    {
        internal bool IsGroupResult { get; set; }
        internal Guid GroupGuid { get; set; }

        internal IList<Individual<TNode>> StartPopulation { get; set; }
        internal IList<Individual<TNode>> FinishPopulation { get; set; }

        internal double MinResult { get; set; }
        internal double MaxResult { get; set; }
        internal double AverageResult { get; set; }
        internal double DegenerationCoefficient { get; set; }
        internal double LastIterationNumber { get; set; }

        internal TimeSpan Time { get; set; }

        internal string ResearchedParameterName { get; set; }
        internal object ResearchedParameterValue { get; set; }
    }
}
