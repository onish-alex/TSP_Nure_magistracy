using System;

namespace AntColony.Experiments.Writer
{
	public class ConsoleWriter<TResearch> : IExperimentResultWriter<TResearch> where TResearch : struct, IComparable<TResearch>
	{
		protected ACExperimentSettings<TResearch> _experimentSettings;
		private bool placedHeader = false;

		public ConsoleWriter(ACExperimentSettings<TResearch> experimentSettings)
		{
			_experimentSettings = experimentSettings;
		}

		public void Write<TNode>(ACExperimentResult<TNode> result)
		{
			if (!placedHeader)
			{
				var header = GetHeaderString(_experimentSettings);
				System.Console.WriteLine(header);
				placedHeader = true;
			}

			Console.WriteLine(
				$"| {result.IsGroupResult,6} " +
				$"| {result.ResearchedParameterValue,20} " +
				$"| {result.Time,19} " +
				$"| {Math.Round(result.MinResult, 2),9} " +
				$"| {Math.Round(result.MaxResult, 2),9} " +
				$"| {Math.Round(result.AverageResult, 2),9}");
		}

		private string GetHeaderString(ACExperimentSettings<TResearch> experimentSettings)
		{
			var header = $"| {"Group",6} " +
						 $"| {experimentSettings.ResearchedParameterName,20} " +
						 $"| {"Time elapsed",19} " +
						 $"| {"Minimum",9} " +
						 $"| {"Maximum",9} " +
						 $"| {"Average",9}";

			return header;
		}
	}
}
