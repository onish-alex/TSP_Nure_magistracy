using Algorithms.Utility.StructuresLinking;
using SOM.Configuration;
using SOM.Experiments.Writer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SOM.Experiments
{
	public static class SOMExperimentsEngine<TVector> where TVector : IVector<double>
	{
		public static IList<SOMExperimentResult> Run<TResearch>(
			IList<TVector> nodes,
			SOMSettings settings,
			SOMExperimentSettings<TResearch> experimentSettings,
			Func<IList<TVector>, double> resultGetter,
			Func<IList<TVector>, IList<TVector>> parser,
			WritersEnum writerTypes = WritersEnum.None,
			string path = "")
			where TResearch : struct, IComparable<TResearch>
		{
			IList<IExperimentResultWriter<TResearch>> writers = CreateWriters(writerTypes, path, settings, experimentSettings);

			var resultsList = new List<SOMExperimentResult>();

			//get researched parameter reflection info
			var researchedProperty = settings.GetType().GetProperty(experimentSettings.ResearchedParameterName);
			var researchedParam = experimentSettings.ResearchedParameterRange.Min;

			while (researchedParam.Value.CompareTo(experimentSettings.ResearchedParameterRange.Max.Value) <= 0)
			{
				try
				{
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
						var settingsClone = settings.Clone;

						var algo = new TwoDimensionalSOM<TVector>(
							settingsClone,
							nodes,
							Configuration.Topology.Sphere);

						var timer = Stopwatch.StartNew();

						while (!algo.FinishCondition)
						{
							algo.ProcessIteration();
							Console.WriteLine($"{algo.ProcessedVectors} - {timer.Elapsed} | coef: {settingsClone.LearningCoefficient} | length: {algo.GetFullLength()} | n: {settingsClone.CooperationCoefficient}");
						}

						timer.Stop();

						var result = parser(algo.Network.ToList());

						var experimentResult = new SOMExperimentResult()
						{
							Length = resultGetter(result),
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

					if (repeatingCount > 1)
					{
						var group = resultsList.Where(x => !x.IsGroupResult && x.GroupGuid == groupGuid);

						var groupExperimentResult = new SOMExperimentResult()
						{
							Length = group.Min(x => x.Length),
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
				catch (Exception ex)
				{
					if (writers != null && writers.Any())
						foreach (var writer in writers)
							writer.WriteLog(ex);

					break;
				}
			}

			if (writers != null && writers.Any())
				foreach (var writer in writers)
					if (writer is IDisposable disposableWriter)
						disposableWriter.Dispose();

			return resultsList;
		}

		private static IList<IExperimentResultWriter<TResearch>> CreateWriters<TResearch>(WritersEnum writerTypes, string path, SOMSettings settings, SOMExperimentSettings<TResearch> experimentSettings) where TResearch : struct, IComparable<TResearch>
		{
			var writers = new List<IExperimentResultWriter<TResearch>>();
			var fileName = GetFileName(experimentSettings);

			if ((writerTypes & WritersEnum.CSV) > 0) writers.Add(new CSVWriter<TResearch>(path, fileName, settings, experimentSettings));
			if ((writerTypes & WritersEnum.JSON) > 0) writers.Add(new JsonWriter<TResearch>(path, fileName, settings, experimentSettings));
			if ((writerTypes & WritersEnum.Console) > 0) writers.Add(new ConsoleWriter<TResearch>(settings, experimentSettings));

			return writers;
		}

		private static string GetFileName<TResearch>(SOMExperimentSettings<TResearch> experimentSettings) where TResearch : struct, IComparable<TResearch>
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
