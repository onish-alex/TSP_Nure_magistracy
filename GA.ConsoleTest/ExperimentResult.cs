using GA.Core.Models;
using System;
using System.Collections.Generic;

namespace GA.ConsoleApp
{
    public class ExperimentResult<TNode>
    {
        public bool IsGroupResult { get; set; }
        public Guid GroupGuid { get; set; }

        public IList<Individual<TNode>> StartPopulation { get; set; }
        public IList<Individual<TNode>> FinishPopulation { get; set; }

        public double MinResult { get; set; }
        public double MaxResult { get; set; }
        public double AverageResult { get; set; }
        public double DegenerationCoefficient { get; set; }
        public double LastIterationNumber { get; set; }

        public TimeSpan Time { get; set; }

        public string ResearchedParameterName { get; set; }
        public object ResearchedParameterValue { get; set; }
    }
}
