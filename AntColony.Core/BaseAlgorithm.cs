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
        protected Dictionary<TNode, Dictionary<TNode, double>> probabilities;

        protected static Random random = new Random();

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

                    pheromoneMap[nodes[i]].Add(nodes[j], 0);
                }
            }

            //if α and β weigths are common for each ant
            if (settings.UseCommonWeights)
            {
                probabilities = new Dictionary<TNode, Dictionary<TNode, double>>();

                for (var i = 0; i < nodes.Count; i++)
                {
                    probabilities[nodes[i]] = new Dictionary<TNode, double>();

                    //if ij.length = ji.length, going only through edges above main diagonal
                    //var jj = settings.UseSymmetricDistances
                    //    ? i + 1
                    //    : 1;
                    var jj = 0;

                    for (var j = jj; j < nodes.Count; j++)
                    {
                        if (i == j)
                            continue;

                        var travelProbabilityPart = GetTravelProbability(nodes[i], nodes[j]);
                        probabilities[nodes[i]].Add(nodes[j], travelProbabilityPart);

                        //if (settings.UseSymmetricDistances)
                          //  probabilities[nodes[j]].Add(nodes[i], travelProbabilityPart);
                    }
                }
            }
        }

        public abstract IList<IList<TNode>> Run(int antCount);

        private protected abstract void TravelPath(Ant<TNode> ant);

        protected abstract double GetTravelProbability(TNode currentNode, TNode unvisitedNode);

        protected abstract void EvaporatePheromones();
    }
}