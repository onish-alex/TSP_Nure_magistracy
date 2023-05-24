using System;
using System.Collections.Generic;

namespace TSP.Core
{
    public static class TSPModelGenerator
    {
        private static readonly Random random = new Random();

        public static TSPModel GetNewModel(int nodeCount, (int Min, int Max) xRange, (int Min, int Max) yRange)
        {
            var nodes = new List<TSPNode>(nodeCount);

            for (var i = 0; i < nodeCount; i++)
            {
                nodes.Add(new TSPNode()
                {
                    Name = i.ToString(),
                    X = random.Next(xRange.Min, xRange.Max),
                    Y = random.Next(yRange.Min, yRange.Max),
                });
            }

            return new TSPModel(nodes);
        }
    }
}
