using GA.Core.Utility;
using System;

namespace GA.Experiments.Writer
{
	public class ConsoleWriter<TResearch> : IExperimentResultWriter<TResearch> where TResearch : struct, IComparable<TResearch>
	{
		protected GASettings _settings;
		protected GAExperimentSettings<TResearch> _experimentSettings;
		private bool placedHeader = false;

		public ConsoleWriter(GASettings settings, GAExperimentSettings<TResearch> experimentSettings)
		{
			_settings = settings;
			_experimentSettings = experimentSettings;
		}

		public void Write<TNode>(GAExperimentResult<TNode> result)
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
				$"| {Math.Round(result.MinResult, 2),10} " +
				$"| {Math.Round(result.MaxResult, 2),10} " +
				$"| {Math.Round(result.AverageResult, 2),10} " +
				$"| {result.LastIterationNumber,12} " +
				$"| {result.Time,17} " +
				$"| {result.TimeForIteration,17} " +
				$"| {Math.Round(result.DegenerationCoefficient, 2).ToString() + "%",17}" +
				$"| {Math.Round(result.DegenerationCoefficient2, 2).ToString() + "%",17}");
		}

		public void WriteLog(Exception ex) => Console.WriteLine(ex);

		private string GetHeaderString(GAExperimentSettings<TResearch> experimentSettings, GASettings settings)
		{
			var header = $"| {"Group",6} " +
						 $"| {experimentSettings.ResearchedParameterName,20} " +
						 $"| {"Minimum",10} " +
						 $"| {"Maximum",10} " +
						 $"| {"Average",10} " +
						 $"| {"Iterations",12} " +
						 $"| {"Time elapsed",17} " +
						 $"| {"Avg iter time",17} " +
						 $"| {"Degeneration coef.",17}";

			return header;
		}
	}
}
