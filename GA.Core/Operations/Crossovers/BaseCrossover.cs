using GA.Core.Models;
using System.Collections.Generic;

namespace GA.Core.Operations.Crossovers
{
	public abstract class BaseCrossover : BaseGAOperation
	{
		public abstract IList<TIndividual> GetNextGeneration<TIndividual, TGene>(IList<(TIndividual, TIndividual)> parents) where TIndividual : Individual<TGene>;
	}
}
