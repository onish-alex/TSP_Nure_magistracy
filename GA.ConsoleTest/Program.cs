using GA.Core;
using GA.Core.Extensions;
using GA.Core.Helpers;
using GA.Core.Selections;
using System;
using System.Globalization;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using TSP.Core;
using GA.Core.Crossovers;
using GA.Core.Mutations;

namespace GA.ConsoleTest
{
    class Program
    {
        public static double GetDegenerationCoefficient<T>(IList<IList<T>> population)
        {
            var equalIndividuals = new Dictionary<IList<T>, int>();

            bool isSolutionContains;
            foreach (var individual in population)
            {
                isSolutionContains = false;

                foreach (var eqInd in equalIndividuals.Keys)
                {
                    if (eqInd.SequenceEqual(individual))
                    {
                        equalIndividuals[eqInd]++;
                        isSolutionContains = true;
                        break;
                    }
                }

                if (!isSolutionContains)
                    equalIndividuals.Add(individual, 1);
            }

            var coef = 1 - (double)(equalIndividuals.Count - 1) / (population.Count - 1);

            return coef;        
        }

        //public static double GetDegenerationCoefficient<T>(IList<IList<T>> population)
        //{
        //    var equalIndividuals = new List<IList<T>>();

        //    bool isSolutionContains;

        //    foreach (var individual in population)
        //    {
        //        isSolutionContains = false;

        //        foreach (var eqInd in equalIndividuals)
        //        {
        //            if (eqInd.SequenceEqual(individual))
        //            {
        //                isSolutionContains = true;
        //                break;
        //            }
        //        }

        //        if (!isSolutionContains)
        //            equalIndividuals.Add(individual);
        //    }

        //    var coef = -1 * (double)(equalIndividuals.Count - 1) / (population.Count - 1) + 1;

        //    return coef;
        //}

        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");


            var modelName = "gr96.tsp";
            var populationSize = 7500;
            var generationsAmount = 500;
            var mutationProbability = 5D;
            var eliteCoefficient = 100D;
            var mutationSwapSectionLength = 2;

            var model = TSPModelLoader.GetModelFromFile(modelName);
            var rand = new Random();

            var mutation = new SwapMutation() { SwapSectionLength = mutationSwapSectionLength };

            var algo = new GeneticAlgorithm<TSPNode>(
                new RouletteWheelSelection(), 
                new SinglePointCrossover(), 
                mutation);

            var population = new List<IList<TSPNode>>(populationSize);

            for (int i = 0; i < populationSize; i++)
            {
                var individual = rand.GetUniqueRandomSet(model.Nodes, model.Nodes.Count);
                population.Add(individual);
            }

            var timer = Stopwatch.StartNew();

            for (var i = 0; i < generationsAmount; i++)
                population = (List<IList<TSPNode>>)algo.GetNextGenerationWithParents(
                    population, 
                    (x) => 1 / model.GetDistance(x), 
                    mutationProbability);

            timer.Stop();

            Console.WriteLine($"{"Time elapsed"} | {"Minimum"} | {"Maximum"} | {"Average"} | {"Degeneration coef."}");
            Console.WriteLine($"{timer.Elapsed} | {Math.Round(population.Min(x => model.GetDistance(x)), 2)} | {Math.Round(population.Max(x => model.GetDistance(x)), 2)} | {Math.Round(population.Average(x => model.GetDistance(x)), 2)} | {Math.Round(GetDegenerationCoefficient(population) * 100, 2)}%");
            Console.ReadKey();
        }
    }
}
