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
		private ISelection selection;
		private ICrossover crossover;
		private IMutation mutation;

		private IList<Individual<TGene>> population;
		Func<Individual<TGene>, double> fitnessGetter;

		Dictionary<Individual<TGene>, double> fitnesses;

		private (Individual<TGene> Individual, double Fitness) currentBestResult;
		private GASettings settings;

		private int iteration = 0;
		private int stagnationCounter = 0;
		private bool stopFlag = false;

		private FitnessSortEnum sort;

		public IList<Individual<TGene>> Population => population.ToList();
		public int Iteration => iteration;

		/// <summary>
		/// Build algorithm for individuals which distinguish by order of genes
		/// </summary>
		/// <param name="genotype"></param>
		public GeneticAlgorithm(
			ISelection selection,
			ICrossover crossover,
			IMutation mutation,
			IList<Individual<TGene>> population,
			GASettings settings,
			Func<Individual<TGene>, double> fitnessGetter,
			FitnessSortEnum sort = FitnessSortEnum.Descending)
		{
			this.selection = selection;
			this.crossover = crossover;
			this.mutation = mutation;

			this.population = population;

			fitnesses = new Dictionary<Individual<TGene>, double>();
			this.fitnessGetter = fitnessGetter;

			this.settings = settings;
			this.sort = sort;

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

		public (Individual<TGene> Individual, double Fitness) Run()
		{
			if (settings.GenerationsMaxCount < 1)
				throw new ArgumentOutOfRangeException(
					nameof(settings.GenerationsMaxCount),
					settings.GenerationsMaxCount,
					$"{nameof(settings.GenerationsMaxCount)} must be greater than 0");

			for (iteration = 0; iteration < settings.GenerationsMaxCount; iteration++)
			{
				//Console.WriteLine(iteration);
				GetNextGeneration();

				if (stopFlag)
					break;
			}

			return currentBestResult;
		}

		public IList<Individual<TGene>> GetNextGeneration()
		{
			if (settings.ElitePercent.HasValue)
			{
				if (settings.ElitePercent < 0D && settings.ElitePercent > 100D)
					throw new ArgumentOutOfRangeException(
						nameof(settings.ElitePercent),
						settings.ElitePercent.Value,
						"elite percent must be between 0 and 100");

				var populationEliteCount = (int)Math.Ceiling(population.Count * (settings.ElitePercent.Value / 100));
				fitnesses =
					(sort == FitnessSortEnum.Ascending ? fitnesses.OrderByDescending(x => x.Value) : fitnesses.OrderBy(x => x.Value))
					.Take(populationEliteCount)
					.ToDictionary(x => x.Key, x => x.Value);
			}

			var parentPairs = selection.GetParentPairs(fitnesses, sort);
			var children = crossover.GetNextGeneration(parentPairs);
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

				fitnesses =
					(sort == FitnessSortEnum.Ascending ? fitnesses.OrderByDescending(x => x.Value) : fitnesses.OrderBy(x => x.Value))
					.Take(population.Count)
					.ToDictionary(x => x.Key, x => x.Value);

				population = fitnesses.Select(x => x.Key).ToList();
			}

			var currentIterationBestResult = (sort == FitnessSortEnum.Ascending ? fitnesses.OrderByDescending(x => x.Value) : fitnesses.OrderBy(x => x.Value)).FirstOrDefault();

			var resetStagnation = false;

			if ((sort == FitnessSortEnum.Ascending && currentIterationBestResult.Value > currentBestResult.Fitness || currentBestResult.Fitness == 0D)
			 || (sort == FitnessSortEnum.Descending && currentIterationBestResult.Value < currentBestResult.Fitness || currentBestResult.Fitness == 0D))
			{
				currentBestResult = (currentIterationBestResult.Key, currentIterationBestResult.Value);
				resetStagnation = true;
			}

			if (settings.StagnatingGenerationsLimit.HasValue && settings.StagnatingGenerationsLimit.Value > 0)
			{
				if (resetStagnation)
				{
					stagnationCounter = 0;
				}
				else
				{
					stagnationCounter++;
				}

				if (stagnationCounter >= settings.StagnatingGenerationsLimit)
					stopFlag = true;
			}

			if (settings.DegenerationMaxPercent.HasValue && settings.DegenerationMaxPercent.Value > 0D)
			{
				var degenerationCoef = population.GetDegenerationCoefficient() * 100D;
				//var degenerationCoef2 = population.GetDegenerationCoefficient2() * 100D;

				if (degenerationCoef > settings.DegenerationMaxPercent.Value)
					stopFlag = true;
			}
			return population;
		}
	}
}
