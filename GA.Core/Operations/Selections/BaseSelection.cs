using System.Collections.Generic;

namespace GA.Core.Operations.Selections
{
	public abstract class BaseSelection : BaseGAOperation
	{
		public abstract IList<(TIndividual, TIndividual)> GetParentPairs<TIndividual>
		   (IDictionary<TIndividual, double> populationFitnesses);
	}
}
