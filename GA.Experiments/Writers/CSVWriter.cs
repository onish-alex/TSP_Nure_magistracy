using GA.Core.Utility;
using System;

namespace GA.Experiments.Writer
{
	public class CSVWriter<TResearch> : FileWriter<TResearch> where TResearch : struct, IComparable<TResearch>
	{
		private const string CSV_SEPARATOR_DEFINITION = "sep=";

		private bool placedHeader = false;
		private string separator;
		protected override string _extension => "csv";

		public CSVWriter(string path, string fileName, GASettings settings, GAExperimentSettings<TResearch> experimentSettings, string separator = ";")
			: base(path, fileName, settings, experimentSettings)
		{
			this.separator = separator;
		}

		public override void Write<TNode>(GAExperimentResult<TNode> result)
		{
			using (var writer = System.IO.File.AppendText(_fullFilePath))
			{
				if (!placedHeader)
				{
					writer.WriteLine(CSV_SEPARATOR_DEFINITION + separator);

					var header = GetHeaderString(_experimentSettings, _settings);
					writer.WriteLine(header);
					placedHeader = true;
				}

				var data = $"{result.IsGroupResult}{separator}{result.ResearchedParameterValue}{separator}{result.MinResult}{separator}{result.MaxResult}{separator}{result.AverageResult}{separator}{result.LastIterationNumber}{separator}{result.Time}{separator}{result.TimeForIteration}{separator}{result.DegenerationCoefficient}{separator}{result.DegenerationCoefficient2}";

				writer.WriteLine(data);
			}
		}

		private string GetHeaderString(GAExperimentSettings<TResearch> experimentSettings, GASettings settings)
		{
			var header = $"{"Group"}{separator}{experimentSettings.ResearchedParameterName}{separator}{"Minimum"}{separator}{"Maximum"}{separator}{"Average"}{separator}{"Iterations"}{separator}{"Time elapsed"}{separator}{"Avg iteration time"}{separator}{"Degeneration coef."}{separator}{"Degeneration coef2."}";
			return header;
		}

		public override void WriteLog(Exception ex)
		{
			using (var writer = System.IO.File.AppendText(_fullFilePath))
				writer.WriteLine(ex);
		}
	}
}
