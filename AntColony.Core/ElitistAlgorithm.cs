using AntColony.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColony.Core
{
    public class ElitistAlgorithm<TNode> : ClassicAlgorithm<TNode> where TNode : class
    {
        private Func<IList<TNode>, double> routeDistanceGetter;

        public ElitistAlgorithm(
            IList<TNode> nodes,
            Func<TNode, TNode, double> edgeDistanceGetter,
            AntColonySettings settings,
            Func<IList<TNode>, double> routeDistanceGetter) : base(nodes, edgeDistanceGetter, settings)
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

            var routes = ants.Select(x => x.TravelledPathMemory).ToList();

            var bestRoutes = routes.OrderBy(x => routeDistanceGetter(x)).Take(antSettings.EliteAntCount);

            foreach (var route in bestRoutes)
            {
                for (var i = 0; i < route.Count - 1; i++)
                {
                    var currentPheromoneAmount = pheromoneMap[route[i]][route[i + 1]];

                    var pheromoneDelta = (settings.UseCommonEliteAntPheromoneAmount 
                                     || (!settings.UseCommonEliteAntPheromoneAmount && antSettings.EliteAntPersonalPheromoneAmounts.Length <= i)) 
                        ? settings.CommonEliteAntPheromoneAmount 
                        : antSettings.EliteAntPersonalPheromoneAmounts[i];
                    pheromoneDelta /= edgeDistanceGetter(route[i], route[i + 1]);

                    pheromoneMap[route[i]][route[i + 1]] = currentPheromoneAmount + pheromoneDelta;
                }
            }

            return routes;
        }
    }
}
