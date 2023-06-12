using Algorithms.Utility.Extensions;
using GA.Core.Operations.Mutations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Operations.Mutations
{
    public class ShiftMutation : BaseMutation
    {
        /// <summary>
        /// amount of elements that will be shifted, from 1 to population.Count - 1
        ///setting population.Count makes no sence if searching for closed route
        /// </summary>
        public int SectionLength { get; set; }

        /// <summary>
        /// start index of section that will be shifted
        /// </summary>
        public int StartIndex { get; set; } = -1;

        /// <summary>
        /// defines how far section must be shifted, positive value to shift forward, negative to shift backward
        /// </summary>
        public int ShiftLength { get; set; }

        /// <summary>
        /// if parameters are not set and random setting is using, mutation setting applied to all individuals, otherwise it will be regenerated for each individual
        /// </summary>
        public bool UsePermanentParams { get; set; } = true;

        public override void ProcessMutation<TIndividual, TGene>(IList<TIndividual> population, double probability)
        {
            if (StartIndex < 0 && UsePermanentParams)
                StartIndex = Random.Next(0, population[0].Count);

            var sectionLengthMax = 0;
            if (UsePermanentParams)
                sectionLengthMax = population[0].Count - StartIndex;

            if (SectionLength <= 0 && UsePermanentParams)
                SectionLength = sectionLengthMax == 1
                    ? sectionLengthMax
                    : Random.Next(1, (population[0].Count - 1) - StartIndex); //max value exlusive, so param will be in [1, population.count - 1]

            if (ShiftLength == 0 && UsePermanentParams)
                ShiftLength = Random.CheckProbability(0.5)
                    ? Random.Next(1, population[0].Count - SectionLength)
                    : Random.Next(-population[0].Count - SectionLength - 1, 0); // 50/50 choosing [1, population.count - 1] or [-(population.count - 1), -1]

            for (var i = 0; i < population.Count; i++)
            {
                if (Random.CheckProbability(probability))
                {
                    if (!UsePermanentParams)
                    {
                        StartIndex = Random.Next(0, population[i].Count);

                        sectionLengthMax = population[i].Count - StartIndex;
                        
                        SectionLength = sectionLengthMax == 1 
                            ? sectionLengthMax 
                            : Random.Next(1, (population[i].Count - 1) - StartIndex); //max value exlusive, so param will be in [1, population.count - 1]

                        ShiftLength = Random.CheckProbability(0.5)
                            ? Random.Next(1, population[i].Count - SectionLength)
                            : Random.Next(-population[i].Count + SectionLength + 1, 0); // 50/50 choosing [1, population.count - 1] or [-(population.count - 1), -1]
                    }

                    var tempIndividualLength = population[i].Count - SectionLength;

                    var shiftedSection = population[i].GetRange(StartIndex, SectionLength);
                    population[i].RemoveRange(StartIndex, SectionLength);

                    var shiftedIndex = StartIndex + ShiftLength;

                    if (shiftedIndex < 0)
                        shiftedIndex += tempIndividualLength;
                    else if (shiftedIndex > tempIndividualLength - 1)
                        shiftedIndex -= tempIndividualLength;

                    for (var j = 0; j < shiftedSection.Count; j++)
                        population[i].Insert(shiftedIndex + j, shiftedSection[j]);
                }
            }
        }
    }
}
