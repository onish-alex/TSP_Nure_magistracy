using Algorithms.Utility;
using GA.Core.Operations.Selections;
using GA.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GA.Operations.Selections
{
	public class RouletteWheelSelection : BaseSelection
	{
		public RouletteWheelSelection(GAOperationSettings operationSettings) : base(operationSettings) { }

		/// <summary>
		/// Returns parent pairs by wheel selection
		/// </summary>
		/// <typeparam name="TIndividual">Type that represent individual with set of genes</typeparam>
		/// <param name="populationFitnesses">Individuals and their fitness values</param>
		/// <returns>List of tuples - parent pairs for crossover</returns>
		public override IList<(TIndividual, TIndividual)> GetParentPairs<TIndividual>
			(IDictionary<TIndividual, double> populationFitnesses, FitnessSortEnum sort = FitnessSortEnum.Descending)
		{
			///Example:
			///4 individuals with fitnesses: 10, 20, 30, 40
			///fitnessSum: 10+20+30+40 = 100
			///wheel turning: get random value from 0 to fitnessSum (100)
			///if random value:
			///from 0 to 10 - choose 1st individual
			///from 10 to (10+20) - 2nd
			///from (10+20) to (10+20+30) - 3rd
			///from (10+20+30) to (10+20+30+40) - 4th
			///
			/// individuals:            1st    2nd        3rd           4th
			///                       |....|........|............|................|
			/// right border values:  0    10       30           60               100

			//probability right border values for individuals
			var probabilities = new Dictionary<double, TIndividual>();

			var fitnessSum = (sort == FitnessSortEnum.Ascending) ? populationFitnesses.Values.Sum() : populationFitnesses.Values.Select(x => 1 / x).Sum();

			var rightValue = 0D;

			foreach (var fitness in populationFitnesses)
			{
				rightValue += (sort == FitnessSortEnum.Descending) ? 1 / fitness.Value : fitness.Value;
				probabilities.Add(rightValue, fitness.Key);
			}

			var pairsCount = populationFitnesses.Count / 2;
			var pairs = new List<(TIndividual, TIndividual)>(pairsCount);

			for (var i = 0; i < pairsCount; i++)
			{
				var probabilityValues = probabilities.Keys.ToList();

				//choose first parent
				var firstWheelTurning = Random.Shared.NextDouble() * fitnessSum;

				probabilityValues.Add(firstWheelTurning);
				probabilityValues.Sort();

				var firstParentIndex = SearchHelper.BinarySearch(probabilityValues, firstWheelTurning);
				var firstParentProbabilityValue = probabilityValues[firstParentIndex + 1];
				var firstParent = probabilities[firstParentProbabilityValue];

				//remove first parent index so we can't choose it also as second parent
				probabilityValues.RemoveAt(firstParentIndex + 1);
				probabilityValues.RemoveAt(firstParentIndex);

				//exclude fitness of chosen first parent
				var correctedProbabilityValues = probabilityValues
												.Select(x => x >= firstParentProbabilityValue ? x - ((sort == FitnessSortEnum.Ascending) ? populationFitnesses[firstParent] : 1 / populationFitnesses[firstParent]) : x)
												.ToList();

				fitnessSum -= (sort == FitnessSortEnum.Ascending) ? populationFitnesses[firstParent] : 1 / populationFitnesses[firstParent];

				//choose second parent
				var secondWheelTurning = Random.Shared.NextDouble() * fitnessSum;

				correctedProbabilityValues.Add(secondWheelTurning);
				correctedProbabilityValues.Sort();

				//here's we took second parent from 'probabilityValues', not 'correctedProbabilityValues', that's why we don't need +1 index shift
				var secondParentIndex = SearchHelper.BinarySearch(correctedProbabilityValues, secondWheelTurning);
				var secondParent = probabilities[probabilityValues[secondParentIndex]];

				pairs.Add((firstParent, secondParent));
				fitnessSum += (sort == FitnessSortEnum.Ascending) ? populationFitnesses[firstParent] : 1 / populationFitnesses[firstParent];
			}

			return pairs;
		}

		protected override void InitSettings() { }
	}
}
