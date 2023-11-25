using System.Collections.Generic;

namespace GA.Core.Operations.Selections
{
	public interface ISelection
	{
		IList<(TIndividual, TIndividual)> GetParentPairs<TIndividual>
		   (IDictionary<TIndividual, double> populationFitnesses);
	}
}
