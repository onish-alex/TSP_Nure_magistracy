using GA.Core.Models;
using GA.Core.Operations.Crossovers;
using GA.Core.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Operations.Crossovers.Concurrent
{
    public class ParallelSinglePointOrderedCrossover : BaseCrossover
    {
        public int PointIndex { get; set; }

        public ParallelSinglePointOrderedCrossover(GAOperationSettings operationSettings) : base(operationSettings) { }

        public override IList<TIndividual> GetNextGeneration<TIndividual, TGene>(IList<(TIndividual, TIndividual)> parents)
        {
            ConcurrentBag<TIndividual> children = new ConcurrentBag<TIndividual>();

            if (operationSettings.InitType == GAOperationInitType.EveryGeneration)
                InitSettings();

            Parallel.ForEach(parents, new ParallelOptions() {  }, (pair, state) =>
            {
                var firstChildGenome = new List<TGene>();
                var secondChildGenome = new List<TGene>();

                var pointIndex = PointIndex;

                if (operationSettings.InitType == GAOperationInitType.EveryIndividual)
                    InitSettingsInner(out pointIndex);

                for (int i = 0; i < pointIndex; i++)
                {
                    firstChildGenome.Add(pair.Item1[i]);
                    secondChildGenome.Add(pair.Item2[i]);
                }

                for (int i = 0; i < operationSettings.NodesCount; i++)
                {
                    if (!firstChildGenome.Contains(pair.Item2[i]))
                        firstChildGenome.Add(pair.Item2[i]);

                    if (!secondChildGenome.Contains(pair.Item1[i]))
                        secondChildGenome.Add(pair.Item1[i]);
                }

                children.Add(Individual<TGene>.GetInstance<TIndividual>(firstChildGenome));
                children.Add(Individual<TGene>.GetInstance<TIndividual>(secondChildGenome));
            });

            return children.ToList();
        }

        protected override void InitSettings()
        {
            InitSettingsInner(out int pointIndex);
            PointIndex = pointIndex;
        }

        private void InitSettingsInner(out int pointIndex)
        {
            pointIndex = Random.Shared.Next(1, operationSettings.NodesCount);
        }
    }
}
