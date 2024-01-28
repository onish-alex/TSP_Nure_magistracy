using System;
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
			var threshold = 0.01D;

			return model.Nodes.FirstOrDefault(node => 
			   Math.Abs(node.X - vector["X"]) <= threshold 
			&& Math.Abs(node.Y - vector["Y"]) <= threshold);
		}
	}
}
