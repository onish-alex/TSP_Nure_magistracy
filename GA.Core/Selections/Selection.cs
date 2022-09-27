using System.Collections.Generic;

namespace GA.Core.Selections
{
    public abstract class Selection : GAOperation
    {
        public abstract IList<(TIndividual, TIndividual)> GetParentPairs<TIndividual>
           (IDictionary<TIndividual, double> populationFitnesses);

        public abstract IList<(TIndividual, TIndividual)> GetParentPairs<TIndividual>
           (IDictionary<TIndividual, double> populationFitnesses, double eliteCoefficient);
    }
}
