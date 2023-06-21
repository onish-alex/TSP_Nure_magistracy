using GA.Core.Models;
using GA.Core.Utility;
using System.Collections.Generic;

namespace GA.Core.Operations.Mutations.Concurrent
{
	public abstract class ParallelBaseMutation : ParallelBaseGAOperation, IMutation
	{
		protected ParallelBaseMutation(GAOperationSettings operationSettings) : base(operationSettings)
		{
		}

		public abstract void ProcessMutation<TIndividual, TGene>(IList<TIndividual> population, double probability) where TIndividual : Individual<TGene>;
	}
}
