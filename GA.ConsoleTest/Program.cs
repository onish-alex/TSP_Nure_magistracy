using Algorithms.Utility.NumberWrapper;
using GA.Core.Utility;
using GA.Experiments;
using GA.Experiments.Writer;
using GA.Operations;
using System;
using System.Diagnostics;
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
				MutationProbability = 10,
				OnlyChildrenInNewGeneration = false,
				GenerationsMaxCount = 5000,
				//StagnatingGenerationsLimit = 200,
				DegenerationMaxPercent = 90,

				//CrossoverType = CrossoversEnum.ParallelInverOver,
				MutationsType = MutationsEnum.ParallelShift,
				SelectionType = SelectionsEnum.RouletteWheel,

				CrossoverSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count },
				SelectionSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryGeneration, NodesCount = model.Nodes.Count },
				MutationSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryGeneration, NodesCount = model.Nodes.Count },
			};

			var crossovers = Enum.GetValues<CrossoversEnum>()
				.OrderBy(x => (int)x)
				.Select(x => (int)x);

			var experimentSettings = new GAExperimentSettings<int>()
			{
				PopulationSize = 3000,
				ResearchedParameterName = nameof(settings.CrossoverType),
				ResearchedParameterIncrement = 1,
				ResearchedParameterRange = (new NumberInt((int)CrossoversEnum.OrderBased), new NumberInt((int)CrossoversEnum.InverOver)),
				UseSameInitialPopulation = true,

				ControlRepeatingCount = 3
			};

			var results = GAExperimentsEngine.Run(
				model.Nodes,
				settings,
				experimentSettings,
				(x) => model.GetDistance(x),
				(x) => model.GetDistance(x),
				WritersEnum.CSV | WritersEnum.JSON | WritersEnum.Console);

			var psi = new ProcessStartInfo("shutdown", "/s /t 0");
			psi.CreateNoWindow = true;
			psi.UseShellExecute = false;
			Process.Start(psi);
		}
	}
}
