﻿using SOM.TSPCompatibility;
using System.Collections.Generic;
using SOM;
using System.Linq;
using TSP.Examples;
using System.Globalization;
using System;
using TSP.Core;

namespace SOM.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

            var model = PreparedModelLoader.GetModel(PreparedModelsEnum.ulysses16);

            var som = new TwoDimensionalSOM<IVector<double>>(
                new Configuration.SOMSettings() 
                { 
                    LearningCoefficient = 0.2D, 
                    UseDistancePenalties = true,
                    PenaltiesIncreasingCoefficient = 0.05D,
                    RoundPrecision = 1D,
                    LearningFadingCoefficient = 0.01D,
                    UseElasticity = false,
                },
                model.Nodes.Select(x => SOMMapper.Map(x)).ToList(), 
                (x) => new Vector(x.ToList()), 
                Configuration.Topology.Sphere);


            var nodes = som.BuildMap().Select(x => SOMMapper.Map(model, x));
            Console.WriteLine(model.GetDistance(nodes.ToList()));

            Console.WriteLine("Route1:" + string.Join(',', nodes.Select(x => x.Name)));
            Console.WriteLine(string.Join("\r\n", nodes.Select(x => $"{x.X:F2};{x.Y:F2}")));

            var route2 = new List<TSPNode>()
            {
                nodes.Single(x => x.Name == "1"),
                nodes.Single(x => x.Name == "14"),
                nodes.Single(x => x.Name == "13"),
                nodes.Single(x => x.Name == "12"),
                nodes.Single(x => x.Name == "7"),
                nodes.Single(x => x.Name == "6"),
                nodes.Single(x => x.Name == "15"),
                nodes.Single(x => x.Name == "5"),
                nodes.Single(x => x.Name == "11"),
                nodes.Single(x => x.Name == "9"),
                nodes.Single(x => x.Name == "10"),
                nodes.Single(x => x.Name == "16"),
                nodes.Single(x => x.Name == "3"),
                nodes.Single(x => x.Name == "2"),
                nodes.Single(x => x.Name == "4"),
                nodes.Single(x => x.Name == "8"),
            };


            Console.WriteLine("Route2:" + model.GetDistance(route2.ToList()));
        }
    }
}