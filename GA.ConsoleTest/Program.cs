using GA.Core;
using GA.Core.Extensions;
using GA.Core.Helpers;
using GA.Operations.Selections;
using System;
using System.Globalization;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using TSP.Core;
using GA.Operations.Crossovers;
using GA.Operations.Mutations;
using GA.Core.Models;
using GA.Analytics;
using GA.Core.Utility;

namespace GA.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

            var modelName = "eil51.tsp";
            var populationSize = 2000;
            var generationsAmount = 5000;
            var mutationProbability = 5D;
            double? eliteCoefficient = null;
            var mutationSwapSectionLength = 2;

            var model = TSPModelLoader.GetModelFromFile(modelName);
            var rand = new Random();

            var mutation = new SwapMutation() { SwapSectionLength = mutationSwapSectionLength };

            

            IList<Individual<TSPNode>> population = new List<Individual<TSPNode>>(populationSize);

            var i = 0;


            for (i = 0; i < populationSize; i++)
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
                new SinglePointCrossover(),
                new SwapMutation() { SwapSectionLength = mutationSwapSectionLength },
                population,
                (x) => 1 / model.GetDistance(x));


            for (i = 0; i < generationsAmount; i++)
                population = algo.GetNextGeneration(settings);

            timer.Stop();

            Console.WriteLine($"{"Time elapsed"} | {"Minimum"} | {"Maximum"} | {"Average"} | {"Degeneration coef."}");
            Console.WriteLine($"{timer.Elapsed} | {Math.Round(population.Min(x => model.GetDistance(x)), 2)} | {Math.Round(population.Max(x => model.GetDistance(x)), 2)} | {Math.Round(population.Average(x => model.GetDistance(x)), 2)} | {Math.Round(population.GetDegenerationCoefficient() * 100, 2)}%");
            Console.ReadKey();
        }
    }
}
