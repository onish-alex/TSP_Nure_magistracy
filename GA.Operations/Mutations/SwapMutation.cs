using Algorithms.Utility.Extensions;
using GA.Core.Operations.Mutations;
using System;
using System.Collections.Generic;

namespace GA.Operations.Mutations
{
    public class SwapMutation : BaseMutation
    {
        public int SwapSectionLength { get; set; } = 1;

        public override void ProcessMutation<TIndividual, TGene>(IList<TIndividual> population, double probability)
        {
            for (var i = 0; i < population.Count; i++)
            {
                if (Random.CheckProbability(probability))
                {
                    if (SwapSectionLength > population[i].Count / 2 || SwapSectionLength < 1)
                        SwapSectionLength = Random.Next(1, population[i].Count / 2);

                    var firstSectionIndex = Random.Next(0, population[i].Count - SwapSectionLength + 1);
                    var firstRange = population[i].GetRange(firstSectionIndex, SwapSectionLength);
                    population[i].RemoveRange(firstSectionIndex, SwapSectionLength);

                    var secondSectionIndex = Random.Next(0, population[i].Count - SwapSectionLength + 1);
                    var secondRange = population[i].GetRange(secondSectionIndex, SwapSectionLength);
                    population[i].RemoveRange(secondSectionIndex, SwapSectionLength);

                    for (var j = 0; j < SwapSectionLength; j++)
                        if (firstSectionIndex + j > population[i].Count - 1)
                            population[i].Add(secondRange[j]);
                        else
                            population[i].Insert(firstSectionIndex + j, secondRange[j]);

                    for (var j = 0; j < SwapSectionLength; j++)
                        if (secondSectionIndex + j > population[i].Count - 1)
                            population[i].Add(firstRange[j]);
                        else
                            population[i].Insert(secondSectionIndex + j, firstRange[j]);
                }
            }
        }
    }
}
