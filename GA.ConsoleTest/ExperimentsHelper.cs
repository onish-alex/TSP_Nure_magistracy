using Algorithms.Utility.Extensions;
using GA.Analytics;
using GA.Core;
using GA.Core.Models;
using GA.Core.Operations.Crossovers;
using GA.Core.Operations.Mutations;
using GA.Core.Operations.Selections;
using GA.Core.Utility;
using GA.Operations;
using GA.Operations.Crossovers;
using GA.Operations.Crossovers.Concurrent;
using GA.Operations.Mutations;
using GA.Operations.Selections;
using GA.Operations.Selections.Concurrent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GA.ConsoleApp
{
    public static class ExperimentsHelper
    {
        public static void Run<TNode, TResearch>(
            IList<TNode> nodes,
            GASettings settings,
            GAExperimentSettings<TResearch> experimentSettings,
            Func<Individual<TNode>, double> fitnessGetter,
            Func<Individual<TNode>, double> resultGetter)
            where TResearch : struct, IComparable<TResearch>
        {
            var researchedProperty = settings.GetType().GetProperty(experimentSettings.ResearchedParameterName(settings));

            if (researchedProperty == null)
                throw new Exception();
            
            var researchedParam = experimentSettings.ResearchedParameterRange.Min;

            IList<Individual<TNode>> population = null;

            if (experimentSettings.UseSameInitialPopulation)
                population = GeneratePopulation(nodes, experimentSettings.PopulationSize);

            Console.WriteLine($"{experimentSettings.ResearchedParameterName(settings), 20} | {"Time elapsed", 19} | {"Minimum", 9} | {"Maximum", 9} | {"Average", 9} | {"Degeneration coef.", 17}");

            while (researchedParam.Value.CompareTo(experimentSettings.ResearchedParameterRange.Max.Value) < 0) 
            {
                if (!experimentSettings.UseSameInitialPopulation)
                    population = GeneratePopulation(nodes, experimentSettings.PopulationSize);

                researchedProperty.SetValue(settings, researchedParam.Value);

                var selection = CreateSelection(settings.SelectionType, experimentSettings.SelectionSettings);
                var crossover = CreateCrossover(settings.CrossoverType, experimentSettings.CrossoverSettings);
                var mutation = CreateMutation(settings.MutationsType, experimentSettings.MutationSettings);

                var algo = new GeneticAlgorithm<TNode>(
                    selection,
                    crossover,
                    mutation,
                    population.ToList(),
                    settings, 
                    fitnessGetter);

                var timer = Stopwatch.StartNew();
                algo.Run();
                timer.Stop();

                var resultPopulation = algo.Population;

                Console.WriteLine(
                    $"{researchedParam,20} " +
                    $"| {timer.Elapsed,19} " +
                    $"| {Math.Round(resultPopulation.Min(x => resultGetter(x)), 2),9} " +
                    $"| {Math.Round(resultPopulation.Max(x => resultGetter(x)), 2),9} " +
                    $"| {Math.Round(resultPopulation.Average(x => resultGetter(x)), 2),9} " +
                    $"| {Math.Round(resultPopulation.GetDegenerationCoefficient() * 100, 2).ToString() + "%",17}");

                researchedParam.AddStore(experimentSettings.ResearchedParameterIncrement.Value);
            }
        }

        private static IMutation CreateMutation(MutationsEnum mutationsType, GAOperationSettings mutationSettings)
        {
            return mutationsType switch
            {
                MutationsEnum.Swap => new SwapMutation(mutationSettings),
                MutationsEnum.Shift => new ShiftMutation(mutationSettings),
                MutationsEnum.Inverse => new InverseMutation(mutationSettings),
                
                MutationsEnum.ParallelSwap => new SwapMutation(mutationSettings),
                MutationsEnum.ParallelShift => new ShiftMutation(mutationSettings),
                MutationsEnum.ParallelInverse => new InverseMutation(mutationSettings),
                
                _ => throw new ArgumentException()
            };
        }

        private static ICrossover CreateCrossover(CrossoversEnum crossoverType, GAOperationSettings crossoverSettings)
        {
            return crossoverType switch
            {
                CrossoversEnum.Cyclic => new CyclicCrossover(crossoverSettings),
                CrossoversEnum.BitMask => new BitMaskCrossover(crossoverSettings),
                CrossoversEnum.PartiallyMapped => new PartiallyMappedCrossover(crossoverSettings),
                CrossoversEnum.TwoPointOrdered => new TwoPointOrderedCrossover(crossoverSettings),
                CrossoversEnum.SinglePointOrdered => new SinglePointOrderedCrossover(crossoverSettings),

                CrossoversEnum.ParallelCyclic => new ParallelCyclicCrossover(crossoverSettings),
                CrossoversEnum.ParallelBitMask => new ParallelBitMaskCrossover(crossoverSettings),
                CrossoversEnum.ParallelPartiallyMapped => new ParallelPartiallyMappedCrossover(crossoverSettings),
                CrossoversEnum.ParallelTwoPointOrdered => new ParallelTwoPointOrderedCrossover(crossoverSettings),
                CrossoversEnum.ParallelSinglePointOrdered => new ParallelSinglePointOrderedCrossover(crossoverSettings),
                
                _ => throw new ArgumentException()
            };
        }

        private static ISelection CreateSelection(SelectionsEnum selectionType, GAOperationSettings selectionSettings)
        {
            return selectionType switch
            {
                SelectionsEnum.RouletteWheel => new RouletteWheelSelection(selectionSettings),
                SelectionsEnum.Tournament => new TournamentSelection(selectionSettings),
                
                SelectionsEnum.ParallelRouletteWheel => new ParallelRouletteWheelSelection(selectionSettings),
                SelectionsEnum.ParallelTournament => new ParallelTournamentSelection(selectionSettings),

                _ => throw new ArgumentException()
            };
        }

        public static IList<Individual<TNode>> GeneratePopulation<TNode>(IList<TNode> nodes, int size)
        {
            IList<Individual<TNode>> population = new List<Individual<TNode>>(size);

            for (var i = 0; i < size; i++)
            {
                var nodesSet = Random.Shared.GetUniqueRandomSet(nodes, nodes.Count);
                population.Add(new Individual<TNode>(nodesSet));
            }

            return population;
        }

        //private static void 
    }
}
