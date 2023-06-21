using Algorithms.Utility.Extensions;
using GA.Core.Operations.Mutations;
using GA.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Operations.Mutations.Concurrent
{
    public class ParallelInverseMutation : ParallelBaseMutation
    {
        public int IntervalStartIndex { get; set; } = -1;

        public int IntervalEndIndex { get; set; } = -1;

        public ParallelInverseMutation(GAOperationSettings operationSettings) : base(operationSettings) { }

        public override void ProcessMutation<TIndividual, TGene>(IList<TIndividual> population, double probability)
        {
            if (operationSettings.InitType == GAOperationInitType.EveryGeneration)
                InitSettings();

            Parallel.For(0, population.Count, parallelOptions, (i) =>
            {
                if (Random.Shared.CheckProbability(probability))
                {
                    var intervalStartIndex = IntervalStartIndex;
                    var intervalEndIndex = IntervalEndIndex;

                    if (operationSettings.InitType == GAOperationInitType.EveryIndividual)
                        InitSettingsInner(out intervalStartIndex, out intervalEndIndex);

                    var rangeToInverse = population[i].GetRange(intervalStartIndex, intervalEndIndex - intervalStartIndex + 1);
                    population[i].RemoveRange(intervalStartIndex, intervalEndIndex - intervalStartIndex + 1);

                    rangeToInverse.Reverse();

                    int j = intervalStartIndex;
                    foreach (var item in rangeToInverse)
                        population[i].Insert(j++, item);
                }
            });
        }

        protected override void InitSettings()
        {
            InitSettingsInner(out int intervalStartIndex, out int intervalEndIndex);
            IntervalStartIndex = intervalStartIndex;
            IntervalEndIndex = intervalEndIndex;
        }

        private void InitSettingsInner(out int intervalStartIndex, out int intervalEndIndex)
        {
            intervalStartIndex = Random.Shared.Next(operationSettings.NodesCount);

            intervalEndIndex = (intervalStartIndex == operationSettings.NodesCount - 1)
                ? intervalStartIndex
                : Random.Shared.Next(intervalStartIndex, operationSettings.NodesCount);
        }
    }
}
