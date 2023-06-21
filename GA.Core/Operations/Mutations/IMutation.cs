using GA.Core.Models;
using System.Collections.Generic;

namespace GA.Core.Operations.Mutations
{
	public interface IMutation
	{
		void ProcessMutation<TIndividual, TGene>(IList<TIndividual> population, double probability) where TIndividual : Individual<TGene>;
	}
}
