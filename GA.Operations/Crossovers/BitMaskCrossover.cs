using Algorithms.Utility.Extensions;
using GA.Core.Models;
using GA.Core.Operations.Crossovers;
using GA.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GA.Operations.Crossovers
{
	public class BitMaskCrossover : BaseCrossover
	{
		public IList<bool> Mask { get; set; }

		public BitMaskCrossover(GAOperationSettings operationSettings) : base(operationSettings) { }

		public override IList<Individual<TGene>> GetNextGeneration<TGene>(IList<(Individual<TGene>, Individual<TGene>)> parents)
		{
			IList<Individual<TGene>> children = new List<Individual<TGene>>(parents.Count * 2);

			if (operationSettings.InitType == GAOperationInitType.EveryGeneration)
				InitSettings();

			foreach (var pair in parents)
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

				//children.Add(Individual<TGene>.GetInstance<TIndividual>(firstChildGenome));
				//children.Add(Individual<TGene>.GetInstance<TIndividual>(secondChildGenome));

				children.Add(new Individual<TGene>(firstChildGenome));
				children.Add(new Individual<TGene>(secondChildGenome));
			}

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
