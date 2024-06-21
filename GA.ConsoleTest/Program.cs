using Algorithms.Utility.NumberWrapper;
using GA.Core.Utility;
using GA.Experiments;
using GA.Experiments.Writer;
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
			var model = PreparedModelLoader.GetModel(PreparedModelsEnum.pr1002);

			var solution = PreparedModelLoader.GetSolution(model, PreparedModelsEnum.pr1002);

			var crossovers = Enum.GetValues<CrossoversEnum>()
				.OrderBy(x => (int)x)
				.Select(x => (int)x);
            /////////////////////////////////////////////////////////Swap OneTime
            //var settings = new GASettings()
            //{
            //	//ElitePercent = null,
            //	//MutationProbability = 0,
            //	OnlyChildrenInNewGeneration = false,
            //	GenerationsMaxCount = 500,
            //	//StagnatingGenerationsLimit = 0,
            //	//DegenerationMaxPercent = 80,

            //	CrossoverType = CrossoversEnum.OrderBased,
            //	MutationsType = MutationsEnum.Swap,
            //	SelectionType = SelectionsEnum.RouletteWheel,

            //	CrossoverSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count },
            //	SelectionSettings = new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count },
            //	MutationSettings = new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count },
            //};

            //var experimentSettings = new GAExperimentSettings<double>()
            //{
            //	PopulationSize = 250,
            //	ResearchedParameterName = nameof(settings.MutationProbability),
            //	ResearchedParameterIncrement = 10,
            //	ResearchedParameterRange = (new NumberDouble(0), new NumberDouble(0)),
            //	UseSameInitialPopulation = true,

            //	ControlRepeatingCount = 10
            //};

            //loading tsp map from file



            //GA operations
            //var selection = GAExperimentsEngine.CreateSelection(settings.SelectionType, settings.SelectionSettings);
            //var crossover = GAExperimentsEngine.CreateCrossover(settings.CrossoverType, settings.CrossoverSettings);
            //var mutation = GAExperimentsEngine.CreateMutation(settings.MutationsType, settings.MutationSettings);

            //var populationSize = 200;
            //var population = GAExperimentsEngine.GeneratePopulation(model.Nodes, populationSize);

            //var algo = new GeneticAlgorithm<TSPNode>(
            //				selection,
            //				crossover,
            //				mutation,
            //				population.ToList(),
            //				settings,
            //				(x) => model.GetDistance(x));

            //algo.Run(); //full run till end
            //algo.GetNextGeneration(); //one iteration, more appropriate for updating the plot

            //GA settings
            var settings = new GASettings()
            {
                MutationProbability = 0,
                GenerationsMaxCount = 300000,
                //StagnatingGenerationsLimit = 0,
                //DegenerationMaxPercent = 80,

                CrossoverType = CrossoversEnum.InverOver,
                MutationsType = MutationsEnum.Swap,
                SelectionType = SelectionsEnum.RouletteWheel,

                CrossoverSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count },
                SelectionSettings = new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count },
                MutationSettings = new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count },
            };

            var experimentSettings = new GAExperimentSettings<double>()
			{
				PopulationSize = 100,
				ResearchedParameterName = nameof(settings.MutationProbability),
				ResearchedParameterStep = 5,
				ResearchedParameterRange = (new NumberDouble(0), new NumberDouble(0)),
				//ResearchedParameterName = nameof(settings.CrossoverType),
				//ResearchedParameterIncrement = 1,
				//ResearchedParameterRange = (new NumberInt((int)CrossoversEnum.OrderBased), new NumberInt((int)CrossoversEnum.InverOver)),
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
		}
	}
}
