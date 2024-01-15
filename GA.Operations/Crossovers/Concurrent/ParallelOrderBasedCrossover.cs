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
	public class ParallelOrderBasedCrossover : ParallelBaseCrossover
	{
		private const double MIN_SELECTED_POSITIONS_PERCENT = 25D;
		private const double MAX_SELECTED_POSITIONS_PERCENT = 50D;

		public IList<int> SelectedPositionsIndexes { get; set; }

		public ParallelOrderBasedCrossover(GAOperationSettings operationSettings) : base(operationSettings)
		{
		}

		public override IList<Individual<TGene>> GetNextGeneration<TGene>(IList<(Individual<TGene>, Individual<TGene>)> parents)
		{
			var children = new ConcurrentBag<Individual<TGene>>();

			if (operationSettings.InitType == GAOperationInitType.EveryGeneration)
				InitSettings();

			Parallel.ForEach(parents, parallelOptions, (pair) =>
			{
				var firstChildGenome = new List<TGene>(Enumerable.Repeat(default(TGene), operationSettings.NodesCount));
				var secondChildGenome = new List<TGene>(Enumerable.Repeat(default(TGene), operationSettings.NodesCount));

				if (operationSettings.InitType == GAOperationInitType.EveryIndividual)
					InitSettings();

				foreach (var index in SelectedPositionsIndexes)
				{
					firstChildGenome[index] = pair.Item2[index];
					secondChildGenome[index] = pair.Item1[index];
				}

				var restIndexes = Enumerable.Range(0, operationSettings.NodesCount).Except(SelectedPositionsIndexes);
				var firstParentQueue = new Queue<TGene>(pair.Item1.Except(SelectedPositionsIndexes.Select(x => firstChildGenome[x])));
				var secondParentQueue = new Queue<TGene>(pair.Item2.Except(SelectedPositionsIndexes.Select(x => secondChildGenome[x])));

				foreach (var index in restIndexes)
				{
					firstChildGenome[index] = firstParentQueue.Dequeue();
					secondChildGenome[index] = secondParentQueue.Dequeue();
				}

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
			});

			return children.ToList();
		}

		protected override void InitSettings()
		{
			var selectedPositionsPercent = 
				Random.Shared.NextDouble() 
				* (MAX_SELECTED_POSITIONS_PERCENT - MIN_SELECTED_POSITIONS_PERCENT) 
				+ MIN_SELECTED_POSITIONS_PERCENT;

			var selectedPositionsCount = (int)Math.Round(operationSettings.NodesCount * selectedPositionsPercent / 100);
			SelectedPositionsIndexes = Random.Shared.GetUniqueRandomSet(Enumerable.Range(0, operationSettings.NodesCount).ToList(), selectedPositionsCount);
		}
	}
}
