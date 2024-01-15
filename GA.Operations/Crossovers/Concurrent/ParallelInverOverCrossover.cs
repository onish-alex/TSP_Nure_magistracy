using Algorithms.Utility.Extensions;
using GA.Core.Models;
using GA.Core.Operations.Crossovers;
using GA.Core.Operations.Crossovers.Concurrent;
using GA.Core.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Operations.Crossovers
{
	public class ParallelInverOverCrossover : ParallelBaseCrossover
	{
		public int sPosition { get; set; }
		public int kPosition { get; set; }

		public ParallelInverOverCrossover(GAOperationSettings operationSettings) : base(operationSettings)
		{
		}

		public override IList<Individual<TGene>> GetNextGeneration<TGene>(IList<(Individual<TGene>, Individual<TGene>)> parents)
		{
			var children = new ConcurrentBag<Individual<TGene>>();

			if (operationSettings.InitType == GAOperationInitType.EveryGeneration)
				InitSettings();

			Parallel.ForEach(parents, parallelOptions, (pair) =>
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
				
				//children.Add(Individual<TGene>.GetInstance<TIndividual>(firstChildGenome));
				//children.Add(Individual<TGene>.GetInstance<TIndividual>(secondChildGenome));

				children.Add(new Individual<TGene>(firstChildGenome));
				children.Add(new Individual<TGene>(secondChildGenome));
			});

			return children.ToList();
		}

		protected override void InitSettings()
		{
			sPosition = Random.Shared.Next(0, operationSettings.NodesCount - 1);
			kPosition = Random.Shared.Next(0, operationSettings.NodesCount - 1);
		}
	}
}
