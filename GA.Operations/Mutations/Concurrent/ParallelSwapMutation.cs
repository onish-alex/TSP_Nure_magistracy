using Algorithms.Utility.Extensions;
using GA.Core.Operations.Mutations;
using GA.Core.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA.Operations.Mutations.Concurrent
{
	public class ParallelSwapMutation : ParallelBaseMutation
	{
		public int SwapSectionLength { get; set; }

		public ParallelSwapMutation(GAOperationSettings operationSettings) : base(operationSettings) { }

		public override void ProcessMutation<TIndividual, TGene>(IList<TIndividual> population, double probability)
		{
			if (operationSettings.InitType == GAOperationInitType.EveryGeneration)
				InitSettings();

			Parallel.For(0, population.Count, parallelOptions, (i) =>
			{
				if (Random.Shared.CheckProbability(probability))
				{
					var swapSectionLength = SwapSectionLength;

					if (operationSettings.InitType == GAOperationInitType.EveryIndividual)
						InitSettingsInner(out swapSectionLength);

					var firstSectionIndex = Random.Shared.Next(0, population[i].Count - swapSectionLength + 1);
					var firstRange = population[i].GetRange(firstSectionIndex, swapSectionLength);
					population[i].RemoveRange(firstSectionIndex, swapSectionLength);

					var secondSectionIndex = Random.Shared.Next(0, population[i].Count - swapSectionLength + 1);
					var secondRange = population[i].GetRange(secondSectionIndex, swapSectionLength);
					population[i].RemoveRange(secondSectionIndex, swapSectionLength);

					for (var j = 0; j < swapSectionLength; j++)
						if (firstSectionIndex + j > population[i].Count - 1)
							population[i].Add(secondRange[j]);
						else
							population[i].Insert(firstSectionIndex + j, secondRange[j]);

					for (var j = 0; j < swapSectionLength; j++)
						if (secondSectionIndex + j > population[i].Count - 1)
							population[i].Add(firstRange[j]);
						else
							population[i].Insert(secondSectionIndex + j, firstRange[j]);
				}
			});
		}

		protected override void InitSettings()
		{
			InitSettingsInner(out int swapSectionLength);
			SwapSectionLength = swapSectionLength;
		}

		private void InitSettingsInner(out int swapSectionLength)
		{
			swapSectionLength = Random.Shared.Next(1, operationSettings.NodesCount / 2);
		}
	}
}
