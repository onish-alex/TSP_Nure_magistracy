using GA.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GA.Core.Selections
{
    public class RouletteWheelSelection : Selection
    {
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

            var fitnessSum = populationFitnesses.Values.Sum();

            var rightValue = 0D;

            foreach (var fitness in populationFitnesses)
            {
                rightValue += fitness.Value;
                probabilities.Add(rightValue, fitness.Key);
            }

            var pairsCount = populationFitnesses.Count / 2;
            var pairs = new List<(TIndividual, TIndividual)>(pairsCount);
            
            for (var i = 0; i < pairsCount; i++)
            {
                var probabilityValues = probabilities.Keys.ToList();
                
                //choose first parent
                var firstWheelTurning = Random.NextDouble() * fitnessSum;

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
                
                fitnessSum -= populationFitnesses[firstParent];

                //choose second parent
                var secondWheelTurning = Random.NextDouble() * fitnessSum - firstParentProbabilityValue;

                correctedProbabilityValues.Add(secondWheelTurning);
                correctedProbabilityValues.Sort();

                var secondParentIndex = SearchHelper.BinarySearch(correctedProbabilityValues, secondWheelTurning);
                var secondParent = probabilities[probabilityValues[secondParentIndex]];

                pairs.Add((firstParent, secondParent));
                fitnessSum += populationFitnesses[firstParent];
            }

            return pairs;
        }

        public override IList<(TIndividual, TIndividual)> GetParentPairs<TIndividual>(IDictionary<TIndividual, double> populationFitnesses, double eliteCoefficient)
        {
            if (eliteCoefficient <= 0D || eliteCoefficient > 100D)
                throw new ArgumentException("eliteCoefficient can contain value in range (0, 100]");
            
            var probabilities = new Dictionary<double, TIndividual>();      //probability right border values for individuals

            var fitnesses = populationFitnesses
                                 .OrderByDescending(x => x.Value)
                                 .Take((int)Math.Ceiling(populationFitnesses.Count * eliteCoefficient / 100))
                                 .ToDictionary(x => x.Key, x => x.Value);

            var fitnessSum = fitnesses.Values.Sum();

            var rightValue = 0D;

            foreach (var fitness in fitnesses)
            {
                rightValue += fitness.Value;
                probabilities.Add(rightValue, fitness.Key);
            }

            var pairsCount = fitnesses.Count / 2;
            var pairs = new List<(TIndividual, TIndividual)>(pairsCount);

            for (var i = 0; i < pairsCount; i++)
            {
                var probabilityValues = probabilities.Keys.ToList();

                //choose first parent
                var firstWheelTurning = Random.NextDouble() * fitnessSum;

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

                fitnessSum -= fitnesses[firstParent];

                //choose second parent
                var secondWheelTurning = Random.NextDouble() * fitnessSum - firstParentProbabilityValue;

                correctedProbabilityValues.Add(secondWheelTurning);
                correctedProbabilityValues.Sort();

                var secondParentIndex = SearchHelper.BinarySearch(correctedProbabilityValues, secondWheelTurning);
                var secondParent = probabilities[probabilityValues[secondParentIndex]];

                pairs.Add((firstParent, secondParent));
                fitnessSum += fitnesses[firstParent];
            }

            return pairs;
        }
    }
}
