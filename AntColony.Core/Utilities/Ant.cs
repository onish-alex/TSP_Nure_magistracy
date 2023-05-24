using System.Collections.Generic;

namespace AntColony.Core.Utilities
{
    internal class Ant<TNode>
    {
        public IList<TNode> TravelledPathMemory;
        public double PersonalPheromoneAmount;

        public Ant()
        {
            TravelledPathMemory = new List<TNode>();
        }
    }
}
