using GA.Core.Models;
using System.Collections.Generic;

namespace GA.Core.Operations.Crossovers
{
	public interface ICrossover
	{
		IList<Individual<TGene>> GetNextGeneration<TGene>(IList<(Individual<TGene>, Individual<TGene>)> parents);
	}
}
