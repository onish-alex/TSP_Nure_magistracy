using GA.Core.Models;
using GA.Core.Operations.Crossovers;
using GA.Core.Utility;
using System;
using System.Collections.Generic;

namespace GA.Operations.Crossovers
{
    public class SinglePointCrossover : BaseCrossover
    {
        public int PointIndex { get; set; }

        public SinglePointCrossover(GAOperationSettings operationSettings) : base(operationSettings) { }

        public override IList<TIndividual> GetNextGeneration<TIndividual, TGene>(IList<(TIndividual, TIndividual)> parents)
        {
            IList<TIndividual> children = new List<TIndividual>(parents.Count * 2);

            if (operationSettings.InitType == GAOperationInitType.EveryGeneration)
                InitSettings();

            foreach (var pair in parents)
            {
                var firstChildGenome = new List<TGene>();
                var secondChildGenome = new List<TGene>();

                if (operationSettings.InitType == GAOperationInitType.EveryIndividual)
                    InitSettings();

                for (int i = 0; i < PointIndex; i++)
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
            }

            return children;
        }

        protected override void InitSettings()
        {
            PointIndex = Random.Shared.Next(1, operationSettings.NodesCount);
        }
    }
}
