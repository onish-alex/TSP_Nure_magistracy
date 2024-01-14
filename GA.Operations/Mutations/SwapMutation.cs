using Algorithms.Utility.Extensions;
using GA.Core.Operations.Mutations;
using GA.Core.Utility;
using System;
using System.Collections.Generic;

namespace GA.Operations.Mutations
{
	public class SwapMutation : ParallelBaseMutation
	{
		public int SwapSectionLength { get; set; }

		public SwapMutation(GAOperationSettings operationSettings) : base(operationSettings) { }

		public override void ProcessMutation<TIndividual, TGene>(IList<TIndividual> population, double probability)
		{
			if (operationSettings.InitType == GAOperationInitType.EveryGeneration)
				InitSettings();

			for (var i = 0; i < population.Count; i++)
			{
				if (Random.Shared.CheckProbability(probability))
				{
					if (operationSettings.InitType == GAOperationInitType.EveryIndividual)
						InitSettings();

					var firstSectionIndex = Random.Shared.Next(0, population[i].Count - SwapSectionLength + 1);
					var firstRange = population[i].GetRange(firstSectionIndex, SwapSectionLength);
					population[i].RemoveRange(firstSectionIndex, SwapSectionLength);

					var secondSectionIndex = Random.Shared.Next(0, population[i].Count - SwapSectionLength + 1);
					var secondRange = population[i].GetRange(secondSectionIndex, SwapSectionLength);
					population[i].RemoveRange(secondSectionIndex, SwapSectionLength);

					for (var j = 0; j < SwapSectionLength; j++)
						if (firstSectionIndex + j > population[i].Count - 1)
							population[i].Add(secondRange[j]);
						else
							population[i].Insert(firstSectionIndex + j, secondRange[j]);

					for (var j = 0; j < SwapSectionLength; j++)
						if (secondSectionIndex + j > population[i].Count - 1)
							population[i].Add(firstRange[j]);
						else
							population[i].Insert(secondSectionIndex + j, firstRange[j]);
				}
			}
		}

		protected override void InitSettings()
		{
			SwapSectionLength = Random.Shared.Next(1, operationSettings.NodesCount / 2);
		}
	}
}
