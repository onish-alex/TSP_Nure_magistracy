using GA.Core.Models;
using GA.Core.Operations.Crossovers.Concurrent;
using GA.Core.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Operations.Crossovers.Concurrent
{
    public class ParallelCyclicCrossover : ParallelBaseCrossover
    {
        public int CycleSearchIndex { get; set; }

        public ParallelCyclicCrossover(GAOperationSettings operationSettings) : base(operationSettings)
        {
        }

        public override IList<TIndividual> GetNextGeneration<TIndividual, TGene>(IList<(TIndividual, TIndividual)> parents)
        {
            //IList<TIndividual> children = new List<TIndividual>(parents.Count * 2);
            var children = new ConcurrentBag<TIndividual>();

            if (operationSettings.InitType == GAOperationInitType.EveryGeneration)
                InitSettings();

            Parallel.ForEach(parents, parallelOptions, (pair) =>
            {
                var firstChildGenome = new List<TGene>(pair.Item1);
                var secondChildGenome = new List<TGene>(pair.Item2);

                var cycleSearchIndex = CycleSearchIndex;

                if (operationSettings.InitType == GAOperationInitType.EveryIndividual)
                    InitSettingsInner(out cycleSearchIndex);
                    
                var cycle = GetCycleIndexes(cycleSearchIndex, pair.Item1, pair.Item2);

                foreach (var index in cycle)
                    (secondChildGenome[index], firstChildGenome[index]) = (firstChildGenome[index], secondChildGenome[index]);

                children.Add(Individual<TGene>.GetInstance<TIndividual>(firstChildGenome));
                children.Add(Individual<TGene>.GetInstance<TIndividual>(secondChildGenome));
            });

            return children.ToList();
        }

        private IEnumerable<int> GetCycleIndexes<TGene>(int startIndex, Individual<TGene> firstParent, Individual<TGene> secondParent)
        {
            var cycleIndexes = new List<int>() { startIndex };
            var currentItem = secondParent[startIndex];

            while (!currentItem.Equals(firstParent[startIndex]))
            {
                var cycleItemIndex = firstParent.IndexOf(currentItem);
                cycleIndexes.Add(cycleItemIndex);

                currentItem = secondParent[cycleItemIndex];
            }

            return cycleIndexes;
        }

        protected override void InitSettings()
        {
            InitSettingsInner(out int cycleSearchIndex);
            CycleSearchIndex = cycleSearchIndex;
        }

        private void InitSettingsInner(out int cycleSearchIndex)
        {
            cycleSearchIndex = Random.Shared.Next(0, operationSettings.NodesCount - 1);
        }
    }
}
