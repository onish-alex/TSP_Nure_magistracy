using Algorithms.Utility.Extensions;
using AntColony.Experiments.Writer;
using AntColony.Experiments;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json.Schema;
using AntColony.Core.Utilities;
using AntColony.Core;

namespace AntColony.Experiments
{
	public static class ACExperimentsEngine
	{
		public static IList<ACExperimentResult<TNode>> Run<TNode, TResearch>(
			IList<TNode> nodes,
			ACExperimentSettings<TResearch> experimentSettings,
			Func<TNode, TNode, double> distanceGetter,
			Func<IList<TNode>, double> resultGetter,
			WritersEnum writerTypes = WritersEnum.None,
			string path = "")
			where TResearch : struct, IComparable<TResearch>
			where TNode : class
		{
			IList<IExperimentResultWriter<TResearch>> writers = CreateWriters(writerTypes, path, experimentSettings);

			var resultsList = new List<ACExperimentResult<TNode>>();

			//get researched parameter reflection info
			var researchedProperty = experimentSettings.GetType().GetProperty(experimentSettings.ResearchedParameterName);
			var researchedParam = experimentSettings.ResearchedParameterRange.Min;

			while (researchedParam.Value.CompareTo(experimentSettings.ResearchedParameterRange.Max.Value) <= 0)
			{
				try
				{
					//set researched parameter 
					researchedProperty.SetValue(experimentSettings, researchedParam.Value);

					//control repeating
					var repeatingCount = experimentSettings.ControlRepeatingCount;
					var groupGuid = Guid.Empty;

					if (experimentSettings.ControlRepeatingCount < 1)
						repeatingCount = 1;
					else if (experimentSettings.ControlRepeatingCount > 1)
						groupGuid = Guid.NewGuid();

					for (int i = 0; i < repeatingCount; i++)
					{
						var ACSettings = new AntColonySettings()
						{
							UpdatePheromonesForGlobalBestWay = experimentSettings.UpdatePheromonesForGlobalBestWay,
							CommonAntPheromoneAmount = experimentSettings.CommonAntPheromoneAmount,
							EvaporationCoefficient = experimentSettings.EvaporationCoefficient,
							CommonEliteAntPheromoneAmount = experimentSettings.CommonEliteAntPheromoneAmount,
							DistanceWeight = experimentSettings.DistanceWeight,
							InitialPheromoneAmount = experimentSettings.InitialPheromoneAmount,
							MaxPheromoneAmount = experimentSettings.MaxPheromoneAmount,
							MinPheromoneAmount = experimentSettings.MinPheromoneAmount,
							PheromoneWeight = experimentSettings.PheromoneWeight,
							UseCommonAntPheromoneAmount = experimentSettings.UseCommonAntPheromoneAmount,
							UseCommonEliteAntPheromoneAmount = experimentSettings.UseCommonEliteAntPheromoneAmount
						};

						var antPopulationSettings = new AntPopulationSettings()
						{
							AntCount = experimentSettings.AntCount,
							AntPersonalPheromoneAmounts = experimentSettings.AntPersonalPheromoneAmounts,
							EliteAntCount = experimentSettings.EliteAntCount,
							EliteAntPersonalPheromoneAmounts = experimentSettings.EliteAntPersonalPheromoneAmounts,
						};

						BaseAlgorithm<TNode> algo = experimentSettings.AntColonyType switch
						{
							AntColonyEnum.Classic => new ClassicAlgorithm<TNode>(nodes, distanceGetter, ACSettings),
							AntColonyEnum.Elitist => new ElitistAlgorithm<TNode>(nodes, distanceGetter, ACSettings, resultGetter),
							AntColonyEnum.MinMax => new MaxMinAlgorithm<TNode>(nodes, distanceGetter, ACSettings, resultGetter),
							_ => throw new NotImplementedException()
						};
						
						var timer = Stopwatch.StartNew();

						IList<IList<TNode>> algoResults = null;

						for (int j = 0; j < experimentSettings.IterationsCount; j++)
                            algoResults = algo.Run(antPopulationSettings);

                        timer.Stop();

						var experimentResult = new ACExperimentResult<TNode>()
						{
							FoundRoutes = algoResults,
							MinResult = algoResults.Min(x => resultGetter(x)),
							MaxResult = algoResults.Max(x => resultGetter(x)),
							AverageResult = algoResults.Average(x => resultGetter(x)),
							//DegenerationCoefficient = algo.Population.GetDegenerationCoefficient() * 100,
							//LastIterationNumber = algo.Iteration,
							ResearchedParameterName = experimentSettings.ResearchedParameterName,
							ResearchedParameterValue = researchedProperty.GetValue(experimentSettings),
							Time = timer.Elapsed,
							IsGroupResult = false,
							GroupGuid = groupGuid
						};

						resultsList.Add(experimentResult);

						if (writers != null && writers.Any())
							foreach (var writer in writers)
								writer.Write(experimentResult);
					}

					if (repeatingCount > 1)
					{
						var group = resultsList.Where(x => !x.IsGroupResult && x.GroupGuid == groupGuid);

						var groupExperimentResult = new ACExperimentResult<TNode>()
						{
							MinResult = group.Min(x => x.MinResult),
							MaxResult = group.Max(x => x.MaxResult),
							AverageResult = group.Average(x => x.AverageResult),
							//DegenerationCoefficient = group.Average(x => x.DegenerationCoefficient),
							//LastIterationNumber = group.Average(x => x.LastIterationNumber),
							ResearchedParameterName = experimentSettings.ResearchedParameterName,
							ResearchedParameterValue = researchedProperty.GetValue(experimentSettings),
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
				catch (Exception ex)
				{
					break;
				}
			}

			if (writers != null && writers.Any())
				foreach (var writer in writers)
					if (writer is IDisposable disposableWriter)
						disposableWriter.Dispose();

			return resultsList;
		}

		private static IList<IExperimentResultWriter<TResearch>> CreateWriters<TResearch>(WritersEnum writerTypes, string path, ACExperimentSettings<TResearch> experimentSettings) where TResearch : struct, IComparable<TResearch>
		{
			var writers = new List<IExperimentResultWriter<TResearch>>();
			var fileName = GetFileName(experimentSettings);

			if ((writerTypes & WritersEnum.CSV) > 0) writers.Add(new CSVWriter<TResearch>(path, fileName, experimentSettings));
			if ((writerTypes & WritersEnum.JSON) > 0) writers.Add(new JsonWriter<TResearch>(path, fileName, experimentSettings));
			if ((writerTypes & WritersEnum.Console) > 0) writers.Add(new ConsoleWriter<TResearch>(experimentSettings));

			return writers;
		}

		private static string GetFileName<TResearch>(ACExperimentSettings<TResearch> experimentSettings) where TResearch : struct, IComparable<TResearch>
		{
			var nameComponents = new List<string>()
			{
				DateTime.Now.ToString("yyMMdd_HHmmss"),
				experimentSettings.ResearchedParameterName,
				experimentSettings.ResearchedParameterRange.Min.Value.ToString(),
				experimentSettings.ResearchedParameterRange.Max.Value.ToString(),
				experimentSettings.ResearchedParameterIncrement.ToString(),
			};

			return string.Join('-', nameComponents);
		}
	}
}
