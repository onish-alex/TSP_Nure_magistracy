using GA.Core.Models;
using GA.Core.Operations.Crossovers;
using GA.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GA.Operations.Crossovers
{
	public class SinglePointOrderedCrossover : BaseCrossover
	{
		public int PointIndex { get; set; }

		public SinglePointOrderedCrossover(GAOperationSettings operationSettings) : base(operationSettings) { }

		public override IList<Individual<TGene>> GetNextGeneration<TGene>(IList<(Individual<TGene>, Individual<TGene>)> parents)
		{
			IList<Individual<TGene>> children = new List<Individual<TGene>>(parents.Count * 2);

			if (operationSettings.InitType == GAOperationInitType.EveryGeneration)
				InitSettings();

			foreach (var pair in parents)
			{
				var firstChildGenome = new List<TGene>();
				var secondChildGenome = new List<TGene>();

				if (operationSettings.InitType == GAOperationInitType.EveryIndividual)
					InitSettings();

				//for (int i = 0; i < PointIndex; i++)
				//{
				//    firstChildGenome.Add(pair.Item1[i]);
				//    secondChildGenome.Add(pair.Item2[i]);
				//}

				//for (int i = 0; i < operationSettings.NodesCount; i++)
				//{
				//    if (!firstChildGenome.Contains(pair.Item2[i])) 
				//        firstChildGenome.Add(pair.Item2[i]);

				//    if (!secondChildGenome.Contains(pair.Item1[i]))    
				//        secondChildGenome.Add(pair.Item1[i]);
				//}

				Func<TGene, int, bool> pointIndexPredicate = (x, i) => i < PointIndex;

				firstChildGenome.AddRange(pair.Item1.TakeWhile(pointIndexPredicate));
				secondChildGenome.AddRange(pair.Item2.TakeWhile(pointIndexPredicate));

				firstChildGenome.AddRange(pair.Item2.Except(firstChildGenome));
				secondChildGenome.AddRange(pair.Item1.Except(secondChildGenome));

				//children.Add(Individual<TGene>.GetInstance<TIndividual>(firstChildGenome));
				//children.Add(Individual<TGene>.GetInstance<TIndividual>(secondChildGenome));

				children.Add(new Individual<TGene>(firstChildGenome));
				children.Add(new Individual<TGene>(secondChildGenome));
			}

			return children;
		}

		protected override void InitSettings()
		{
			PointIndex = Random.Shared.Next(1, operationSettings.NodesCount);
		}
	}
}
