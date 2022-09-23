using System.Collections;
using System.Collections.Generic;

namespace GA.Core.Crossovers
{
    public abstract class Crossover : GAOperation
    {
        public abstract IList<IList<TGene>> GetNextGeneration<TGene>(IList<(IList<TGene>, IList<TGene>)> parents);
    }
}
