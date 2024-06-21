using Algorithms.Utility.Extensions;
using GA.Core.Operations.Mutations;
using GA.Core.Utility;
using System;
using System.Collections.Generic;

namespace GA.Operations.Mutations
{
	public class ShiftMutation : BaseMutation
	{
		/// <summary>
		/// amount of elements that will be shifted, from 1 to population.Count - 1
		///setting population.Count makes no sence if searching for closed route
		/// </summary>
		public int SectionLength { get; set; }

		/// <summary>
		/// start index of section that will be shifted
		/// </summary>
		public int StartIndex { get; set; } = -1;

		/// <summary>
		/// defines how far section must be shifted, positive value to shift forward, negative to shift backward
		/// </summary>
		public int ShiftLength { get; set; }

		public ShiftMutation(GAOperationSettings operationSettings) : base(operationSettings) { }

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

					var tempIndividualLength = population[i].Count - SectionLength;

					var shiftedSection = population[i].GetRange(StartIndex, SectionLength);
					population[i].RemoveRange(StartIndex, SectionLength);

					var shiftedIndex = StartIndex + ShiftLength;

					if (shiftedIndex < 0)
						shiftedIndex += tempIndividualLength;
					else if (shiftedIndex > tempIndividualLength - 1)
						shiftedIndex -= tempIndividualLength;

					for (var j = 0; j < shiftedSection.Count; j++)
						population[i].Insert(shiftedIndex + j, shiftedSection[j]);
				}
			}
		}

		protected override void InitSettings()
		{
			StartIndex = Random.Shared.Next(0, operationSettings.NodesCount);

			var sectionLengthMax = operationSettings.NodesCount - StartIndex;

			SectionLength = sectionLengthMax == 1
				? sectionLengthMax
				: Random.Shared.Next(1, (operationSettings.NodesCount - 1) - StartIndex); //max value exlusive, so param will be in [1, population.count - 1]

			ShiftLength = Random.Shared.CheckProbability(50D)
				? Random.Shared.Next(1, operationSettings.NodesCount - SectionLength)
				: Random.Shared.Next(-operationSettings.NodesCount + SectionLength + 1, 0); // 50/50 choosing [1, population.count - 1] or [-(population.count - 1), -1]
		}
	}
}
