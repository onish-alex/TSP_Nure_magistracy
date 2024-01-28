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

			var model = PreparedModelLoader.GetModel(PreparedModelsEnum.pr1002);
			var solution = PreparedModelLoader.GetSolution(model, PreparedModelsEnum.pr1002);

			var crossovers = Enum.GetValues<CrossoversEnum>()
				.OrderBy(x => (int)x)
				.Select(x => (int)x);
			/////////////////////////////////////////////////////////Swap OneTime
			var settings = new GASettings()
			{
				//ElitePercent = null,
				//MutationProbability = 0,
				OnlyChildrenInNewGeneration = false,
				GenerationsMaxCount = 10000,
				StagnatingGenerationsLimit = 250,
				//DegenerationMaxPercent = 80,

				CrossoverType = CrossoversEnum.InverOver,
				MutationsType = MutationsEnum.Shift,
				SelectionType = SelectionsEnum.RouletteWheel,

				CrossoverSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count },
				SelectionSettings = new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count },
				MutationSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryGeneration, NodesCount = model.Nodes.Count },
			};

			var experimentSettings = new GAExperimentSettings<double>()
			{
				PopulationSize = 50,
				ResearchedParameterName = nameof(settings.MutationProbability),
				ResearchedParameterIncrement = 10,
				ResearchedParameterRange = (new NumberDouble(0), new NumberDouble(100)),
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

			///////////////////////////////////////////Swap EveryGeneration

			//settings = new GASettings()
			//{
			//	//ElitePercent = null,
			//	//MutationProbability = 0,
			//	OnlyChildrenInNewGeneration = false,
			//	GenerationsMaxCount = 10000,
			//	StagnatingGenerationsLimit = 250,
			//	//DegenerationMaxPercent = 80,

			//	CrossoverType = CrossoversEnum.InverOver,
			//	MutationsType = MutationsEnum.Shift,
			//	SelectionType = SelectionsEnum.RouletteWheel,

			//	CrossoverSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count },
			//	SelectionSettings = new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count },
			//	MutationSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryGeneration, NodesCount = model.Nodes.Count },
			//};

			//experimentSettings = new GAExperimentSettings<double>()
			//{
			//	PopulationSize = 50,
			//	ResearchedParameterName = nameof(settings.MutationProbability),
			//	ResearchedParameterIncrement = 10,
			//	ResearchedParameterRange = (new NumberDouble(0), new NumberDouble(100)),
			//	UseSameInitialPopulation = true,

			//	ControlRepeatingCount = 3
			//};

			//results = GAExperimentsEngine.Run(
			//	model.Nodes,
			//	settings,
			//	experimentSettings,
			//	(x) => model.GetDistance(x),
			//	(x) => model.GetDistance(x),
			//	WritersEnum.CSV | WritersEnum.JSON | WritersEnum.Console);
			/////////////////////////////////////////////Swap EveryIndividual

			//settings = new GASettings()
			//{
			//	//ElitePercent = null,
			//	//MutationProbability = 0,
			//	OnlyChildrenInNewGeneration = false,
			//	GenerationsMaxCount = 10000,
			//	StagnatingGenerationsLimit = 250,
			//	//DegenerationMaxPercent = 80,

			//	CrossoverType = CrossoversEnum.InverOver,
			//	MutationsType = MutationsEnum.Shift,
			//	SelectionType = SelectionsEnum.RouletteWheel,

			//	CrossoverSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count },
			//	SelectionSettings = new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count },
			//	MutationSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count },
			//};

			//experimentSettings = new GAExperimentSettings<double>()
			//{
			//	PopulationSize = 50,
			//	ResearchedParameterName = nameof(settings.MutationProbability),
			//	ResearchedParameterIncrement = 10,
			//	ResearchedParameterRange = (new NumberDouble(0), new NumberDouble(100)),
			//	UseSameInitialPopulation = true,

			//	ControlRepeatingCount = 3
			//};

			//results = GAExperimentsEngine.Run(
			//	model.Nodes,
			//	settings,
			//	experimentSettings,
			//	(x) => model.GetDistance(x),
			//	(x) => model.GetDistance(x),
			//	WritersEnum.CSV | WritersEnum.JSON | WritersEnum.Console);
			///////////////////////////////////////////Shift OneTime

			//settings = new GASettings()
			//{
			//	//ElitePercent = null,
			//	//MutationProbability = 0,
			//	OnlyChildrenInNewGeneration = false,
			//	GenerationsMaxCount = 5000,
			//	StagnatingGenerationsLimit = 250,
			//	//DegenerationMaxPercent = 80,

			//	CrossoverType = CrossoversEnum.InverOver,
			//	MutationsType = MutationsEnum.Shift,
			//	SelectionType = SelectionsEnum.RouletteWheel,

			//	CrossoverSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count },
			//	SelectionSettings = new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count },
			//	MutationSettings = new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count },
			//};

			//experimentSettings = new GAExperimentSettings<double>()
			//{
			//	PopulationSize = 250,
			//	ResearchedParameterName = nameof(settings.MutationProbability),
			//	ResearchedParameterIncrement = 5,
			//	ResearchedParameterRange = (new NumberDouble(0), new NumberDouble(100)),
			//	UseSameInitialPopulation = true,

			//	ControlRepeatingCount = 3
			//};

			//results = GAExperimentsEngine.Run(
			//	model.Nodes,
			//	settings,
			//	experimentSettings,
			//	(x) => model.GetDistance(x),
			//	(x) => model.GetDistance(x),
			//	WritersEnum.CSV | WritersEnum.JSON | WritersEnum.Console);

			/////////////////////////////////////////////Shift EveryGeneration

			//settings = new GASettings()
			//{
			//	//ElitePercent = null,
			//	//MutationProbability = 0,
			//	OnlyChildrenInNewGeneration = false,
			//	GenerationsMaxCount = 5000,
			//	StagnatingGenerationsLimit = 250,
			//	//DegenerationMaxPercent = 80,

			//	CrossoverType = CrossoversEnum.InverOver,
			//	MutationsType = MutationsEnum.Shift,
			//	SelectionType = SelectionsEnum.RouletteWheel,

			//	CrossoverSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count },
			//	SelectionSettings = new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count },
			//	MutationSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryGeneration, NodesCount = model.Nodes.Count },
			//};

			//experimentSettings = new GAExperimentSettings<double>()
			//{
			//	PopulationSize = 250,
			//	ResearchedParameterName = nameof(settings.MutationProbability),
			//	ResearchedParameterIncrement = 5,
			//	ResearchedParameterRange = (new NumberDouble(0), new NumberDouble(100)),
			//	UseSameInitialPopulation = true,

			//	ControlRepeatingCount = 3
			//};

			//results = GAExperimentsEngine.Run(
			//	model.Nodes,
			//	settings,
			//	experimentSettings,
			//	(x) => model.GetDistance(x),
			//	(x) => model.GetDistance(x),
			//	WritersEnum.CSV | WritersEnum.JSON | WritersEnum.Console);
			/////////////////////////////////////////////Shift EveryIndividual

			//settings = new GASettings()
			//{
			//	//ElitePercent = null,
			//	//MutationProbability = 0,
			//	OnlyChildrenInNewGeneration = false,
			//	GenerationsMaxCount = 5000,
			//	StagnatingGenerationsLimit = 250,
			//	//DegenerationMaxPercent = 80,

			//	CrossoverType = CrossoversEnum.InverOver,
			//	MutationsType = MutationsEnum.Shift,
			//	SelectionType = SelectionsEnum.RouletteWheel,

			//	CrossoverSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count },
			//	SelectionSettings = new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count },
			//	MutationSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count },
			//};

			//experimentSettings = new GAExperimentSettings<double>()
			//{
			//	PopulationSize = 250,
			//	ResearchedParameterName = nameof(settings.MutationProbability),
			//	ResearchedParameterIncrement = 5,
			//	ResearchedParameterRange = (new NumberDouble(0), new NumberDouble(100)),
			//	UseSameInitialPopulation = true,

			//	ControlRepeatingCount = 3
			//};

			//results = GAExperimentsEngine.Run(
			//	model.Nodes,
			//	settings,
			//	experimentSettings,
			//	(x) => model.GetDistance(x),
			//	(x) => model.GetDistance(x),
			//	WritersEnum.CSV | WritersEnum.JSON | WritersEnum.Console);
			/////////////////////////////////////////////Inverse OneTime

			//settings = new GASettings()
			//{
			//	//ElitePercent = null,
			//	//MutationProbability = 0,
			//	OnlyChildrenInNewGeneration = false,
			//	GenerationsMaxCount = 5000,
			//	StagnatingGenerationsLimit = 250,
			//	//DegenerationMaxPercent = 80,

			//	CrossoverType = CrossoversEnum.InverOver,
			//	MutationsType = MutationsEnum.Inverse,
			//	SelectionType = SelectionsEnum.RouletteWheel,

			//	CrossoverSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count },
			//	SelectionSettings = new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count },
			//	MutationSettings = new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count },
			//};

			//experimentSettings = new GAExperimentSettings<double>()
			//{
			//	PopulationSize = 250,
			//	ResearchedParameterName = nameof(settings.MutationProbability),
			//	ResearchedParameterIncrement = 5,
			//	ResearchedParameterRange = (new NumberDouble(0), new NumberDouble(100)),
			//	UseSameInitialPopulation = true,

			//	ControlRepeatingCount = 3
			//};

			//results = GAExperimentsEngine.Run(
			//	model.Nodes,
			//	settings,
			//	experimentSettings,
			//	(x) => model.GetDistance(x),
			//	(x) => model.GetDistance(x),
			//	WritersEnum.CSV | WritersEnum.JSON | WritersEnum.Console);

			/////////////////////////////////////////////Inverse EveryGeneration

			//settings = new GASettings()
			//{
			//	//ElitePercent = null,
			//	//MutationProbability = 0,
			//	OnlyChildrenInNewGeneration = false,
			//	GenerationsMaxCount = 5000,
			//	StagnatingGenerationsLimit = 250,
			//	//DegenerationMaxPercent = 80,

			//	CrossoverType = CrossoversEnum.InverOver,
			//	MutationsType = MutationsEnum.Inverse,
			//	SelectionType = SelectionsEnum.RouletteWheel,

			//	CrossoverSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count },
			//	SelectionSettings = new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count },
			//	MutationSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryGeneration, NodesCount = model.Nodes.Count },
			//};

			//experimentSettings = new GAExperimentSettings<double>()
			//{
			//	PopulationSize = 250,
			//	ResearchedParameterName = nameof(settings.MutationProbability),
			//	ResearchedParameterIncrement = 5,
			//	ResearchedParameterRange = (new NumberDouble(0), new NumberDouble(100)),
			//	UseSameInitialPopulation = true,

			//	ControlRepeatingCount = 3
			//};

			//results = GAExperimentsEngine.Run(
			//	model.Nodes,
			//	settings,
			//	experimentSettings,
			//	(x) => model.GetDistance(x),
			//	(x) => model.GetDistance(x),
			//	WritersEnum.CSV | WritersEnum.JSON | WritersEnum.Console);
			/////////////////////////////////////////////Inverse EveryIndividual

			//settings = new GASettings()
			//{
			//	//ElitePercent = null,
			//	//MutationProbability = 0,
			//	OnlyChildrenInNewGeneration = false,
			//	GenerationsMaxCount = 5000,
			//	StagnatingGenerationsLimit = 250,
			//	//DegenerationMaxPercent = 80,

			//	CrossoverType = CrossoversEnum.InverOver,
			//	MutationsType = MutationsEnum.Inverse,
			//	SelectionType = SelectionsEnum.RouletteWheel,

			//	CrossoverSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count },
			//	SelectionSettings = new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count },
			//	MutationSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count },
			//};

			//experimentSettings = new GAExperimentSettings<double>()
			//{
			//	PopulationSize = 250,
			//	ResearchedParameterName = nameof(settings.MutationProbability),
			//	ResearchedParameterIncrement = 5,
			//	ResearchedParameterRange = (new NumberDouble(0), new NumberDouble(100)),
			//	UseSameInitialPopulation = true,

			//	ControlRepeatingCount = 3
			//};

			//results = GAExperimentsEngine.Run(
			//	model.Nodes,
			//	settings,
			//	experimentSettings,
			//	(x) => model.GetDistance(x),
			//	(x) => model.GetDistance(x),
			//	WritersEnum.CSV | WritersEnum.JSON | WritersEnum.Console);

			//var psi = new ProcessStartInfo("shutdown", "/s /t 0");
			//psi.CreateNoWindow = true;
			//psi.UseShellExecute = false;
			//Process.Start(psi);
		}
	}
}
