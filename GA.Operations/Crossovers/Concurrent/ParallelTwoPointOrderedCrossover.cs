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
	public class ParallelTwoPointOrderedCrossover : ParallelBaseCrossover
	{
		public int FirstPointIndex { get; set; } = -1;

		public int SecondPointIndex { get; set; } = -1;

		public ParallelTwoPointOrderedCrossover(GAOperationSettings operationSettings) : base(operationSettings)
		{
		}

		public override IList<TIndividual> GetNextGeneration<TIndividual, TGene>(IList<(TIndividual, TIndividual)> parents)
		{
			ConcurrentBag<TIndividual> children = new ConcurrentBag<TIndividual>();

			if (operationSettings.InitType == GAOperationInitType.EveryGeneration)
				InitSettings();

			Parallel.ForEach(parents, parallelOptions, (pair) =>
			{
				var firstChildGenome = new List<TGene>();
				var secondChildGenome = new List<TGene>();

				var firstPointIndex = FirstPointIndex;
				var secondPointIndex = SecondPointIndex;

				if (operationSettings.InitType == GAOperationInitType.EveryIndividual)
					InitSettingsInner(out firstPointIndex, out secondPointIndex);

				for (int i = firstPointIndex; i <= secondPointIndex; i++)
				{
					firstChildGenome.Add(pair.Item1[i]);
					secondChildGenome.Add(pair.Item2[i]);
				}

				var firstChildInsertIndex = 0;
				var secondChildInsertIndex = 0;

				for (int i = 0; i < operationSettings.NodesCount; i++)
				{
					if (!firstChildGenome.Contains(pair.Item2[i]))
					{
						if (firstChildGenome.Count >= SecondPointIndex + 1)
							firstChildGenome.Add(pair.Item2[i]);
						else
							firstChildGenome.Insert(firstChildInsertIndex++, pair.Item2[i]);
					}

					if (!secondChildGenome.Contains(pair.Item1[i]))
					{
						if (secondChildGenome.Count >= SecondPointIndex + 1)
							secondChildGenome.Add(pair.Item1[i]);
						else
							secondChildGenome.Insert(secondChildInsertIndex++, pair.Item1[i]);
					}

				}

				children.Add(Individual<TGene>.GetInstance<TIndividual>(firstChildGenome));
				children.Add(Individual<TGene>.GetInstance<TIndividual>(secondChildGenome));
			});

			return children.ToList();
		}

		protected override void InitSettings()
		{
			InitSettingsInner(out int firstPointIndex, out int secondPointIndex);
			FirstPointIndex = firstPointIndex;
			SecondPointIndex = secondPointIndex;
		}

		private void InitSettingsInner(out int firstPointIndex, out int secondPointIndex)
		{
			firstPointIndex = Random.Shared.Next(operationSettings.NodesCount - 1);

			secondPointIndex = (firstPointIndex == operationSettings.NodesCount - 2)
				? operationSettings.NodesCount - 1
				: Random.Shared.Next(firstPointIndex, operationSettings.NodesCount);
		}
	}
}
