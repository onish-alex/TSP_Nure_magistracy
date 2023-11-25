using GA.Core.Utility;
using System.Collections.Generic;

namespace GA.Core.Operations.Selections.Concurrent
{
	public abstract class ParallelBaseSelection : ParallelBaseGAOperation, ISelection
	{
		protected ParallelBaseSelection(GAOperationSettings operationSettings) : base(operationSettings)
		{
		}

		public abstract IList<(TIndividual, TIndividual)> GetParentPairs<TIndividual>(IDictionary<TIndividual, double> populationFitnesses);
	}
}
