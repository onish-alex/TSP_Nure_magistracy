using AntColony.Core.Utilities;
using System;
using System.Collections.Generic;

namespace AntColony.Core
{
    public abstract class BaseAlgorithm<TNode> where TNode : class
    {
        protected IList<TNode> nodes;
        protected Func<TNode, double> edgeDistanceGetter;
        protected Dictionary<TNode, Dictionary<TNode, double>> pheromoneMap;
        protected AntColonySettings settings;
        protected Dictionary<(TNode, TNode), double> probabilities;

        protected BaseAlgorithm(IList<TNode> nodes, Func<TNode, double> edgeDistanceGetter, AntColonySettings settings)
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

                    pheromoneMap[nodes[i]].Add(nodes[j], 0);
                }
            }

            if (settings.UseCommonWeights)
            {
                probabilities = new Dictionary<(TNode, TNode), double>();

                for (var i = 0; i < nodes.Count; i++)
                {
                    var j = settings.UseSymmetricDistances
                        ? i + 1
                        : 1;

                    for (j; j < nodes.Count; j++)
                    {
                        if (i == j)
                            continue;

                        var travelProbabilityPart = GetTravelProbability(nodes[i], nodes[j]);
                        probabilities.Add((nodes[i], nodes[j]), travelProbabilityPart);

                        if (settings.UseSymmetricDistances)
                            probabilities.Add((nodes[j], nodes[i]), travelProbabilityPart);
                    }
                }
            }
        }

        public abstract IList<IList<TNode>> Run(int antCount);

        private protected abstract void TravelPath(Ant<TNode> ant);

        protected abstract double GetTravelProbability(TNode currentNode, TNode unvisitedNode);
    }
}