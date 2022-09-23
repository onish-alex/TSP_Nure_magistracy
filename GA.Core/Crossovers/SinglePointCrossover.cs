using System;
using System.Collections.Generic;
using System.Linq;

namespace GA.Core.Crossovers
{
    public class SinglePointCrossover : Crossover
    {
        public bool IsRandomPoint { get; set; }

        public int PointValue { get; set; }

        public override IList<IList<TGene>> GetNextGeneration<TGene>(IList<(IList<TGene>, IList<TGene>)> parents)
        {
            var children = new List<IList<TGene>>(parents.Count);

            foreach (var pair in parents)
            {
                int point;

                if (IsRandomPoint)
                    point = Random.Next(1, pair.Item1.Count);
                else
                    point = PointValue;

                children.Add(pair.Item1.Take(point).Concat(pair.Item2.TakeLast(pair.Item2.Count - point)).ToList());
                children.Add(pair.Item2.Take(point).Concat(pair.Item1.TakeLast(pair.Item1.Count - point)).ToList());

            }

            return children;
        }
    }
}
