using GA.Core.Models;
using GA.Core.Operations.Crossovers;
using GA.Core.Utility;
using System;
using System.Collections.Generic;

namespace GA.Operations.Crossovers
{
	public class TwoPointOrderedCrossover : BaseCrossover
	{
		public int FirstPointIndex { get; set; } = -1;

		public int SecondPointIndex { get; set; } = -1;

		public TwoPointOrderedCrossover(GAOperationSettings operationSettings) : base(operationSettings)
		{
		}

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

				for (int i = FirstPointIndex; i <= SecondPointIndex; i++)
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
			}

			return children;
		}

		protected override void InitSettings()
		{
			FirstPointIndex = Random.Shared.Next(operationSettings.NodesCount - 1);

			SecondPointIndex = (FirstPointIndex == operationSettings.NodesCount - 2)
				? operationSettings.NodesCount - 1
				: Random.Shared.Next(FirstPointIndex, operationSettings.NodesCount);
		}
	}
}
