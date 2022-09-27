using System.Collections.Generic;

namespace GA.Core.Mutations
{
    public abstract class Mutation : GAOperation
    {
        public abstract void ProcessMutation<TGene>(IList<IList<TGene>> population, double probability);
    }
}
