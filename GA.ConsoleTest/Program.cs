using GA.Core.Utility;
using System;
using System.Globalization;
using System.Linq;
using TSP.Examples;
using Algorithms.Utility.NumberWrapper;
using GA.ConsoleApp.Experiments;
using GA.ConsoleApp.Experiments.Writer;
using System.Collections.Generic;

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
                GenerationsMaxCount = 300,
                StagnatingGenerationsLimit = 200,
            };

            var crossovers = Enum.GetValues<CrossoversEnum>()
                .OrderBy(x => (int)x)
                .Select(x => (int)x);

            var experimentSettings = new GAExperimentSettings<double>()
            {
                PopulationSize = 500,
                ResearchedParameterName = nameof(settings.MutationProbability),
                ResearchedParameterIncrement = 20,
                ResearchedParameterRange = (new NumberDouble(0), new NumberDouble(100)),
                UseSameInitialPopulation = true,

                CrossoverSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryGeneration, NodesCount = model.Nodes.Count },
                SelectionSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryGeneration, NodesCount = model.Nodes.Count },
                MutationSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryGeneration, NodesCount = model.Nodes.Count },

                CrossoverType = CrossoversEnum.PartiallyMapped,
                MutationsType = MutationsEnum.Shift,
                SelectionType = SelectionsEnum.RouletteWheel,

                ControlRepeatingCount = 3
            };

            using (var writer = new CSVWriter<double>("2.csv", ",", settings, experimentSettings))
            {
                var results = ExperimentsHelper.Run(
                    model.Nodes,
                    settings,
                    experimentSettings,
                    (x) => 1 / model.GetDistance(x),
                    (x) => model.GetDistance(x),
                    WritersEnum.CSV | WritersEnum.JSON);
            }

            //Console.WriteLine($"| {"Group", 10} | {experimentSettings.ResearchedParameterName(settings),20} | {"Time elapsed",19} | {"Minimum",9} | {"Maximum",9} | {"Average",9} | {"Iterations",12} | {"Degeneration coef.",17}");

            //foreach (var result in results.OrderBy(x => x.ResearchedParameterValue).ThenBy(x => x.GroupGuid).ThenBy(x => x.IsGroupResult))
            //{
            //    Console.WriteLine(
            //        $"| {result.IsGroupResult, 10} " +
            //        $"| {result.ResearchedParameterValue,20} " +
            //        $"| {result.Time,19} " +
            //        $"| {Math.Round(result.MinResult, 2),9} " +
            //        $"| {Math.Round(result.MaxResult, 2),9} " +
            //        $"| {Math.Round(result.AverageResult, 2),9} " +
            //        $"| {result.LastIterationNumber,12} " +
            //        $"| {Math.Round(result.DegenerationCoefficient, 2).ToString() + "%",17}");
            //}

            //=====================================

            //Console.WriteLine(Environment.ProcessorCount);

            //CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            //CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

            //var populationSize = 1000;
            ////var generationsAmount = 1000;
            //var mutationProbability = 100D;
            //double? eliteCoefficient = null;

            //var model = PreparedModelLoader.GetModel(PreparedModelsEnum.ch150);
            //var solution = PreparedModelLoader.GetSolution(model, PreparedModelsEnum.ch150);

            //Console.WriteLine(model.GetDistance(solution, true));

            //IList<Individual<TSPNode>> population = new List<Individual<TSPNode>>(populationSize);

            //for (var i = 0; i < populationSize; i++)
            //{
            //    var nodes = Random.Shared.GetUniqueRandomSet(model.Nodes, model.Nodes.Count);
            //    population.Add(new Individual<TSPNode>(nodes));
            //}

            //var timer = Stopwatch.StartNew();

            //var settings = new GASettings()
            //{
            //    ElitePercent = eliteCoefficient,
            //    MutationProbability = mutationProbability,
            //    OnlyChildrenInNewGeneration = false,
            //     GenerationsMaxCount = 500,
            //     StagnatingGenerationsLimit = 5,
            //};

            //var algo = new GeneticAlgorithm<TSPNode>(
            //    //new RouletteWheelSelection(new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count }),
            //    //new RouletteWheelSelection(new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count }),
            //    new TournamentSelection(new GAOperationSettings() { InitType = GAOperationInitType.Manual, NodesCount = model.Nodes.Count }) { },
            //    //{ ParentSelecting = ParentSelectingPrinciple.Outbreeding, IndividualCanJoinOnlyOnePair = false },

            //    //new PartiallyMappedCrossover(new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count }),
            //    //new ParallelPartiallyMappedCrossover(new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count }),
            //    new SinglePointOrderedCrossover(new GAOperationSettings() { InitType = GAOperationInitType.EveryGeneration, NodesCount = model.Nodes.Count }),
            //    //new ParallelSinglePointOrderedCrossover(new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count }),
            //    //new BitMaskCrossover(new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count }),
            //    //new TwoPointOrderedCrossover(new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count }),
            //    //new ParallelTwoPointOrderedCrossover(new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count }),
            //    //new CyclicCrossover(new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count }),
            //    //new ParallelCyclicCrossover(new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count }),

            //    //new ShiftMutation(new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count }),
            //    //new ParallelShiftMutation(new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count }),
            //    //new SwapMutation(new GAOperationSettings() { InitType = GAOperationInitType.Manual, NodesCount = model.Nodes.Count }) { SwapSectionLength = 2 },
            //    //new ParallelSwapMutation(new GAOperationSettings() { InitType = GAOperationInitType.Manual, NodesCount = model.Nodes.Count }) { SwapSectionLength = 2},
            //    new InverseMutation(new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count }),
            //    //new ParallelInverseMutation(new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count }),
            //    population,
            //    //,
            //    //() => 
            //    //{ 
            //    //    var nodes = Random.Shared.GetUniqueRandomSet(model.Nodes, model.Nodes.Count); 
            //    //    return new Individual<TSPNode>(nodes); 
            //    //}
            //    settings, (x) => 1 / model.GetDistance(x));

            //algo.Run();
            //population = algo.Population;
            ////for (var i = 0; i < generationsAmount; i++)
            ////    population = algo.GetNextGeneration();

            //timer.Stop();

            ////Console.WriteLine($"{"Time elapsed",19} | {"Minimum"} | {"Maximum"} | {"Average"} | {"Degeneration coef."}");
            ////Console.WriteLine($"{timer.Elapsed,19} " +
            ////    $",{Math.Round(population.Min(x => model.GetDistance(x)), 2),7} " +
            ////    $",{Math.Round(population.Max(x => model.GetDistance(x)), 2),7} " +
            ////    $",{Math.Round(population.Average(x => model.GetDistance(x)), 2),7} " +
            ////    $",{Math.Round(population.GetDegenerationCoefficient() * 100, 2).ToString() + "%",17}");
            //Console.WriteLine($"{"Time elapsed",19} | {"Minimum"} | {"Maximum"} | {"Average"} | {"Degeneration coef."}");
            //Console.WriteLine($"{timer.Elapsed} " +
            //    $"{Math.Round(population.Min(x => model.GetDistance(x)), 2)} " +
            //    $"{Math.Round(population.Max(x => model.GetDistance(x)), 2)} " +
            //    $"{Math.Round(population.Average(x => model.GetDistance(x)), 2)} " +
            //    $"{Math.Round(population.GetDegenerationCoefficient() * 100, 2).ToString() + "%"}");
            ////Console.ReadKey();

        }
    }
}
