using AntColony.Core.Utilities;
using System;
using System.Collections.Generic;

namespace AntColony.Core
{
	public abstract class BaseAlgorithm<TNode> where TNode : class
	{
		protected IList<TNode> nodes;
		protected Func<TNode, TNode, double> edgeDistanceGetter;
		protected Dictionary<TNode, Dictionary<TNode, double>> pheromoneMap;
		protected AntColonySettings settings;

		protected BaseAlgorithm(IList<TNode> nodes, Func<TNode, TNode, double> edgeDistanceGetter, AntColonySettings settings)
		{
			this.nodes = nodes;
			this.edgeDistanceGetter = edgeDistanceGetter;
			this.settings = settings;

			pheromoneMap = new Dictionary<TNode, Dictionary<TNode, double>>();

			for (var i = 0; i < nodes.Count; i++)
			{
				pheromoneMap.Add(nodes[i], new Dictionary<TNode, double>());

				for (var j = 0; j < nodes.Count; j++)
				{
					if (i == j)
						continue;

					pheromoneMap[nodes[i]].Add(nodes[j], settings.InitialPheromoneAmount);
				}
			}
		}

		public abstract IList<IList<TNode>> Run(AntPopulationSettings antSettings);

		private protected abstract void TravelPath(Ant<TNode> ant);

		protected virtual double GetTravelProbability(TNode currentNode, TNode unvisitedNode)
		{
			return Math.Pow(pheromoneMap[currentNode][unvisitedNode], settings.PheromoneWeight) + Math.Pow(1 / edgeDistanceGetter(currentNode, unvisitedNode), settings.DistanceWeight);
		}

		protected abstract void EvaporatePheromones();

		protected virtual List<Ant<TNode>> TravelAllPaths(int antCount)
		{
			var ants = new List<Ant<TNode>>(antCount);

			for (int i = 0; i < antCount; i++)
			{
				var ant = new Ant<TNode>();
				ants.Add(ant);
				TravelPath(ant);
			}

			return ants;
		}
	}
}