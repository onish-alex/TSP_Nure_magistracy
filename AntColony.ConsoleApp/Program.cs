using Algorithms.Utility.NumberWrapper;
using AntColony.Core;
using AntColony.Core.Utilities;
using AntColony.Experiments;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using TSP.Core;
using TSP.Examples;

namespace AntColony.ConsoleApp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
			CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

			//var modelName = "a280.tsp";

			var model = PreparedModelLoader.GetModel(PreparedModelsEnum.ch150);
			var solution = PreparedModelLoader.GetSolution(model, PreparedModelsEnum.ch150);
			//var model = TSPModelGenerator.GetNewModel(
			//    nodeCount: 100,
			//    xRange: (0, 100),
			//    yRange: (0, 100));

			Console.WriteLine("Optimal route length: {0}", model.GetDistance(solution, true));

			var settings = new ACExperimentSettings<double>()
			{
				AntColonyType = AntColonyEnum.Classic,
				
				ControlRepeatingCount = 3,
				
				ResearchedParameterRange = (new NumberDouble(0.1), new NumberDouble(0.9)),
				ResearchedParameterIncrement = 0.1,
				
				UseCommonAntPheromoneAmount = true,
				CommonAntPheromoneAmount = 100,

				//UseCommonEliteAntPheromoneAmount = true,
				//CommonEliteAntPheromoneAmount = 1000,

				//EvaporationCoefficient = 0.1,
				//TODO make pheromone refreshing after result not changing after few iterations
				DistanceWeight = 1,
				PheromoneWeight = 1.5,

				//MinPheromoneAmount = 100,
				//MaxPheromoneAmount = 10000,

				InitialPheromoneAmount = 0,
				//UpdatePheromonesForGlobalBestWay = false,

				AntCount = 500,
				//EliteAntCount = 10
			};

			settings.ResearchedParameterName = nameof(settings.EvaporationCoefficient);

			ACExperimentsEngine.Run(
				model.Nodes,
				settings,
				model.GetSectionDistance,
				x => model.GetDistance(x, true),
				Experiments.Writer.WritersEnum.Console | Experiments.Writer.WritersEnum.JSON | Experiments.Writer.WritersEnum.CSV);
			
		}
	}
}