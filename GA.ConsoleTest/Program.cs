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

            var populationSize = 2000;
            var generationsAmount = 1000;
            var mutationProbability = 50D;
            double? eliteCoefficient = null;
            var mutationSwapSectionLength = 2;

            var model = PreparedModelLoader.GetModel(PreparedModelsEnum.ch150);
            var solution = PreparedModelLoader.GetSolution(model, PreparedModelsEnum.ch150);

            Console.WriteLine(model.GetDistance(solution));

            var rand = new Random();

            var mutation = new SwapMutation() { SwapSectionLength = mutationSwapSectionLength };

            IList<Individual<TSPNode>> population = new List<Individual<TSPNode>>(populationSize);

            for (var i = 0; i < populationSize; i++)
            {
                var nodes = rand.GetUniqueRandomSet(model.Nodes, model.Nodes.Count);
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
                new RouletteWheelSelection(),
                new SinglePointCrossover() { IsRandomPoint = true },
                new ShiftMutation() { UsePermanentParams = false },
                //new SwapMutation() { SwapSectionLength = 2},
                population,
                (x) => 1 / model.GetDistance(x));


            for (var i = 0; i < generationsAmount; i++)
                population = algo.GetNextGeneration(settings);

            timer.Stop();

            Console.WriteLine($"{"Time elapsed", 19} | {"Minimum"} | {"Maximum"} | {"Average"} | {"Degeneration coef."}");
            Console.WriteLine($"{timer.Elapsed, 19} | {Math.Round(population.Min(x => model.GetDistance(x)), 2), 7} | {Math.Round(population.Max(x => model.GetDistance(x)), 2), 7} | {Math.Round(population.Average(x => model.GetDistance(x)), 2) ,7} | {Math.Round(population.GetDegenerationCoefficient() * 100, 2).ToString() + "%", 17}");
            //Console.ReadKey();
        }
    }
}
