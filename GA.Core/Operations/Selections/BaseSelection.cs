using GA.Core.Utility;
using System.Collections.Generic;

namespace GA.Core.Operations.Selections
{
	public abstract class BaseSelection : BaseGAOperation
	{
		protected BaseSelection(GAOperationSettings operationSettings) : base(operationSettings) { }

		public abstract IList<(TIndividual, TIndividual)> GetParentPairs<TIndividual>
		   (IDictionary<TIndividual, double> populationFitnesses);
	}
}
