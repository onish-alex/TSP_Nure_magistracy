using AntColony.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AntColony.Core.Concurrent
{
    public abstract class ParallelBaseAlgorithm<TNode> where TNode : class
    {
        protected IList<TNode> nodes;
        protected Func<TNode, TNode, double> edgeDistanceGetter;
        protected ConcurrentDictionary<TNode, ConcurrentDictionary<TNode, double>> pheromoneMap;
        protected AntColonySettings settings;

        protected ParallelBaseAlgorithm(IList<TNode> nodes, Func<TNode, TNode, double> edgeDistanceGetter, AntColonySettings settings)
        {
            this.nodes = nodes;
            this.edgeDistanceGetter = edgeDistanceGetter;
            this.settings = settings;

            pheromoneMap = new ConcurrentDictionary<TNode, ConcurrentDictionary<TNode, double>>();

            Parallel.For(0, nodes.Count, (i) =>
            {
                pheromoneMap.TryAdd(nodes[i], new ConcurrentDictionary<TNode, double>());

                for (var j = 0; j < nodes.Count; j++)
                {
                    if (i == j)
                        continue;

                    pheromoneMap[nodes[i]].TryAdd(nodes[j], settings.InitialPheromoneAmount);
                }
            });
        }

        public abstract IList<IList<TNode>> Run(AntPopulationSettings antSettings);

        private protected abstract void TravelPath(Ant<TNode> ant);

        protected virtual double GetTravelProbability(TNode currentNode, TNode unvisitedNode)
        {
            return Math.Pow(pheromoneMap[currentNode][unvisitedNode], settings.PheromoneWeight) + Math.Pow(1 / edgeDistanceGetter(currentNode, unvisitedNode), settings.DistanceWeight);
        }

        protected abstract void EvaporatePheromones();

        protected virtual ConcurrentBag<Ant<TNode>> TravelAllPaths(int antCount)
        {
            var ants = new ConcurrentBag<Ant<TNode>>();

            Parallel.For(0, antCount, (i) =>
            {
                var ant = new Ant<TNode>();
                ants.Add(ant);
                TravelPath(ant);
            });

            return ants;
        }
    }
}