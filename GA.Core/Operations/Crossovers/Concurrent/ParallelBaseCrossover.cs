using GA.Core.Models;
using GA.Core.Utility;
using System.Collections.Generic;

namespace GA.Core.Operations.Crossovers.Concurrent
{
    public abstract class ParallelBaseCrossover : ParallelBaseGAOperation, ICrossover
    {
        protected ParallelBaseCrossover(GAOperationSettings operationSettings) : base(operationSettings) { }

		public abstract IList<TIndividual> GetNextGeneration<TIndividual, TGene>(IList<(TIndividual, TIndividual)> parents) where TIndividual : Individual<TGene>;
	}
}
