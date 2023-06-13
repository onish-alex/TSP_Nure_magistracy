using GA.Core.Models;
using GA.Core.Utility;
using System.Collections.Generic;

namespace GA.Core.Operations.Crossovers
{
	public abstract class BaseCrossover : BaseGAOperation
	{
		protected BaseCrossover(GAOperationSettings operationSettings) : base(operationSettings) { }

		public abstract IList<TIndividual> GetNextGeneration<TIndividual, TGene>(IList<(TIndividual, TIndividual)> parents) where TIndividual : Individual<TGene>;
	}
}
