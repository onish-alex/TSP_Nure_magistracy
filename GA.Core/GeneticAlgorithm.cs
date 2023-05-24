using GA.Core.Models;
using GA.Core.Operations.Crossovers;
using GA.Core.Operations.Mutations;
using GA.Core.Operations.Selections;
using GA.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GA.Core
{
	public class GeneticAlgorithm<TGene>
	{
		private static readonly Random rand = new Random();

		private BaseSelection selection;
		private BaseCrossover crossover;
		private BaseMutation mutation;
		private IList<Individual<TGene>> population;
		Func<Individual<TGene>, double> fitnessGetter;
		Dictionary<Individual<TGene>, double> fitnesses;

		/// <summary>
		/// Build algorithm for individuals which distinguish by order of genes
		/// </summary>
		/// <param name="genotype"></param>
		public GeneticAlgorithm(
			BaseSelection selection,
			BaseCrossover crossover,
			BaseMutation mutation,
			IList<Individual<TGene>> population,
			Func<Individual<TGene>, double> fitnessGetter)
		{
			this.selection = selection;
			selection.Random = rand;

			this.crossover = crossover;
			crossover.Random = rand;

			this.mutation = mutation;
			mutation.Random = rand;

			this.population = population;
			this.fitnessGetter = fitnessGetter;

			fitnesses = new Dictionary<Individual<TGene>, double>();

			try
			{
				foreach (var individual in population)
					fitnesses.Add(individual, fitnessGetter(individual));
			}
			catch (Exception ex)
			{
				//LOG error somewhere in fitness getter
			}
		}

		public IList<Individual<TGene>> GetNextGeneration(GASettings settings = null)
		{
			if (settings == null)
				settings = new GASettings();

			if (settings.ElitePercent.HasValue)
			{
				if (settings.ElitePercent < 0D && settings.ElitePercent > 100D)
					throw new ArgumentOutOfRangeException(
						nameof(settings.ElitePercent),
						settings.ElitePercent.Value,
						"elite percent must be between 0 and 100");

				var populationEliteCount = (int)Math.Ceiling(population.Count * (settings.ElitePercent.Value / 100));
				fitnesses = fitnesses
					.OrderByDescending(x => x.Value)
					.Take(populationEliteCount)
					.ToDictionary(x => x.Key, x => x.Value);
			}

			var parentPairs = selection.GetParentPairs(fitnesses);
			var children = crossover.GetNextGeneration<Individual<TGene>, TGene>(parentPairs);
			mutation.ProcessMutation<Individual<TGene>, TGene>(children, settings.MutationProbability);

			if (settings.OnlyChildrenInNewGeneration)
			{
				population = children;
				fitnesses = children.ToDictionary(x => x, x => fitnessGetter(x));
			}
			else
			{
				foreach (var child in children)
					fitnesses.Add(child, fitnessGetter(child));

				fitnesses = fitnesses
					.OrderByDescending(x => x.Value)
					.Take(population.Count)
					.ToDictionary(x => x.Key, x => x.Value);

				population = fitnesses.Select(x => x.Key).ToList();
			}

			return population;
		}
	}
}
