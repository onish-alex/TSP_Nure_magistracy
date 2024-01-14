using Algorithms.Utility.Extensions;
using GA.Core.Operations.Mutations;
using GA.Core.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA.Operations.Mutations.Concurrent
{
	public class ParallelShiftMutation : ParallelBaseMutation
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

		public ParallelShiftMutation(GAOperationSettings operationSettings) : base(operationSettings) { }

		public override void ProcessMutation<TIndividual, TGene>(IList<TIndividual> population, double probability)
		{
			if (operationSettings.InitType == GAOperationInitType.EveryGeneration)
				InitSettings();

			Parallel.For(0, population.Count, parallelOptions, (i) =>
			{
				if (Random.Shared.CheckProbability(probability))
				{
					var startIndex = StartIndex;
					var sectionLength = SectionLength;
					var shiftLength = ShiftLength;

					if (operationSettings.InitType == GAOperationInitType.EveryIndividual)
						InitSettingsInner(out startIndex, out sectionLength, out shiftLength);

					var tempIndividualLength = population[i].Count - sectionLength;

					var shiftedSection = population[i].GetRange(startIndex, sectionLength);
					population[i].RemoveRange(startIndex, sectionLength);

					var shiftedIndex = startIndex + shiftLength;

					if (shiftedIndex < 0)
						shiftedIndex += tempIndividualLength;
					else if (shiftedIndex > tempIndividualLength - 1)
						shiftedIndex -= tempIndividualLength;

					for (var j = 0; j < shiftedSection.Count; j++)
						population[i].Insert(shiftedIndex + j, shiftedSection[j]);
				}
			});
		}

		protected override void InitSettings()
		{
			InitSettingsInner(out int startIndex, out int sectionLength, out int shiftLength);
			StartIndex = startIndex;
			SectionLength = sectionLength;
			ShiftLength = shiftLength;
		}

		private void InitSettingsInner(out int startIndex, out int sectionLength, out int shiftLength)
		{
			startIndex = Random.Shared.Next(0, operationSettings.NodesCount);

			var sectionLengthMax = operationSettings.NodesCount - startIndex;

			sectionLength = sectionLengthMax == 1
				? sectionLengthMax
				: Random.Shared.Next(1, (operationSettings.NodesCount - 1) - startIndex); //max value exlusive, so param will be in [1, population.count - 1]

			shiftLength = Random.Shared.CheckProbability(50D)
				? Random.Shared.Next(1, operationSettings.NodesCount - sectionLength)
				: Random.Shared.Next(-operationSettings.NodesCount + sectionLength + 1, 0); // 50/50 choosing [1, population.count - 1] or [-(population.count - 1), -1]
		}
	}
}
