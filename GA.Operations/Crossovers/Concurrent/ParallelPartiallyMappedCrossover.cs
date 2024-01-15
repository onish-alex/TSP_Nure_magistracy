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
	public class ParallelPartiallyMappedCrossover : ParallelBaseCrossover
	{
		public int IntervalStartIndex { get; set; } = -1;

		public int IntervalEndIndex { get; set; } = -1;

		public ParallelPartiallyMappedCrossover(GAOperationSettings operationSettings) : base(operationSettings) { }

		public override IList<Individual<TGene>> GetNextGeneration<TGene>(IList<(Individual<TGene>, Individual<TGene>)> parents)
		{
			if (operationSettings.InitType == GAOperationInitType.EveryGeneration)
				InitSettings();

			var children = new ConcurrentBag<Individual<TGene>>();

			Parallel.ForEach(parents, parallelOptions, (pair) =>
			{
				var firstChildGenome = new List<TGene>();
				var secondChildGenome = new List<TGene>();

				var intervalStartIndex = IntervalStartIndex;
				var intervalEndIndex = IntervalEndIndex;

				if (operationSettings.InitType == GAOperationInitType.EveryIndividual)
					InitSettingsInner(
						out intervalStartIndex,
						out intervalEndIndex);

				var firstInterval = pair.Item1.GetRange(intervalStartIndex, intervalEndIndex - intervalStartIndex + 1);
				var secondInterval = pair.Item2.GetRange(intervalStartIndex, intervalEndIndex - intervalStartIndex + 1);

				firstChildGenome.AddRange(firstInterval);
				secondChildGenome.AddRange(secondInterval);

				for (var i = 0; i < intervalStartIndex; i++)
				{
					var firstChildGene = pair.Item2[i];
					var secondChildGene = pair.Item1[i];

					if (firstInterval.Contains(pair.Item2[i]))
						firstChildGene = MapGene(pair.Item2[i], firstInterval, secondInterval);

					if (secondInterval.Contains(pair.Item1[i]))
						secondChildGene = MapGene(pair.Item1[i], secondInterval, firstInterval);

					firstChildGenome.Insert(i, firstChildGene);
					secondChildGenome.Insert(i, secondChildGene);
				}

				for (var i = intervalEndIndex + 1; i < pair.Item1.Count; i++)
				{
					var firstChildGene = pair.Item2[i];
					var secondChildGene = pair.Item1[i];

					if (firstInterval.Contains(pair.Item2[i]))
						firstChildGene = MapGene(pair.Item2[i], firstInterval, secondInterval);

					if (secondInterval.Contains(pair.Item1[i]))
						secondChildGene = MapGene(pair.Item1[i], secondInterval, firstInterval);

					firstChildGenome.Add(firstChildGene);
					secondChildGenome.Add(secondChildGene);
				}

				//children.Add(Individual<TGene>.GetInstance<TIndividual>(firstChildGenome));
				//children.Add(Individual<TGene>.GetInstance<TIndividual>(secondChildGenome));

				children.Add(new Individual<TGene>(firstChildGenome));
				children.Add(new Individual<TGene>(secondChildGenome));
			});

			return children.ToList();
		}

		private TGene MapGene<TGene>(TGene repeatedGene, List<TGene> sourceInterval, List<TGene> otherInterval)
		{
			var mappedGene = repeatedGene;

			while (sourceInterval.Contains(mappedGene))
				mappedGene = otherInterval[sourceInterval.IndexOf(mappedGene)];

			return mappedGene;
		}

		protected override void InitSettings()
		{
			InitSettingsInner(
				out int intervalStartIndex,
				out int intervalEndIndex);

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
