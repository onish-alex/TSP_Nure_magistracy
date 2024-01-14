using GA.Core.Utility;
using System;
using System.Globalization;
using System.Linq;
using TSP.Examples;
using Algorithms.Utility.NumberWrapper;
using GA.Experiments;
using GA.Experiments.Writer;
using System.Collections.Generic;
using GA.Operations;
using GA.Experiments;

namespace GA.ConsoleTest
{
    class Program
    {

        //посмотреть, можно ли где заюзать Interlocked
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

            var model = PreparedModelLoader.GetModel(PreparedModelsEnum.ch150);
            var solution = PreparedModelLoader.GetSolution(model, PreparedModelsEnum.ch150);

            var settings = new GASettings()
            {
                ElitePercent = 100D,
                MutationProbability = 0,
                OnlyChildrenInNewGeneration = false,
                GenerationsMaxCount = 300,
                StagnatingGenerationsLimit = 200,
            };

            var crossovers = Enum.GetValues<CrossoversEnum>()
                .OrderBy(x => (int)x)
                .Select(x => (int)x);

            var experimentSettings = new GAExperimentSettings<double>()
            {
                PopulationSize = 500,
                ResearchedParameterName = nameof(settings.MutationProbability),
                ResearchedParameterIncrement = 20,
                ResearchedParameterRange = (new NumberDouble(0), new NumberDouble(100)),
                UseSameInitialPopulation = true,

                CrossoverSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryGeneration, NodesCount = model.Nodes.Count },
                SelectionSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryGeneration, NodesCount = model.Nodes.Count },
                MutationSettings = new GAOperationSettings() { InitType = GAOperationInitType.EveryGeneration, NodesCount = model.Nodes.Count },

                CrossoverType = CrossoversEnum.PartiallyMapped,
                MutationsType = MutationsEnum.Shift,
                SelectionType = SelectionsEnum.RouletteWheel,

                ControlRepeatingCount = 3
            };

            var results = ExperimentsHelper.Run(
                model.Nodes,
                settings,
                experimentSettings,
                (x) => 1 / model.GetDistance(x),
                (x) => model.GetDistance(x),
                WritersEnum.CSV | WritersEnum.JSON | WritersEnum.Console);
        }
    }
}
