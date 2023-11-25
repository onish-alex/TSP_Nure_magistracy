using Algorithms.Utility.Extensions;
using GA.Core.Models;
using GA.Core.Operations.Crossovers;
using GA.Core.Utility;
using System;
using System.Collections.Generic;

namespace GA.Operations.Crossovers
{
    [Obsolete("In development")]
    public class BitMaskCrossover : BaseCrossover
    {
        public IList<bool> Mask { get; set; }

        public BitMaskCrossover(GAOperationSettings operationSettings) : base(operationSettings) { }

        public override IList<TIndividual> GetNextGeneration<TIndividual, TGene>(IList<(TIndividual, TIndividual)> parents)
        {
            IList<TIndividual> children = new List<TIndividual>(parents.Count * 2);

            if (operationSettings.InitType == GAOperationInitType.EveryGeneration)
                InitSettings();

            foreach (var pair in parents)
            {
                var firstChildGenome = new List<TGene>(pair.Item1.Count);
                var secondChildGenome = new List<TGene>(pair.Item2.Count);

                if (operationSettings.InitType == GAOperationInitType.EveryIndividual)
                    InitSettings();

                for (var i = 0; i < operationSettings.NodesCount; i++)
                {
                    if (Mask[i])
                    {
                        firstChildGenome.Add(pair.Item1[i]);
                        secondChildGenome.Add(pair.Item2[i]);
                    }
                    else
                    {
                        firstChildGenome.Add(pair.Item2[i]);
                        secondChildGenome.Add(pair.Item1[i]);
                    }
                }

                children.Add(Individual<TGene>.GetInstance<TIndividual>(firstChildGenome));
                children.Add(Individual<TGene>.GetInstance<TIndividual>(secondChildGenome));
            }

            return children;
        }

        protected override void InitSettings()
        {
            Mask = new List<bool>();

            for (var i = 0; i < operationSettings.NodesCount; i++)
                Mask.Add(Random.Shared.CheckProbability(50D));
        }
    }
}
