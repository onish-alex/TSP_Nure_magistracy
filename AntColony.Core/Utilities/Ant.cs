using System.Collections.Generic;

namespace AntColony.Core.Utilities
{
    public class Ant<TNode>
    {
        public IList<TNode> TravelledPathMemory;
        public double PersonalPheromoneAmount;

        public Ant()
        {
            TravelledPathMemory = new List<TNode>();
        }
    }
}
