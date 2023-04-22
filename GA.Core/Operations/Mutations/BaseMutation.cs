using GA.Core.Models;
using System.Collections.Generic;

namespace GA.Core.Operations.Mutations
{
	public abstract class BaseMutation : BaseGAOperation
	{
		public abstract void ProcessMutation<TIndividual, TGene>(IList<TIndividual> population, double probability) where TIndividual : Individual<TGene>;
	}
}
