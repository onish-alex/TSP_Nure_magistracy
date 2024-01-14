using System.Collections.Generic;
using System.Linq;
using TSP.Core;

namespace SOM.TSPCompatibility
{
	public static class SOMMapper
	{
		public static IVector<double> Map(TSPNode node)
		{
			var parameters = new List<double>
			{
				node.X,
				node.Y
			};

			return new Vector(parameters);
		}

		public static TSPNode Map(TSPModel model, IVector<double> vector)
		{
			return model.Nodes.FirstOrDefault(node => node.X == vector["X"] && node.Y == vector["Y"]);
		}
	}
}
