using AntColony.Core.Utilities;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColony.Core
{
    public class ClassicAlgorithm<TNode> : BaseAlgorithm<TNode> where TNode : class
    {
        public ClassicAlgorithm(IList<TNode> nodes, Func<TNode, double> edgeDistanceGetter) : base(nodes, edgeDistanceGetter) { }

        public override IList<IList<TNode>> Run(int antCount)
        {
            var ants = new List<Ant<TNode>>();

            for (int i = 0; i < antCount; i++)
            {
                ants.Add(new Ant<TNode>());
            }

            return null;
                //////
        }

        protected override double GetTravelProbability(TNode currentNode, TNode unvisitedNode)
        {
            throw new NotImplementedException();
        }

        private protected override void TravelPath(Ant<TNode> ant)
        {
            var nodes = this.nodes.ToArray();
            ant.TravelledPathMemory.Add(nodes[0]);

            while (ant.TravelledPathMemory.Count != nodes.Length)
            {
                foreach(var node in nodes.Except(ant.TravelledPathMemory)) 
                {
                    var travelProbability = GetTravelProbability();
                    /////
                }
            }
        }
    }
}
