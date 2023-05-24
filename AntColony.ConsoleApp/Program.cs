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

            var modelName = "a280.tsp";

            var model = PreparedModelLoader.GetModel(PreparedModelsEnum.eil51);
            //var model = TSPModelGenerator.GetNewModel(
            //    nodeCount: 100,
            //    xRange: (0, 100),
            //    yRange: (0, 100));

            var settings = new AntColonySettings()
            {
                UseCommonAntPheromoneAmount = true,
                CommonAntPheromoneAmount = 100,

                UseCommonEvaporation = true,
                EvaporationCoefficient = 0.8,

                //UseCommonWeights = true,
                DistanceWeight = 1,
                PheromoneWeight = 1,

                UseSymmetricDistances = true,
            };

            var algo = new ClassicAlgorithm<TSPNode>(model.Nodes, model.GetSectionDistance, settings);

            var timer = Stopwatch.StartNew();

            IList<IList<TSPNode>> paths = null;

            for (int i = 0; i <= 100; i++)
                paths = algo.Run(50);

            timer.Stop();

            Console.WriteLine($"{"Time elapsed"} | {"Minimum"} | {"Maximum"} | {"Average"}");
            Console.WriteLine($"{timer.Elapsed} | {Math.Round(paths.Min(x => model.GetDistance(x)), 2)} | {Math.Round(paths.Max(x => model.GetDistance(x)), 2)} | {Math.Round(paths.Average(x => model.GetDistance(x)), 2)} ");
            Console.ReadKey();
        }
    }
}