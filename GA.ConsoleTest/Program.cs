using Algorithms.Utility.Extensions;
using GA.Analytics;
using GA.Core;
using GA.Core.Models;
using GA.Core.Utility;
using GA.Operations.Crossovers;
using GA.Operations.Mutations;
using GA.Operations.Selections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using TSP.Core;
using TSP.Examples;

namespace GA.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {

            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

            var populationSize = 1000;
            var generationsAmount = 300;
            var mutationProbability = 5D;
            double? eliteCoefficient = null;

            var model = PreparedModelLoader.GetModel(PreparedModelsEnum.eil51);
            var solution = PreparedModelLoader.GetSolution(model, PreparedModelsEnum.eil51);

            Console.WriteLine(model.GetDistance(solution));

            IList<Individual<TSPNode>> population = new List<Individual<TSPNode>>(populationSize);

            for (var i = 0; i < populationSize; i++)
            {
                var nodes = Random.Shared.GetUniqueRandomSet(model.Nodes, model.Nodes.Count);
                population.Add(new Individual<TSPNode>(nodes));
            }

            var timer = Stopwatch.StartNew();

            var settings = new GASettings()
            {
                ElitePercent = eliteCoefficient,
                MutationProbability = mutationProbability,
                OnlyChildrenInNewGeneration = false
            };

            var algo = new GeneticAlgorithm<TSPNode>(
                new RouletteWheelSelection(new GAOperationSettings() { InitType = GAOperationInitType.OneTime, NodesCount = model.Nodes.Count }),
                //new PartiallyMappedCrossover(new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count }),
                new SinglePointCrossover(new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count }),
                new ShiftMutation(new GAOperationSettings() { InitType = GAOperationInitType.EveryIndividual, NodesCount = model.Nodes.Count }),
                //new SwapMutation(new GAOperationSettings() { InitType = GAOperationInitType.Manual, NodesCount = model.Nodes.Count }) { SwapSectionLength = 2},
                population,
                (x) => 1 / model.GetDistance(x));


            for (var i = 0; i < generationsAmount; i++)
                population = algo.GetNextGeneration(settings);

            timer.Stop();

            Console.WriteLine($"{"Time elapsed", 19} | {"Minimum"} | {"Maximum"} | {"Average"} | {"Degeneration coef."}");
            Console.WriteLine($"{timer.Elapsed, 19} " +
                $"| {Math.Round(population.Min(x => model.GetDistance(x)), 2), 7} " +
                $"| {Math.Round(population.Max(x => model.GetDistance(x)), 2), 7} " +
                $"| {Math.Round(population.Average(x => model.GetDistance(x)), 2) ,7} " +
                $"| {Math.Round(population.GetDegenerationCoefficient() * 100, 2).ToString() + "%", 17}");
            //Console.ReadKey();
        
        }
    }
}
