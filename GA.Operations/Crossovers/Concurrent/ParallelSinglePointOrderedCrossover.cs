using GA.Core.Models;
using GA.Core.Operations.Crossovers.Concurrent;
using GA.Core.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Operations.Crossovers.Concurrent
{
	public class ParallelSinglePointOrderedCrossover : ParallelBaseCrossover
	{
		public int PointIndex { get; set; }

		public ParallelSinglePointOrderedCrossover(GAOperationSettings operationSettings) : base(operationSettings) { }

		public override IList<Individual<TGene>> GetNextGeneration<TGene>(IList<(Individual<TGene>, Individual<TGene>)> parents)
		{
			var children = new ConcurrentBag<Individual<TGene>>();

			if (operationSettings.InitType == GAOperationInitType.EveryGeneration)
				InitSettings();

			Parallel.ForEach(parents, parallelOptions, (pair, state) =>
			{
				var firstChildGenome = new List<TGene>();
				var secondChildGenome = new List<TGene>();

				var pointIndex = PointIndex;

				if (operationSettings.InitType == GAOperationInitType.EveryIndividual)
					InitSettingsInner(out pointIndex);

				//for (int i = 0; i < pointIndex; i++)
				//{
				//	firstChildGenome.Add(pair.Item1[i]);
				//	secondChildGenome.Add(pair.Item2[i]);
				//}

				//for (int i = 0; i < operationSettings.NodesCount; i++)
				//{
				//	if (!firstChildGenome.Contains(pair.Item2[i]))
				//		firstChildGenome.Add(pair.Item2[i]);

				//	if (!secondChildGenome.Contains(pair.Item1[i]))
				//		secondChildGenome.Add(pair.Item1[i]);
				//}

				Func<TGene, int, bool> pointIndexPredicate = (x, i) => i < PointIndex;

				firstChildGenome.AddRange(pair.Item1.TakeWhile(pointIndexPredicate));
				secondChildGenome.AddRange(pair.Item2.TakeWhile(pointIndexPredicate));

				firstChildGenome.AddRange(pair.Item2.Except(firstChildGenome));
				secondChildGenome.AddRange(pair.Item1.Except(secondChildGenome));

				//children.Add(Individual<TGene>.GetInstance<TIndividual>(firstChildGenome));
				//children.Add(Individual<TGene>.GetInstance<TIndividual>(secondChildGenome));

				children.Add(new Individual<TGene>(firstChildGenome));
				children.Add(new Individual<TGene>(secondChildGenome));
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
