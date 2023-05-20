using AntColony.Core;
using AntColony.Core.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using TSP.Core;

namespace AntColony.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

            var modelName = "eil51.tsp";

            var model = TSPModelLoader.GetModelFromFile(modelName);

            var settings = new AntColonySettings()
            {
                UseCommonAntPheromoneAmount = true,
                CommonAntPheromoneAmount = 5,

                UseCommonEvaporation = true,
                EvaporationCoefficient = 0.4,

                UseCommonWeights = true,
                DistanceWeight = 0.5,
                PheromoneWeight = 0.5,

                UseSymmetricDistances = true,
            };

            var algo = new ClassicAlgorithm<TSPNode>(model.Nodes, model.GetSectionDistance, settings);

            var timer = Stopwatch.StartNew();

            IList<IList<TSPNode>> paths = null;

            for (int i = 0; i <= 250; i++)
                paths = algo.Run(250);

            timer.Stop();

            Console.WriteLine($"{"Time elapsed"} | {"Minimum"} | {"Maximum"} | {"Average"}");
            Console.WriteLine($"{timer.Elapsed} | {Math.Round(paths.Min(x => model.GetDistance(x)), 2)} | {Math.Round(paths.Max(x => model.GetDistance(x)), 2)} | {Math.Round(paths.Average(x => model.GetDistance(x)), 2)} ");
            Console.ReadKey();
        }
    }
}