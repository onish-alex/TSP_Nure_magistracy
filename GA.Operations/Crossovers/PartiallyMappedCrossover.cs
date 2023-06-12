using GA.Core.Models;
using GA.Core.Operations.Crossovers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Operations.Crossovers
{
    public class PartiallyMappedCrossover : BaseCrossover
    {
        public int IntervalStartIndex { get; set; } = -1;
        public int IntervalEndIndex { get; set; } = -1;
        public bool UsePermanentParams { get; set; } = true;

        public override IList<TIndividual> GetNextGeneration<TIndividual, TGene>(IList<(TIndividual, TIndividual)> parents)
        {
            if ((IntervalStartIndex == -1
             || IntervalEndIndex == -1
             || IntervalEndIndex < IntervalStartIndex)
             && UsePermanentParams)
            {
                var genomeLength = parents.First().Item1.Count;

                IntervalStartIndex = Random.Next(genomeLength);
                IntervalEndIndex = (IntervalStartIndex == genomeLength - 1)
                    ? IntervalStartIndex
                    : Random.Next(IntervalEndIndex, genomeLength);
            }

            IList<TIndividual> children = new List<TIndividual>(parents.Count * 2);

            foreach (var pair in parents)
            {
                var firstChildGenome = new List<TGene>();
                var secondChildGenome = new List<TGene>();

                if (!UsePermanentParams)
                {
                    var genomeLength = parents.First().Item1.Count;

                    IntervalStartIndex = Random.Next(genomeLength);
                    IntervalEndIndex = (IntervalStartIndex == genomeLength - 1)
                        ? IntervalStartIndex
                        : Random.Next(IntervalEndIndex, genomeLength);
                }

                var firstInterval = pair.Item1.GetRange(IntervalStartIndex, IntervalEndIndex - IntervalStartIndex + 1);
                var secondInterval = pair.Item2.GetRange(IntervalStartIndex, IntervalEndIndex - IntervalStartIndex + 1);

                firstChildGenome.AddRange(firstInterval);
                secondChildGenome.AddRange(secondInterval);

                for (var i = 0; i < IntervalStartIndex; i++)
                {
                    var firstChildGene = pair.Item2[i];
                    var secondChildGene = pair.Item1[i];

                    if (firstInterval.Contains(pair.Item2[i]))
                        firstChildGene = MapGene(pair.Item2[i], firstInterval, secondInterval);

                    if (secondInterval.Contains(pair.Item1[i]))
                        secondChildGene = MapGene(pair.Item1[i], firstInterval, secondInterval);

                    firstChildGenome.Add(firstChildGene);
                    secondChildGenome.Add(secondChildGene);
                }
            }

            return children;
        }

        private TGene MapGene<TGene>(TGene repeatedGene, List<TGene> sourceInterval, List<TGene> otherInterval)
        {
            var mappedGene = repeatedGene;
            (List<TGene> Current, List<TGene> Searched) intervals = (sourceInterval, otherInterval);

            while (intervals.Current.Contains(mappedGene))
            {
                mappedGene = intervals.Searched[intervals.Current.IndexOf(mappedGene)];
                intervals = (intervals.Searched, intervals.Current);
            }

            return mappedGene;
        }
    }
}
