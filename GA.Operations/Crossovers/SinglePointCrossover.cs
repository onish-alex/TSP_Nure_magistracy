using GA.Core.Models;
using GA.Core.Operations.Crossovers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GA.Operations.Crossovers
{
    public class SinglePointCrossover : BaseCrossover
    {
        public bool IsRandomPoint { get; set; }

        public int PointIndex { get; set; }

        public override IList<TIndividual> GetNextGeneration<TIndividual, TGene>(IList<(TIndividual, TIndividual)> parents)
        {
            IList<TIndividual> children = new List<TIndividual>(parents.Count);

            int pointIndex = PointIndex;

            foreach (var pair in parents)
            {
                if (IsRandomPoint)
                    pointIndex = Random.Next(1, pair.Item1.Count);

                var firstChildGenome = new List<TGene>();
                var secondChildGenome = new List<TGene>();

                for (int i = 0; i < pointIndex; i++)
                {
                    firstChildGenome.Add(pair.Item1[i]);
                    secondChildGenome.Add(pair.Item2[i]);
                }

                for (int i = pointIndex; i < pair.Item1.Count; i++)
                {
                    firstChildGenome.Add(pair.Item2[i]);
                    secondChildGenome.Add(pair.Item1[i]);
                }

                children.Add(Individual<TGene>.GetInstance<TIndividual>(firstChildGenome));
                children.Add(Individual<TGene>.GetInstance<TIndividual>(secondChildGenome));
            }

            return children;
        }
    }
}
