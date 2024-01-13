﻿using Algorithms.Utility.Extensions;
using GA.Analytics;
using GA.ConsoleApp.Experiments.Writer;
using GA.Core;
using GA.Core.Models;
using GA.Core.Operations.Crossovers;
using GA.Core.Operations.Mutations;
using GA.Core.Operations.Selections;
using GA.Core.Utility;
using GA.Operations.Crossovers;
using GA.Operations.Crossovers.Concurrent;
using GA.Operations.Mutations;
using GA.Operations.Selections;
using GA.Operations.Selections.Concurrent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GA.ConsoleApp.Experiments
{
    public static class ExperimentsHelper
    {
        internal static IList<ExperimentResult<TNode>> Run<TNode, TResearch>(
            IList<TNode> nodes,
            GASettings settings,
            GAExperimentSettings<TResearch> experimentSettings,
            Func<Individual<TNode>, double> fitnessGetter,
            Func<Individual<TNode>, double> resultGetter,
            WritersEnum writerTypes = WritersEnum.None,
            string path = null)
            where TResearch : struct, IComparable<TResearch>
        {
            IList<IExperimentResultWriter<TResearch>> writers = CreateWriters(writerTypes, path, settings, experimentSettings);

            var resultsList = new List<ExperimentResult<TNode>>();

            //get researched parameter reflection info
            var researchedProperty = settings.GetType().GetProperty(experimentSettings.ResearchedParameterName);
            var researchedParam = experimentSettings.ResearchedParameterRange.Min;

            //set initial population
            IList<Individual<TNode>> population = null;
            if (experimentSettings.UseSameInitialPopulation)
                population = GeneratePopulation(nodes, experimentSettings.PopulationSize);

            while (researchedParam.Value.CompareTo(experimentSettings.ResearchedParameterRange.Max.Value) <= 0)
            {
                //reinit population if needed
                if (!experimentSettings.UseSameInitialPopulation)
                    population = GeneratePopulation(nodes, experimentSettings.PopulationSize);

                //set researched parameter 
                researchedProperty.SetValue(settings, researchedParam.Value);

                //control repeating
                var repeatingCount = experimentSettings.ControlRepeatingCount;
                var groupGuid = Guid.Empty;

                if (experimentSettings.ControlRepeatingCount < 1)
                    repeatingCount = 1;
                else if (experimentSettings.ControlRepeatingCount > 1)
                    groupGuid = Guid.NewGuid();

                for (int i = 0; i < repeatingCount; i++)
                {
                    var selection = CreateSelection(experimentSettings.SelectionType, experimentSettings.SelectionSettings);
                    var crossover = CreateCrossover(experimentSettings.CrossoverType, experimentSettings.CrossoverSettings);
                    var mutation = CreateMutation(experimentSettings.MutationsType, experimentSettings.MutationSettings);

                    var algo = new GeneticAlgorithm<TNode>(
                        selection,
                        crossover,
                        mutation,
                        population.ToList(),
                        settings,
                        fitnessGetter);

                    var timer = Stopwatch.StartNew();
                    var bestResult = algo.Run();
                    timer.Stop();

                    var experimentResult = new ExperimentResult<TNode>()
                    {
                        StartPopulation = population,
                        FinishPopulation = algo.Population,
                        MinResult = resultGetter(bestResult.Individual),
                        MaxResult = algo.Population.Max(x => resultGetter(x)),
                        AverageResult = algo.Population.Average(x => resultGetter(x)),
                        DegenerationCoefficient = algo.Population.GetDegenerationCoefficient() * 100,
                        LastIterationNumber = algo.Iteration,
                        ResearchedParameterName = experimentSettings.ResearchedParameterName,
                        ResearchedParameterValue = researchedProperty.GetValue(settings),
                        Time = timer.Elapsed,
                        IsGroupResult = false,
                        GroupGuid = groupGuid
                    };

                    resultsList.Add(experimentResult);

                    if (writers != null && writers.Any())
                        foreach (var writer in writers)
                            writer.Write(experimentResult);
                }

                if (repeatingCount > 0)
                {
                    var group = resultsList.Where(x => !x.IsGroupResult && x.GroupGuid == groupGuid);

                    var groupExperimentResult = new ExperimentResult<TNode>()
                    {
                        StartPopulation = population,
                        FinishPopulation = group.First().FinishPopulation,
                        MinResult = group.Min(x => x.MinResult),
                        MaxResult = group.Max(x => x.MaxResult),
                        AverageResult = group.Average(x => x.AverageResult),
                        DegenerationCoefficient = group.Average(x => x.DegenerationCoefficient),
                        LastIterationNumber = group.Average(x => x.LastIterationNumber),
                        ResearchedParameterName = experimentSettings.ResearchedParameterName,
                        ResearchedParameterValue = researchedProperty.GetValue(settings),
                        Time = new TimeSpan((long)group.Average(x => x.Time.Ticks)),
                        IsGroupResult = true,
                        GroupGuid = groupGuid
                    };

                    resultsList.Add(groupExperimentResult);
                    
                    if (writers != null && writers.Any())
                        foreach (var writer in writers)
                            writer.Write(groupExperimentResult);
                }

                researchedParam.AddStore(experimentSettings.ResearchedParameterIncrement);
            }

            return resultsList;
        }

        private static IList<IExperimentResultWriter<TResearch>> CreateWriters<TResearch>(WritersEnum writerTypes, string path, GASettings settings, GAExperimentSettings<TResearch> experimentSettings) where TResearch : struct, IComparable<TResearch>
        {
            var writers = new List<IExperimentResultWriter<TResearch>>();
            var fileName = GetFileName(experimentSettings);

            if ((writerTypes & WritersEnum.CSV) > 0) writers.Add(new CSVWriter<TResearch>(path, fileName, settings, experimentSettings));
            if ((writerTypes & WritersEnum.JSON) > 0) writers.Add(new JsonWriter<TResearch>(path, fileName, settings, experimentSettings));

            return writers;
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

        private static IList<Individual<TNode>> GeneratePopulation<TNode>(IList<TNode> nodes, int size)
        {
            IList<Individual<TNode>> population = new List<Individual<TNode>>(size);

            for (var i = 0; i < size; i++)
            {
                var nodesSet = Random.Shared.GetUniqueRandomSet(nodes, nodes.Count);
                population.Add(new Individual<TNode>(nodesSet));
            }

            return population;
        }

        private static string GetFileName<TResearch>(GAExperimentSettings<TResearch> experimentSettings) where TResearch : struct, IComparable<TResearch>
        {
            var nameComponents = new List<string>()
            {
                DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                experimentSettings.ResearchedParameterName,
                experimentSettings.ResearchedParameterRange.Min.Value.ToString(),
                experimentSettings.ResearchedParameterRange.Max.Value.ToString(),
                experimentSettings.ResearchedParameterIncrement.ToString(),
            };

            return string.Join('-', nameComponents);
        } 
    }
}
