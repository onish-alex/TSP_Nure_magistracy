using GA.Core.Models;
using GA.Core.Utility;
using System.Collections.Generic;

namespace GA.Core.Operations.Mutations
{
	public abstract class BaseMutation : ParallelBaseGAOperation, IMutation
	{
		protected BaseMutation(GAOperationSettings operationSettings) : base(operationSettings) { }

		public abstract void ProcessMutation<TIndividual, TGene>(IList<TIndividual> population, double probability) where TIndividual : Individual<TGene>;
	}
}
