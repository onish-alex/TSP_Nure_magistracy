using GA.Core.Crossovers;
using GA.Core.Mutations;
using GA.Core.Selections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Core
{
    public class GeneticAlgorithm<TGene>
    {
        private readonly static Random rand = new Random();

        private IList<TGene> genotype;
        private Selection selection;
        private Crossover crossover;
        private Mutation mutation;
        
        /// <summary>
        /// Build algorithm for individuals which distinguish by order of genes
        /// </summary>
        /// <param name="genotype"></param>
        public GeneticAlgorithm(
            IList<TGene> genotype,
            Selection selection,
            Crossover crossover,
            Mutation mutation)
        {
            this.selection = selection;
            selection.Random = rand;

            this.crossover = crossover;
            crossover.Random = rand;

            this.mutation = mutation;
            mutation.Random = rand;
        }

        public IList<IList<TGene>> ProcessGeneration(IList<IList<TGene>> population, Func<IList<TGene>, double> fitnessGetter)
        {
            var parentPairs = selection.GetParentPairs(population, fitnessGetter);
            return crossover.GetNextGeneration(parentPairs);
        }
    }
}
