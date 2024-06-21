using Algorithms.Utility.NumberWrapper;
using AntColony.Core.Utilities;
using AntColony.Experiments;
using System;
using System.Globalization;
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

			var model = PreparedModelLoader.GetModel(PreparedModelsEnum.tsp225);
			var solution = PreparedModelLoader.GetSolution(model, PreparedModelsEnum.tsp225);

			Console.WriteLine("Optimal route length: {0}", model.GetDistance(solution, true));

			var settings = new ACExperimentSettings<int>()
			{
				AntColonyType = AntColonyEnum.MinMax,

				ControlRepeatingCount = 3,

				ResearchedParameterRange = (new NumberInt(35), new NumberInt(100)),
				ResearchedParameterIncrement = 5,

				UseCommonAntPheromoneAmount = true,
				CommonAntPheromoneAmount = 25000,

				//UseCommonEliteAntPheromoneAmount = true,
				//CommonEliteAntPheromoneAmount = 225,

				EvaporationCoefficient = 0.05,
				//TODO make pheromone refreshing after result not changing after few iterations
				PheromoneWeight = 2.5,
				DistanceWeight = 5,

				MinPheromoneAmount = 50,
				MaxPheromoneAmount = 1000,

				InitialPheromoneAmount = 1000,
				UpdatePheromonesForGlobalBestWay = false,

				AntCount = 35,
				//EliteAntCount = 5,
				IterationsCount = 35,
			};

			settings.ResearchedParameterName = nameof(settings.IterationsCount);

			ACExperimentsEngine.Run(
				model.Nodes,
				settings,
				model.GetSectionDistance,
				x => model.GetDistance(x, true),
				Experiments.Writer.WritersEnum.Console | Experiments.Writer.WritersEnum.JSON | Experiments.Writer.WritersEnum.CSV);

			//settings = new ACExperimentSettings<int>()
			//{
			//    AntColonyType = AntColonyEnum.Classic,

			//    ControlRepeatingCount = 3,

			//    ResearchedParameterRange = (new NumberInt(5), new NumberInt(50)),
			//    ResearchedParameterIncrement = 5,

			//    UseCommonAntPheromoneAmount = true,
			//    CommonAntPheromoneAmount = 100,

			//    //UseCommonEliteAntPheromoneAmount = true,
			//    //CommonEliteAntPheromoneAmount = 1000,

			//    EvaporationCoefficient = 0.5,
			//    //TODO make pheromone refreshing after result not changing after few iterations
			//    PheromoneWeight = 2.5,
			//    DistanceWeight = 5,

			//    //MinPheromoneAmount = 100,
			//    //MaxPheromoneAmount = 10000,

			//    InitialPheromoneAmount = 0,
			//    //UpdatePheromonesForGlobalBestWay = false,

			//    //AntCount = 25,
			//    //EliteAntCount = 10
			//    IterationsCount = 25,
			//};

			//settings.ResearchedParameterName = nameof(settings.AntCount);

			//ACExperimentsEngine.Run(
			//    model.Nodes,
			//    settings,
			//    model.GetSectionDistance,
			//    x => model.GetDistance(x, true),
			//    Experiments.Writer.WritersEnum.Console | Experiments.Writer.WritersEnum.JSON | Experiments.Writer.WritersEnum.CSV);

			//var psi = new ProcessStartInfo("shutdown", "/s /t 0");
			//psi.CreateNoWindow = true;
			//psi.UseShellExecute = false;
			//Process.Start(psi);

			//settings = new ACExperimentSettings<double>()
			//{
			//    AntColonyType = AntColonyEnum.Classic,

			//    ControlRepeatingCount = 1,

			//    ResearchedParameterRange = (new NumberDouble(0.5), new NumberDouble(5)),
			//    ResearchedParameterIncrement = 0.5,

			//    UseCommonAntPheromoneAmount = true,
			//    CommonAntPheromoneAmount = 100,

			//    //UseCommonEliteAntPheromoneAmount = true,
			//    //CommonEliteAntPheromoneAmount = 1000,

			//    EvaporationCoefficient = 0.5,
			//    //TODO make pheromone refreshing after result not changing after few iterations
			//    //PheromoneWeight = 1,
			//    DistanceWeight = 4,

			//    //MinPheromoneAmount = 100,
			//    //MaxPheromoneAmount = 10000,

			//    InitialPheromoneAmount = 0,
			//    //UpdatePheromonesForGlobalBestWay = false,

			//    AntCount = 25,
			//    //EliteAntCount = 10
			//    IterationsCount = 25,
			//};

			//settings.ResearchedParameterName = nameof(settings.PheromoneWeight);

			//ACExperimentsEngine.Run(
			//    model.Nodes,
			//    settings,
			//    model.GetSectionDistance,
			//    x => model.GetDistance(x, true),
			//    Experiments.Writer.WritersEnum.Console | Experiments.Writer.WritersEnum.JSON | Experiments.Writer.WritersEnum.CSV);

			//settings = new ACExperimentSettings<double>()
			//{
			//    AntColonyType = AntColonyEnum.Classic,

			//    ControlRepeatingCount = 1,

			//    ResearchedParameterRange = (new NumberDouble(0.5), new NumberDouble(5)),
			//    ResearchedParameterIncrement = 0.5,

			//    UseCommonAntPheromoneAmount = true,
			//    CommonAntPheromoneAmount = 100,

			//    //UseCommonEliteAntPheromoneAmount = true,
			//    //CommonEliteAntPheromoneAmount = 1000,

			//    EvaporationCoefficient = 0.5,
			//    //TODO make pheromone refreshing after result not changing after few iterations
			//    //PheromoneWeight = 1,
			//    DistanceWeight = 4.5,

			//    //MinPheromoneAmount = 100,
			//    //MaxPheromoneAmount = 10000,

			//    InitialPheromoneAmount = 0,
			//    //UpdatePheromonesForGlobalBestWay = false,

			//    AntCount = 25,
			//    //EliteAntCount = 10
			//    IterationsCount = 25,
			//};

			//settings.ResearchedParameterName = nameof(settings.PheromoneWeight);

			//ACExperimentsEngine.Run(
			//    model.Nodes,
			//    settings,
			//    model.GetSectionDistance,
			//    x => model.GetDistance(x, true),
			//    Experiments.Writer.WritersEnum.Console | Experiments.Writer.WritersEnum.JSON | Experiments.Writer.WritersEnum.CSV);

			//settings = new ACExperimentSettings<double>()
			//{
			//    AntColonyType = AntColonyEnum.Classic,

			//    ControlRepeatingCount = 1,

			//    ResearchedParameterRange = (new NumberDouble(0.5), new NumberDouble(5)),
			//    ResearchedParameterIncrement = 0.5,

			//    UseCommonAntPheromoneAmount = true,
			//    CommonAntPheromoneAmount = 100,

			//    //UseCommonEliteAntPheromoneAmount = true,
			//    //CommonEliteAntPheromoneAmount = 1000,

			//    EvaporationCoefficient = 0.5,
			//    //TODO make pheromone refreshing after result not changing after few iterations
			//    //PheromoneWeight = 1,
			//    DistanceWeight = 5,

			//    //MinPheromoneAmount = 100,
			//    //MaxPheromoneAmount = 10000,

			//    InitialPheromoneAmount = 0,
			//    //UpdatePheromonesForGlobalBestWay = false,

			//    AntCount = 25,
			//    //EliteAntCount = 10
			//    IterationsCount = 25,
			//};

			//settings.ResearchedParameterName = nameof(settings.PheromoneWeight);

			//ACExperimentsEngine.Run(
			//    model.Nodes,
			//    settings,
			//    model.GetSectionDistance,
			//    x => model.GetDistance(x, true),
			//    Experiments.Writer.WritersEnum.Console | Experiments.Writer.WritersEnum.JSON | Experiments.Writer.WritersEnum.CSV);
		}
	}
}