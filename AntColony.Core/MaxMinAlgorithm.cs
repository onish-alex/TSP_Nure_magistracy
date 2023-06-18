using AntColony.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColony.Core
{
    //Best on 0.1 Evap., distance:pheromone 1:1,5
    public class MaxMinAlgorithm<TNode> : ClassicAlgorithm<TNode> where TNode : class
    {
        private Func<IList<TNode>, double> routeDistanceGetter;

        public MaxMinAlgorithm(IList<TNode> nodes, Func<TNode, TNode, double> edgeDistanceGetter, AntColonySettings settings, Func<IList<TNode>, double> routeDistanceGetter)
            : base(nodes, edgeDistanceGetter, settings) 
        {
            this.routeDistanceGetter = routeDistanceGetter;
        }

        public override IList<IList<TNode>> Run(AntPopulationSettings antSettings)
        {
            var ants = new List<Ant<TNode>>(antSettings.AntCount);

            for (int i = 0; i < antSettings.AntCount; i++)
            {
                var ant = new Ant<TNode>();
                ants.Add(ant);
                TravelPath(ant);
            }

            EvaporatePheromones();

            var routes = ants.Select(x => x.TravelledPathMemory).ToList();

            var bestRouteAnt = ants.OrderBy(x => routeDistanceGetter(x.TravelledPathMemory)).First();

            for (var i = 0; i < bestRouteAnt.TravelledPathMemory.Count - 1; i++)
            {
                var currentPheromoneAmount = pheromoneMap[bestRouteAnt.TravelledPathMemory[i]][bestRouteAnt.TravelledPathMemory[i + 1]];

                var pheromoneDelta = settings.UseCommonAntPheromoneAmount ? settings.CommonAntPheromoneAmount : bestRouteAnt.PersonalPheromoneAmount;
                pheromoneDelta /= edgeDistanceGetter(bestRouteAnt.TravelledPathMemory[i], bestRouteAnt.TravelledPathMemory[i + 1]);

                var edgeNewAmount = currentPheromoneAmount + pheromoneDelta;

                pheromoneMap[bestRouteAnt.TravelledPathMemory[i]][bestRouteAnt.TravelledPathMemory[i + 1]] = 
                    (edgeNewAmount > settings.MaxPheromoneAmount)
                        ? settings.MaxPheromoneAmount
                        : edgeNewAmount;
            }

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
    }
}
