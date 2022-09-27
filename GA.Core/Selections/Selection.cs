using System;
using System.Collections.Generic;

namespace GA.Core.Selections
{
    public abstract class Selection : GAOperation
    {
        public abstract IList<(TIndividual, TIndividual)> GetParentPairs<TIndividual>
           (IList<TIndividual> population, Func<TIndividual, double> fitnessGetter);

        public abstract IList<(TIndividual, TIndividual)> GetParentPairs<TIndividual>
           (IList<TIndividual> population, Func<TIndividual, double> fitnessGetter, double eliteCoefficient);
    }
}
