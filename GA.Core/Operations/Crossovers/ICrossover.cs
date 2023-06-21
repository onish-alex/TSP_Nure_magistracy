using GA.Core.Models;
using System.Collections.Generic;

namespace GA.Core.Operations.Crossovers
{
	public interface ICrossover
	{
		IList<TIndividual> GetNextGeneration<TIndividual, TGene>(IList<(TIndividual, TIndividual)> parents) where TIndividual : Individual<TGene>;
	}
}
