using AntColony.Core;
using AntColony.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using TSP.Core;
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

            var model = PreparedModelLoader.GetModel(PreparedModelsEnum.pr2392);
            var solution = PreparedModelLoader.GetSolution(model, PreparedModelsEnum.pr2392);
            //var model = TSPModelGenerator.GetNewModel(
            //    nodeCount: 100,
            //    xRange: (0, 100),
            //    yRange: (0, 100));

            Console.WriteLine("Optimal route length: {0}", model.GetDistance(solution, true));

            var settings = new AntColonySettings()
            {
                UseCommonAntPheromoneAmount = true,
                CommonAntPheromoneAmount = 25,

                UseCommonEliteAntPheromoneAmount = true,
                CommonEliteAntPheromoneAmount = 1000,

                EvaporationCoefficient = 0.25,

                DistanceWeight = 1,
                PheromoneWeight = 1,
            };

            var antSettings = new AntPopulationSettings()
            {
                AntCount = 250,
                EliteAntCount = 10
            };

            //var algo = new ClassicAlgorithm<TSPNode>(model.Nodes, model.GetSectionDistance, settings);
            var algo = new ElitistAlgorithm<TSPNode>(model.Nodes, model.GetSectionDistance, settings, (x) => model.GetDistance(x, true));

            var timer = Stopwatch.StartNew();

            IList<IList<TSPNode>> paths = null;

            for (int i = 0; i <= 50; i++)
                paths = algo.Run(antSettings);

            timer.Stop();

            Console.WriteLine($"{"Time elapsed"} | {"Minimum"} | {"Maximum"} | {"Average"}");
            Console.WriteLine($"{timer.Elapsed} | {Math.Round(paths.Min(x => model.GetDistance(x)), 2)} | {Math.Round(paths.Max(x => model.GetDistance(x)), 2)} | {Math.Round(paths.Average(x => model.GetDistance(x)), 2)} ");
            Console.ReadKey();
        }
    }
}