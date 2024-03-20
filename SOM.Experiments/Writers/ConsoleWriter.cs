using SOM.Configuration;
using System;

namespace SOM.Experiments.Writer
{
	public class ConsoleWriter<TResearch> : IExperimentResultWriter<TResearch> where TResearch : struct, IComparable<TResearch>
	{
		protected SOMSettings _settings;
		protected SOMExperimentSettings<TResearch> _experimentSettings;
		private bool placedHeader = false;

		public ConsoleWriter(SOMSettings settings, SOMExperimentSettings<TResearch> experimentSettings)
		{
			_settings = settings;
			_experimentSettings = experimentSettings;
		}

		public void Write(SOMExperimentResult result)
		{
			if (!placedHeader)
			{
				var header = GetHeaderString(_experimentSettings, _settings);
				System.Console.WriteLine(header);
				placedHeader = true;
			}

			Console.WriteLine(
				$"| {result.IsGroupResult,6} " +
				$"| {result.ResearchedParameterValue,20} " +
				$"| {Math.Round(result.Length, 2),10} " +
				$"| {result.Time,17} ");
		}

		public void WriteLog(Exception ex) => Console.WriteLine(ex);

		private string GetHeaderString(SOMExperimentSettings<TResearch> experimentSettings, SOMSettings settings)
		{
			var header = $"| {"Group",6} " +
						 $"| {experimentSettings.ResearchedParameterName,20} " +
						 $"| {"Length",10} " +
						 $"| {"Time elapsed",17} ";

			return header;
		}
	}
}
