using Algorithms.Utility.Extensions;
using GA.Core.Models;
using GA.Core.Operations.Crossovers;
using GA.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GA.Operations.Crossovers
{
	public class InverOverCrossover : BaseCrossover
	{
		public int sPosition { get; set; }
		public int kPosition { get; set; }

		public InverOverCrossover(GAOperationSettings operationSettings) : base(operationSettings)
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

				var sInverseIndex = sPosition + 1;
				var kInverseIndex = kPosition + 1;

				var sGene = firstChildGenome[sInverseIndex];
				var kGene = secondChildGenome[kInverseIndex];

				var firstChildAnotherInverseIndex = firstChildGenome.IndexOf(kGene);
				var firstChildMinInverseIndex = sInverseIndex < firstChildAnotherInverseIndex ? sInverseIndex : firstChildAnotherInverseIndex;
				var firstChildExcerptLength = Math.Abs(sInverseIndex - firstChildAnotherInverseIndex) + 1;
				var firstInverseExcerpt = firstChildGenome.GetRange(firstChildMinInverseIndex, firstChildExcerptLength).Reverse<TGene>();
				firstChildGenome.RemoveRange(firstChildMinInverseIndex, firstChildExcerptLength);
				firstChildGenome.InsertRange(firstChildMinInverseIndex, firstInverseExcerpt);

				var secondChildAnotherInverseIndex = secondChildGenome.IndexOf(sGene);
				var secondChildMinInverseIndex = kInverseIndex < secondChildAnotherInverseIndex ? kInverseIndex : secondChildAnotherInverseIndex;
				var secondChildExcerptLength = Math.Abs(kInverseIndex - secondChildAnotherInverseIndex) + 1;
				var secondInverseExcerpt = secondChildGenome.GetRange(secondChildMinInverseIndex, secondChildExcerptLength).Reverse<TGene>();
				secondChildGenome.RemoveRange(secondChildMinInverseIndex, secondChildExcerptLength);
				secondChildGenome.InsertRange(secondChildMinInverseIndex, secondInverseExcerpt);

				//foreach (var index in cycle)
				//{
				//	var swapBuffer = firstChildGenome[index];
				//	firstChildGenome[index] = secondChildGenome[index];
				//	secondChildGenome[index] = swapBuffer;
				//}

				//children.Add(Individual<TGene>.GetInstance<TIndividual>(firstChildGenome));
				//children.Add(Individual<TGene>.GetInstance<TIndividual>(secondChildGenome));

				children.Add(new Individual<TGene>(firstChildGenome));
				children.Add(new Individual<TGene>(secondChildGenome));
			}

			return children;
		}

		protected override void InitSettings()
		{
			sPosition = Random.Shared.Next(0, operationSettings.NodesCount - 1);
			kPosition = Random.Shared.Next(0, operationSettings.NodesCount - 1);
		}
	}
}
