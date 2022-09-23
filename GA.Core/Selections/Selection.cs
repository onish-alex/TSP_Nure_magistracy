using System;
using System.Collections.Generic;

namespace GA.Core.Selections
{
    public abstract class Selection
    {
        protected Random rand;
        
        protected Selection(Random rand)
        {
            this.rand = rand;
        }

        public abstract IEnumerable<(TIndividual, TIndividual)> GetParentPairs<TIndividual>
           (IList<TIndividual> population, Func<TIndividual, double> fitnessGetter);
    }
}
