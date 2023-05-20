using Algorithms.Utility;
using AntColony.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AntColony.Core
{
    public class ClassicAlgorithm<TNode> : BaseAlgorithm<TNode> where TNode : class
    {
        public ClassicAlgorithm(IList<TNode> nodes, Func<TNode, TNode, double> edgeDistanceGetter, AntColonySettings settings) : base(nodes, edgeDistanceGetter, settings) { }

        public override IList<IList<TNode>> Run(int antCount)
        {
            var ants = new List<Ant<TNode>>();

            for (int i = 0; i < antCount; i++)
            {
                var ant = new Ant<TNode>();
                ants.Add(ant);
                TravelPath(ant);
            }

            EvaporatePheromones();

            foreach (var ant in ants)
            {
                for (var i = 0; i < ant.TravelledPathMemory.Count - 1; i++)
                {
                    var currentPheromoneAmount = pheromoneMap[ant.TravelledPathMemory[i]][ant.TravelledPathMemory[i + 1]];

                    var pheromoneDelta = settings.UseCommonAntPheromoneAmount ? settings.CommonAntPheromoneAmount : ant.PersonalPheromoneAmount;
                    pheromoneDelta /= edgeDistanceGetter(ant.TravelledPathMemory[i], ant.TravelledPathMemory[i + 1]);
                    
                    pheromoneMap[ant.TravelledPathMemory[i]][ant.TravelledPathMemory[i + 1]] = currentPheromoneAmount + pheromoneDelta;
                }
            }

            return ants.Select(x => x.TravelledPathMemory).ToList();
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
                    pheromoneMap[nodes[i]][nodes[j]] = edgeCurrentAmount * (1 - settings.EvaporationCoefficient);
                }
            }
        }

        protected override double GetTravelProbability(TNode currentNode, TNode unvisitedNode)
        {
            return Math.Pow(pheromoneMap[currentNode][unvisitedNode], settings.PheromoneWeight) + 1 / edgeDistanceGetter(currentNode, unvisitedNode);
        }

        private protected override void TravelPath(Ant<TNode> ant)
        {
            //write first node to ant memory
            ///TODO: always add first or random?
            var nodes = this.nodes.ToList();
            ant.TravelledPathMemory.Add(nodes[0]);
            nodes.RemoveAt(0);

            while (nodes.Any())
            {
                var probabilityIntervals = new List<double>();

                //writing intervals: 
                //0: from 0 to probability[0]
                //1: from probability[0] to (probability[0] + probability[1])
                //...
                //last: from (probability[0] + ... + probability[Count - 2]) to (probability[0] + ... + probability[Count - 1])
                //(probability[0] + ... + probability[Count - 1]) == Σ(probabilities)

                //Example:
                //t(ι1)^α * μ(ι2)^β = 10
                //t(ι1)^α * μ(ι3)^β = 12
                //t(ι1)^α * μ(ι4)^β = 15
                //t(ι1)^α * μ(ι5)^β = 20
                //t(ι1)^α * μ(ι6)^β = 19

                //Σ(t(ι1)^α * μ(ι6)^β) = 76

                //0: from 0                       to 10                           | 0 - 10
                //1: from (0 + 10)                to (0 + 10 + 12)                | 10 - 22
                //2: from (0 + 10 + 12)           to (0 + 10 + 15)                | 22 - 37
                //3: from (0 + 10 + 12 + 15)      to (0 + 10 + 12 + 15 + 20)      | 37 - 57
                //4: from (0 + 10 + 12 + 15 + 20) to (0 + 10 + 12 + 15 + 20 + 19) | 37 - 76

                //generating random value from 0 to 76, i.g. 53
                //53 in 37-57 interval, so ant chose "1-5" edge

                for (var i = 0; i < nodes.Count; i++)
                {
                    if (i == 0)
                        probabilityIntervals.Add(probabilities[ant.TravelledPathMemory.Last()][nodes[i]]);
                    else
                        probabilityIntervals.Add(probabilityIntervals.Last() + probabilities[ant.TravelledPathMemory.Last()][nodes[i]]);   
                }

                //
                var randomValue = random.NextDouble() * probabilities[ant.TravelledPathMemory.Last()].Values.Sum();

                probabilityIntervals.Add(randomValue);
                probabilityIntervals.Sort();

                var randomValueIndex = SearchHelper.BinarySearch(probabilityIntervals, randomValue);
                if (randomValueIndex != -1)
                {
                    var intervalIndex = (randomValueIndex == probabilityIntervals.Count - 1)
                        ? randomValueIndex - 1
                        : randomValueIndex;

                    probabilityIntervals.RemoveAt(randomValueIndex);

                    ant.TravelledPathMemory.Add(nodes[intervalIndex]);
                    nodes.RemoveAt(intervalIndex);
                }
            }
        }
    }
}
