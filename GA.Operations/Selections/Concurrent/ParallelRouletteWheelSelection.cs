using Algorithms.Utility;
using GA.Core.Operations.Selections;
using GA.Core.Operations.Selections.Concurrent;
using GA.Core.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Operations.Selections.Concurrent
{
    public class ParallelRouletteWheelSelection : ParallelBaseSelection
    {
        public ParallelRouletteWheelSelection(GAOperationSettings operationSettings) : base(operationSettings) { }

        /// <summary>
        /// Returns parent pairs by wheel selection
        /// </summary>
        /// <typeparam name="TIndividual">Type that represent individual with set of genes</typeparam>
        /// <param name="populationFitnesses">Individuals and their fitness values</param>
        /// <returns>List of tuples - parent pairs for crossover</returns>
        public override IList<(TIndividual, TIndividual)> GetParentPairs<TIndividual>
            (IDictionary<TIndividual, double> populationFitnesses)
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

            var probabilities = new Dictionary<double, TIndividual>();      //probability right border values for individuals

            //var fitnessSum = populationFitnesses.Values.AsParallel().Sum();
            //var fitnessSum = populationFitnesses.Values.Sum();
            var rightValue = 0D;

            foreach (var fitness in populationFitnesses)
            {
                rightValue += Math.Round(fitness.Value, 4);
                probabilities.TryAdd(rightValue, fitness.Key);
            }

            var pairsCount = populationFitnesses.Count / 2;
            var pairs = new List<(TIndividual, TIndividual)>(pairsCount);

            //for (var i = 0; i < pairsCount; i++)
            Parallel.For(0, pairsCount, parallelOptions, (i) =>
            {
                List<double> probabilityValues = null;

                lock (this)
                {
                    probabilityValues = probabilities.Keys.ToList();
                }

                //choose first parent
                var firstWheelTurning = Math.Round(Random.Shared.NextDouble() * rightValue, 5);

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
                                                .Select(x => x - firstParentProbabilityValue)
                                                .ToList();

                rightValue = Math.Round(rightValue - populationFitnesses[firstParent]);

                //choose second parent
                var secondWheelTurning = Math.Round(Random.Shared.NextDouble() * rightValue - firstParentProbabilityValue, 4);

                correctedProbabilityValues.Add(secondWheelTurning);
                correctedProbabilityValues.Sort();

                var secondParentIndex = SearchHelper.BinarySearch(correctedProbabilityValues, secondWheelTurning);
                var secondParent = probabilities[probabilityValues[secondParentIndex]];

                pairs.Add((firstParent, secondParent));
                rightValue = Math.Round(rightValue + populationFitnesses[firstParent]);
            });

            return pairs;
        }

        protected override void InitSettings() { }
    }
}
