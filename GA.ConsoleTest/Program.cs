using Algorithms.Utility.NumberWrapper;
using GA.Core.Utility;
using GA.Experiments;
using GA.Experiments.Writer;
using GA.Operations;
using System;
using System.Globalization;
using System.Linq;
using TSP.Examples;

namespace GA.ConsoleTest
{
	class Program
	{

		//посмотреть, можно ли где заюзать Interlocked
		static void Main(string[] args)
		{
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
			CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

			var model = PreparedModelLoader.GetModel(PreparedModelsEnum.ch150);
			var solution = PreparedModelLoader.GetSolution(model, PreparedModelsEnum.ch150);

			var settings = new GASettings()
			{
				ElitePercent = 100D,
				MutationProbability = 0,
				OnlyChildrenInNewGeneration = false,
				GenerationsMaxCount = 500,
				StagnatingGenerationsLimit = 200,

				CrossoverType = CrossoversEnum.ParallelInverOver,
				MutationsType = MutationsEnum.ParallelShift,
				SelectionType = SelectionsEnum.RouletteWheel,

				CrossoverSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryGeneration, NodesCount = model.Nodes.Count },
				SelectionSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryGeneration, NodesCount = model.Nodes.Count },
				MutationSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryGeneration, NodesCount = model.Nodes.Count },
			};

			var crossovers = Enum.GetValues<CrossoversEnum>()
				.OrderBy(x => (int)x)
				.Select(x => (int)x);

			var experimentSettings = new GAExperimentSettings<double>()
			{
				PopulationSize = 200,
				ResearchedParameterName = nameof(settings.MutationProbability),
				ResearchedParameterIncrement = 10,
				ResearchedParameterRange = (new NumberDouble(0), new NumberDouble(100)),
				UseSameInitialPopulation = true,

				ControlRepeatingCount = 3
			};

			var results = ExperimentsEngine.Run(
				model.Nodes,
				settings,
				experimentSettings,
				(x) => 1 / model.GetDistance(x),
				(x) => model.GetDistance(x),
				WritersEnum.CSV | WritersEnum.JSON | WritersEnum.Console);
		}
	}
}
