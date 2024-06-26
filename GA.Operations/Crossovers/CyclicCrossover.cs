﻿using GA.Core.Models;
using GA.Core.Operations.Crossovers;
using GA.Core.Utility;
using System;
using System.Collections.Generic;

namespace GA.Operations.Crossovers
{
	public class CyclicCrossover : BaseCrossover
	{
		public int CycleSearchIndex { get; set; }

		public CyclicCrossover(GAOperationSettings operationSettings) : base(operationSettings)
		{
		}

		public override IList<Individual<TGene>> GetNextGeneration<TGene>(IList<(Individual<TGene>, Individual<TGene>)> parents)
		{
			IList<Individual<TGene>> children = new List<Individual<TGene>>(parents.Count * 2);

			if (operationSettings.InitType == GAOperationInitType.EveryGeneration)
				InitSettings();

			foreach (var pair in parents)
			{
				var firstChildGenome = new List<TGene>(pair.Item1);
				var secondChildGenome = new List<TGene>(pair.Item2);

				if (operationSettings.InitType == GAOperationInitType.EveryIndividual)
					InitSettings();

				var cycle = GetCycleIndexes(CycleSearchIndex, pair.Item1, pair.Item2);

				foreach (var index in cycle)
				{
					var swapBuffer = firstChildGenome[index];
					firstChildGenome[index] = secondChildGenome[index];
					secondChildGenome[index] = swapBuffer;
				}

				//children.Add(Individual<TGene>.GetInstance<TIndividual>(firstChildGenome));
				//children.Add(Individual<TGene>.GetInstance<TIndividual>(secondChildGenome));

				children.Add(new Individual<TGene>(firstChildGenome));
				children.Add(new Individual<TGene>(secondChildGenome));
			}

			return children;
		}

		private IEnumerable<int> GetCycleIndexes<TGene>(int startIndex, Individual<TGene> firstParent, Individual<TGene> secondParent)
		{
			var cycleIndexes = new List<int>() { startIndex };
			var currentItem = secondParent[startIndex];

			while (!currentItem.Equals(firstParent[startIndex]))
			{
				var cycleItemIndex = firstParent.IndexOf(currentItem);
				cycleIndexes.Add(cycleItemIndex);

				currentItem = secondParent[cycleItemIndex];
			}

			return cycleIndexes;
		}

		protected override void InitSettings()
		{
			CycleSearchIndex = Random.Shared.Next(0, operationSettings.NodesCount - 1);
		}
	}
}
