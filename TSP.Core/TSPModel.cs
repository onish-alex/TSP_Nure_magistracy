using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TSP.Core
{
	public class TSPModel
	{
		private readonly IEnumerable<TSPNode> nodes;

		private Dictionary<TSPNode, Dictionary<TSPNode, double>> nodesDistancesMap;

		public Dictionary<TSPNode, Dictionary<TSPNode, double>> DistancesMap => nodesDistancesMap.ToDictionary(x => x.Key, x => x.Value);

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


		/// <summary>
		/// Parses given list of nodes to nodes that belongs to current model
		/// </summary>
		public IList<TSPNode> ParseNodes(IList<TSPNode> nodes)
		{
			var threshold = 0.0000000001D;
			var modelPoints = new List<TSPNode>();

			foreach (var node in nodes)
			{
				var parsedNode = this.Nodes.FirstOrDefault(modelNode =>
					Math.Abs(node.X - modelNode.X) <= threshold
				 && Math.Abs(node.Y - modelNode.Y) <= threshold);

				if (parsedNode != null)
					modelPoints.Add(parsedNode);
			}

			return modelPoints;
		}
	}
}
