using System;
using System.Collections.Generic;
using System.Linq;

namespace TSP.Core
{
	public class TSPModel
	{
		private readonly IEnumerable<TSPNode> nodes;

		private Dictionary<TSPNode, Dictionary<TSPNode, double>> nodesDistancesMap;

		public IList<TSPNode> Nodes => nodes.ToList();

		public TSPModel(IList<TSPNode> nodes)
		{
			this.nodes = nodes;
			nodesDistancesMap = new Dictionary<TSPNode, Dictionary<TSPNode, double>>();

			for (var i = 0; i < nodes.Count; i++)
			{
				nodesDistancesMap.Add(nodes[i], new Dictionary<TSPNode, double>());

				for (var j = 0; j < nodes.Count; j++)
				{
					if (i == j)
						continue;

					nodesDistancesMap[nodes[i]].Add(nodes[j], GetSectionDistance(nodes[i], nodes[j]));
				}
			}
		}

		public double GetSectionDistance(TSPNode first, TSPNode second) =>
			Math.Sqrt(Math.Pow(first.X - second.X, 2)
					+ Math.Pow(first.Y - second.Y, 2));

		public double GetDistance(IList<TSPNode> route, bool isClosedRoute = true)
		{
			var distance = 0D;

			if (nodes.Except(route).Any())
				throw new ArgumentException("Route nodes are not correspond to model nodes set");

			for (var i = 0; i < route.Count - 1; i++)
				distance += nodesDistancesMap[route[i]][route[i + 1]];

			if (isClosedRoute)
				distance += nodesDistancesMap[route.Last()][route.First()];

			return distance;
		}
	}
}
