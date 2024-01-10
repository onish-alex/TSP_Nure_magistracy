using Algorithms.Utility.Extensions;
using GA.Analytics;
using GA.Core;
using GA.Core.Models;
using GA.Core.Utility;
using GA.Operations.Crossovers;
using GA.Operations.Crossovers.Concurrent;
using GA.Operations.Mutations;
using GA.Operations.Mutations.Concurrent;
using GA.Operations.Selections;
using GA.Operations.Selections.Concurrent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Linq;
using TSP.Core;
using TSP.Examples;
using GA.ConsoleApp;
using Algorithms.Utility.NumberWrapper;

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
                StagnatingGenerationsLimit = 5,
            };

            var crossovers = Enum.GetValues<CrossoversEnum>()
                .OrderBy(x => (int)x)
                .Select(x => (int)x);

            var experimentSettings = new GAExperimentSettings<int>()
            {
                PopulationSize = 1000,
                ResearchedParameterName = (x) => nameof(x.CrossoverType),
                ResearchedParameterIncrement = new NumberInt(1),
                ResearchedParameterRange = (new NumberInt(crossovers.First()), new NumberInt(crossovers.Last())),
                UseSameInitialPopulation = true,

                CrossoverSettings = new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count },
                SelectionSettings = new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count },
                MutationSettings = new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count },
                
                //Crossover = new SinglePointOrderedCrossover(new GAOperationSettings() { InitType = GAOperationInitType.EveryGeneration, NodesCount = model.Nodes.Count }),
                //Selection = new TournamentSelection(new GAOperationSettings() { InitType = GAOperationInitType.Manual, NodesCount = model.Nodes.Count }),
                //Mutation = new InverseMutation(new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count }),
            };

            ExperimentsHelper.Run(model.Nodes, settings, experimentSettings, (x) => 1 / model.GetDistance(x), (x) => model.GetDistance(x));

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
