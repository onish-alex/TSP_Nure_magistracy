using GA.Core.Models;
using GA.Core.Operations.Crossovers;
using GA.Core.Utility;
using System;
using System.Collections.Generic;

namespace GA.Operations.Crossovers
{
	public class PartiallyMappedCrossover : BaseCrossover
	{
		public int IntervalStartIndex { get; set; } = -1;

		public int IntervalEndIndex { get; set; } = -1;

		public PartiallyMappedCrossover(GAOperationSettings operationSettings) : base(operationSettings) { }

		public override IList<Individual<TGene>> GetNextGeneration<TGene>(IList<(Individual<TGene>, Individual<TGene>)> parents)
		{
			if (operationSettings.InitType == GAOperationInitType.EveryGeneration)
				InitSettings();

			IList<Individual<TGene>> children = new List<Individual<TGene>>(parents.Count * 2);

			foreach (var pair in parents)
			{
				var firstChildGenome = new List<TGene>();
				var secondChildGenome = new List<TGene>();

				if (operationSettings.InitType == GAOperationInitType.EveryIndividual)
				{
					InitSettings();
				}

				var firstInterval = pair.Item1.GetRange(IntervalStartIndex, IntervalEndIndex - IntervalStartIndex + 1);
				var secondInterval = pair.Item2.GetRange(IntervalStartIndex, IntervalEndIndex - IntervalStartIndex + 1);

				firstChildGenome.AddRange(firstInterval);
				secondChildGenome.AddRange(secondInterval);

				for (var i = 0; i < IntervalStartIndex; i++)
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

				for (var i = IntervalEndIndex + 1; i < pair.Item1.Count; i++)
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
			}

			return children;
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
			IntervalStartIndex = Random.Shared.Next(operationSettings.NodesCount);

			IntervalEndIndex = (IntervalStartIndex == operationSettings.NodesCount - 1)
				? IntervalStartIndex
				: Random.Shared.Next(IntervalStartIndex, operationSettings.NodesCount);
		}
	}
}
