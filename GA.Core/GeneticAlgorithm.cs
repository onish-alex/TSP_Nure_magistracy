using GA.Core.Models;
using GA.Core.Operations.Crossovers;
using GA.Core.Operations.Mutations;
using GA.Core.Operations.Selections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GA.Core
{
	public class GeneticAlgorithm<TGene>
	{
		private readonly static Random rand = new Random();

		private BaseSelection selection;
		private BaseCrossover crossover;
		private BaseMutation mutation;

		/// <summary>
		/// Build algorithm for individuals which distinguish by order of genes
		/// </summary>
		/// <param name="genotype"></param>
		public GeneticAlgorithm(
			BaseSelection selection,
			BaseCrossover crossover,
			BaseMutation mutation)
		{
			this.selection = selection;
			selection.Random = rand;

			this.crossover = crossover;
			crossover.Random = rand;

			this.mutation = mutation;
			mutation.Random = rand;
		}

		public IList<TIndividual> GetNextGeneration<TIndividual>(IList<TIndividual> population, Func<TIndividual, double> fitnessGetter, double mutationProbability, double? elitePercent = null) where TIndividual : Individual<TGene>
		{
			var fitnesses = new Dictionary<TIndividual, double>();

			foreach (var individual in population)
				fitnesses.Add(individual, fitnessGetter(individual));

			if (elitePercent.HasValue)
			{
				if (elitePercent < 0D && elitePercent > 100D)
					throw new ArgumentOutOfRangeException(
						nameof(elitePercent),
						elitePercent.Value,
						"elite percent must be between 0 and 100");

				var populationEliteCount = (int)Math.Ceiling(population.Count * (elitePercent.Value / 100));
				fitnesses = fitnesses
					.OrderByDescending(x => x.Value)
					.Take(populationEliteCount)
					.ToDictionary(x => x.Key, x => x.Value);
			}

			var parentPairs = selection.GetParentPairs(fitnesses);
			var children = crossover.GetNextGeneration<TIndividual, TGene>(parentPairs);
			mutation.ProcessMutation<TIndividual, TGene>(children, mutationProbability);

			return children;
		}

		public IList<TIndividual> GetNextGenerationWithParents<TIndividual>(IList<TIndividual> population, Func<TIndividual, double> fitnessGetter, double mutationProbability, double? elitePercent = null) where TIndividual : Individual<TGene>
		{
			var fitnesses = new Dictionary<TIndividual, double>();

			foreach (var individual in population)
				fitnesses.Add(individual, fitnessGetter(individual));

			if (elitePercent.HasValue)
			{
				if (elitePercent < 0D && elitePercent > 100D)
					throw new ArgumentOutOfRangeException(
						nameof(elitePercent), 
						elitePercent.Value, 
						"elite percent must be between 0 and 100");

				var populationEliteCount = (int)Math.Ceiling(population.Count * (elitePercent.Value / 100));
				fitnesses = fitnesses
					.OrderByDescending(x => x.Value)
					.Take(populationEliteCount)
					.ToDictionary(x => x.Key, x => x.Value);
			}

			var parentPairs = selection.GetParentPairs(fitnesses);
			var children = crossover.GetNextGeneration<TIndividual, TGene>(parentPairs);
			mutation.ProcessMutation<TIndividual, TGene>(children, mutationProbability);

			var childrenFitnesses = children.ToDictionary(x => x, x => fitnessGetter(x));

			var nextGeneration = fitnesses.Concat(childrenFitnesses)
										  .OrderByDescending(x => x.Value)
										  .Take(population.Count)
										  .Select(x => x.Key)
										  .ToList();

			return nextGeneration;
		}
	}
}
