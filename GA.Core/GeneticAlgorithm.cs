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

        private Selection selection;
        private Crossover crossover;
        private Mutation mutation;
        
        /// <summary>
        /// Build algorithm for individuals which distinguish by order of genes
        /// </summary>
        /// <param name="genotype"></param>
        public GeneticAlgorithm(
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

        public IList<IList<TGene>> GetNextGeneration(IList<IList<TGene>> population, Func<IList<TGene>, double> fitnessGetter, double mutationProbability)
        {
            var fitnesses = new Dictionary<IList<TGene>, double>();

            foreach (var individual in population)
                fitnesses.Add(individual, fitnessGetter(individual));

            var parentPairs = selection.GetParentPairs(fitnesses);

            var children = crossover.GetNextGeneration(parentPairs);
            mutation.ProcessMutation(children, mutationProbability);
            
            return children;
        }

        public IList<IList<TGene>> GetNextGeneration(IList<IList<TGene>> population, Func<IList<TGene>, double> fitnessGetter, double mutationProbability, double eliteCoefficient)
        {
            var fitnesses = new Dictionary<IList<TGene>, double>();

            foreach (var individual in population)
                fitnesses.Add(individual, fitnessGetter(individual));

            var parentPairs = selection.GetParentPairs(fitnesses, eliteCoefficient);

            var children = crossover.GetNextGeneration(parentPairs);
            mutation.ProcessMutation(children, mutationProbability);

            return children;
        }

        public IList<IList<TGene>> GetNextGenerationWithParents(IList<IList<TGene>> population, Func<IList<TGene>, double> fitnessGetter, double mutationProbability)
        {
            var populationCount = population.Count;

            var fitnesses = new Dictionary<IList<TGene>, double>();

            foreach (var individual in population)
                fitnesses.Add(individual, fitnessGetter(individual));

            var parentPairs = selection.GetParentPairs(fitnesses);
            var children = crossover.GetNextGeneration(parentPairs);
            mutation.ProcessMutation(children, mutationProbability);

            var childrenFitnesses = children.ToDictionary(x => x, x => fitnessGetter(x));

            var nextGeneration = fitnesses.Concat(childrenFitnesses)
                                          .OrderByDescending(x => x.Value)
                                          .Take(populationCount)
                                          .Select(x => x.Key)
                                          .ToList();

            return nextGeneration;
        }

        public IList<IList<TGene>> GetNextGenerationWithParents(
            IList<IList<TGene>> population, 
            Func<IList<TGene>, double> fitnessGetter, 
            double mutationProbability,
            double eliteCoefficient)
        {
            var populationCount = population.Count;

            var fitnesses = new Dictionary<IList<TGene>, double>();

            foreach (var individual in population)
                fitnesses.Add(individual, fitnessGetter(individual));

            var parentPairs = selection.GetParentPairs(fitnesses, eliteCoefficient);
            var children = crossover.GetNextGeneration(parentPairs);
            mutation.ProcessMutation(children, mutationProbability);

            var childrenFitnesses = children.ToDictionary(x => x, x => fitnessGetter(x));

            var nextGeneration = fitnesses.Concat(childrenFitnesses)
                                          .OrderByDescending(x => x.Value)
                                          .Take(populationCount)
                                          .Select(x => x.Key)
                                          .ToList();

            return nextGeneration;
        }
    }
}
