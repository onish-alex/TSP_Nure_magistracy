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

				children.Add(Individual<TGene>.GetInstance<TIndividual>(pair.Item1.Take(pointIndex).Concat(pair.Item2.TakeLast(pair.Item2.Count - pointIndex)).ToList()));
				children.Add(Individual<TGene>.GetInstance<TIndividual>(pair.Item2.Take(pointIndex).Concat(pair.Item1.TakeLast(pair.Item1.Count - pointIndex)).ToList()));
			}

			return children;
		}
	}
}
