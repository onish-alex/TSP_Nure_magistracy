using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Core.Extensions;

namespace GA.Core.Mutations
{
    public class SwapMutation : Mutation
    {
        private int swapSectionLength = 1;
        public int SwapSectionLength 
        {
            get => swapSectionLength;
            set => swapSectionLength = value;
        }

        public override void ProcessMutation<TGene>(IList<IList<TGene>> population, double probability)
        {
            for (var i = 0; i < population.Count; i++)
            {
                if (Random.CheckProbability(probability))
                {
                    if (swapSectionLength > population[i].Count / 2)
                        swapSectionLength = population[i].Count / 2;

                    //var individual = population[i].ToList();

                    var firstSectionIndex = Random.Next(0, population[i].Count - swapSectionLength + 1);
                    var firstRange = (population[i] as List<TGene>).GetRange(firstSectionIndex, swapSectionLength);
                    (population[i] as List<TGene>).RemoveRange(firstSectionIndex, swapSectionLength);
                    
                    var secondSectionIndex = Random.Next(0, population[i].Count - swapSectionLength + 1);
                    var secondRange = (population[i] as List<TGene>).GetRange(secondSectionIndex, swapSectionLength);
                    (population[i] as List<TGene>).RemoveRange(secondSectionIndex, swapSectionLength);

                    for (var j = 0; j < swapSectionLength; j++)
                        if (firstSectionIndex + j > population[i].Count - 1)
                            population[i].Add(secondRange[j]);
                        else
                            population[i].Insert(firstSectionIndex + j, secondRange[j]);

                    for (var j = 0; j < swapSectionLength; j++)
                        if (secondSectionIndex + j > population[i].Count - 1)
                            population[i].Add(firstRange[j]);
                        else
                            population[i].Insert(secondSectionIndex + j, firstRange[j]);
                    
                }
            }
        }
    }
}
