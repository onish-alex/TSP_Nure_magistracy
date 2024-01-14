using Algorithms.Utility.Extensions;
using GA.Core.Models;
using GA.Core.Operations.Crossovers.Concurrent;
using GA.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA.Operations.Crossovers
{
	[Obsolete("In development")]
	public class ParallelBitMaskCrossover : ParallelBaseCrossover
	{
		public IList<bool> Mask { get; set; }

		public ParallelBitMaskCrossover(GAOperationSettings operationSettings) : base(operationSettings) { }

		public override IList<TIndividual> GetNextGeneration<TIndividual, TGene>(IList<(TIndividual, TIndividual)> parents)
		{
			IList<TIndividual> children = new List<TIndividual>(parents.Count * 2);

			if (operationSettings.InitType == GAOperationInitType.EveryGeneration)
				InitSettings();

			Parallel.ForEach(parents, parallelOptions, (pair) =>
			{
				var firstChildGenome = new List<TGene>(pair.Item1.Count);
				var secondChildGenome = new List<TGene>(pair.Item2.Count);

				if (operationSettings.InitType == GAOperationInitType.EveryIndividual)
					InitSettings();

				for (var i = 0; i < operationSettings.NodesCount; i++)
				{
					if (Mask[i])
					{
						if (!firstChildGenome.Contains(pair.Item1[i]))
							firstChildGenome.Add(pair.Item1[i]);

						if (!secondChildGenome.Contains(pair.Item2[i]))
							secondChildGenome.Add(pair.Item2[i]);
					}
					else
					{
						if (!firstChildGenome.Contains(pair.Item2[i]))
							firstChildGenome.Add(pair.Item2[i]);

						if (!secondChildGenome.Contains(pair.Item1[i]))
							secondChildGenome.Add(pair.Item1[i]);
					}
				}

				firstChildGenome.AddRange(pair.Item1.Except(firstChildGenome));
				secondChildGenome.AddRange(pair.Item2.Except(secondChildGenome));

				children.Add(Individual<TGene>.GetInstance<TIndividual>(firstChildGenome));
				children.Add(Individual<TGene>.GetInstance<TIndividual>(secondChildGenome));
			});

			return children;
		}

		protected override void InitSettings()
		{
			Mask = new List<bool>();

			for (var i = 0; i < operationSettings.NodesCount; i++)
				Mask.Add(Random.Shared.CheckProbability(50D));
		}
	}
}
