using System.Collections.Generic;
using TSP.Core;

namespace SOM.TSPCompatibility
{
    public static class SOMMapper
    {
        public static Vector Map(TSPNode node)
        {
            var parameters = new List<double>
            {
                node.X,
                node.Y
            };

            return new Vector(parameters);
        }
    }
}
