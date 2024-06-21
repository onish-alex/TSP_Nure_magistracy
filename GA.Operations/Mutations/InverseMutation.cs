using Algorithms.Utility.Extensions;
using GA.Core.Operations.Mutations;
using GA.Core.Utility;
using System;
using System.Collections.Generic;

namespace GA.Operations.Mutations
{
	public class InverseMutation : BaseMutation
	{
		public int IntervalStartIndex { get; set; } = -1;

		public int IntervalEndIndex { get; set; } = -1;

		public InverseMutation(GAOperationSettings operationSettings) : base(operationSettings) { }

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

					var rangeToInverse = population[i].GetRange(IntervalStartIndex, IntervalEndIndex - IntervalStartIndex + 1);
					population[i].RemoveRange(IntervalStartIndex, IntervalEndIndex - IntervalStartIndex + 1);

					rangeToInverse.Reverse();

					int j = IntervalStartIndex;
					foreach (var item in rangeToInverse)
						population[i].Insert(j++, item);
				}
			}
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
