using Algorithms.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AntColony.Core.Utilities
{
    internal class Ant<TNode>
    {
        private static readonly Random random = new Random();

        public IList<TNode> TravelledPathMemory;
        public double PersonalPheromoneAmount;

        public Ant()
        {
            TravelledPathMemory = new List<TNode>();
        }
    }
}
