using AntColony.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AntColony.Core
{
	//Best on 0.1 Evap., distance:pheromone 1:1,5
	public class MaxMinAlgorithm<TNode> : ClassicAlgorithm<TNode> where TNode : class
	{
		private Func<IList<TNode>, double> routeDistanceGetter;
		private IList<TNode> bestRoute;
		private double bestRouteLength;

		public MaxMinAlgorithm(IList<TNode> nodes, Func<TNode, TNode, double> edgeDistanceGetter, AntColonySettings settings, Func<IList<TNode>, double> routeDistanceGetter)
			: base(nodes, edgeDistanceGetter, settings)
		{
			this.routeDistanceGetter = routeDistanceGetter;
		}

		public override IList<IList<TNode>> Run(AntPopulationSettings antSettings)
		{
			var ants = TravelAllPaths(antSettings.AntCount);

			EvaporatePheromones();

			var routes = ants.Select(x => x.TravelledPathMemory).ToList();

			var bestRouteAnt = ants.OrderBy(x => routeDistanceGetter(x.TravelledPathMemory)).First();

			if (settings.UpdatePheromonesForGlobalBestWay)
			{
				var currentIterationBestRouteLength = routeDistanceGetter(bestRouteAnt.TravelledPathMemory);

				if (bestRoute == null || !bestRoute.Any() || bestRouteLength > currentIterationBestRouteLength)
				{
					bestRoute = bestRouteAnt.TravelledPathMemory.ToList();
					bestRouteLength = currentIterationBestRouteLength;
				}
			}

			var bestPathForUpdating = settings.UpdatePheromonesForGlobalBestWay
				? bestRoute
				: bestRouteAnt.TravelledPathMemory;

			for (var i = 0; i < bestPathForUpdating.Count - 1; i++)
			{
				var currentPheromoneAmount = pheromoneMap[bestPathForUpdating[i]][bestPathForUpdating[i + 1]];

				var pheromoneDelta = settings.UseCommonAntPheromoneAmount ? settings.CommonAntPheromoneAmount : bestRouteAnt.PersonalPheromoneAmount;
				pheromoneDelta /= edgeDistanceGetter(bestPathForUpdating[i], bestPathForUpdating[i + 1]);

				var edgeNewAmount = currentPheromoneAmount + pheromoneDelta;

				pheromoneMap[bestPathForUpdating[i]][bestPathForUpdating[i + 1]] =
					(edgeNewAmount > settings.MaxPheromoneAmount)
						? settings.MaxPheromoneAmount
						: edgeNewAmount;
			}

			return routes;
		}

		protected override void EvaporatePheromones()
		{
			for (int i = 0; i < nodes.Count; i++)
			{
				for (int j = 0; j < nodes.Count; j++)
				{
					if (i == j)
						continue;

					var edgeCurrentAmount = pheromoneMap[nodes[i]][nodes[j]];
					var edgeNewAmount = edgeCurrentAmount * (1 - settings.EvaporationCoefficient);

					pheromoneMap[nodes[i]][nodes[j]] = (edgeNewAmount < settings.MinPheromoneAmount)
						? settings.MinPheromoneAmount
						: edgeNewAmount;
				}
			}
		}

		protected virtual void RefreshPheromones()
		{
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
	}
}
