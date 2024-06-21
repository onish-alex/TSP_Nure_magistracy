using Algorithms.Utility.NumberWrapper;
using SOM.Experiments;
using SOM.Experiments.Writer;
using System.Globalization;
using TSP.Core;
using TSP.Examples;

namespace SOM.ConsoleApp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
			CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

			var model = PreparedModelLoader.GetModel(PreparedModelsEnum.tsp225);

			var settings = new Configuration.SOMSettings()
			{
				LearningCoefficient = 0.35D,                                            //+-
				UseDistancePenalties = true,                                            //-
				PenaltiesIncreasingCoefficient = 18D,                                  //+
				RoundPrecision = 1D,                                                    //-
				LearningFadingCoefficient = 0.0001D,                                    //+-
				UseElasticity = true,                                                   //-
				CooperationCoefficient = model.Nodes.Count * model.Nodes.Count,         //+-
				CooperationFading = 0.1D,                                               //+
				CooperationThreshold = 1D,                                              //-
				NetworkRadiusPercent = 30D,                                             //-
				NetworkSizeMultiplier = 3                                               //+
			};

			var expSettings = new SOMExperimentSettings<double>()
			{
				ControlRepeatingCount = 1,
				ResearchedParameterName = nameof(settings.LearningCoefficient),
				ResearchedParameterRange = (new NumberDouble(1D), new NumberDouble(1D)),
				ResearchedParameterIncrement = 0.05D,
			};

			SOMExperimentsEngine<TSPNode>.Run(
				model.Nodes,
				settings,
				expSettings,
				x => model.GetDistance(x),
				x => model.ParseNodes(x),
				WritersEnum.CSV | WritersEnum.JSON | WritersEnum.Console);

			//         var som = new TwoDimensionalSOM<TSPNode>(
			//	new Configuration.SOMSettings()
			//	{
			//		LearningCoefficient = 0.35D,											//+-
			//		UseDistancePenalties = true,											//-
			//		PenaltiesIncreasingCoefficient = 0.5D,									//+
			//		RoundPrecision = 1D,													//-
			//		LearningFadingCoefficient = 0.0001D,									//+-
			//		UseElasticity = true,													//-
			//		CooperationCoefficient = model.Nodes.Count * model.Nodes.Count,			//+-
			//		CooperationFading = 0.1D,												//+
			//		CooperationThreshold = 1D,												//-
			//		NetworkRadiusPercent = 30D,												//-
			//		NetworkSizeMultiplier = 3												//+
			//	},
			//	model.Nodes,
			//	Configuration.Topology.Sphere);


			//var network = som.BuildMap();
			//var nodes = model.ParseNodes(network.ToList());
			//Console.WriteLine(model.GetDistance(nodes.ToList()));

			//Console.WriteLine("Route1:" + string.Join(',', nodes.Select(x => x.Name)));
			//Console.WriteLine(string.Join("\r\n", nodes.Select(x => $"{x.X:F2};{x.Y:F2}")));

			//var route2 = new List<TSPNode>()
			//{
			//	nodes.Single(x => x.Name == "1"),
			//	nodes.Single(x => x.Name == "14"),
			//	nodes.Single(x => x.Name == "13"),
			//	nodes.Single(x => x.Name == "12"),
			//	nodes.Single(x => x.Name == "7"),
			//	nodes.Single(x => x.Name == "6"),
			//	nodes.Single(x => x.Name == "15"),
			//	nodes.Single(x => x.Name == "5"),
			//	nodes.Single(x => x.Name == "11"),
			//	nodes.Single(x => x.Name == "9"),
			//	nodes.Single(x => x.Name == "10"),
			//	nodes.Single(x => x.Name == "16"),
			//	nodes.Single(x => x.Name == "3"),
			//	nodes.Single(x => x.Name == "2"),
			//	nodes.Single(x => x.Name == "4"),
			//	nodes.Single(x => x.Name == "8"),
			//};


			//Console.WriteLine("Route2:" + model.GetDistance(route2.ToList()));
		}
	}
}