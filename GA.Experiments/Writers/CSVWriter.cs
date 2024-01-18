using GA.Core.Utility;
using System;
using System.IO;

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
			using (var stream = System.IO.File.OpenWrite(_fullFilePath))
			using (var writer = new StreamWriter(stream))
			{
				if (!placedHeader)
				{
					writer.WriteLine(CSV_SEPARATOR_DEFINITION + separator);

					var header = GetHeaderString(_experimentSettings, _settings);
					writer.WriteLine(header);
					placedHeader = true;
				}

				var data = $"{result.IsGroupResult}{separator}{result.ResearchedParameterValue}{separator}{result.Time}{separator}{result.MinResult}{separator}{result.MaxResult}{separator}{result.AverageResult}{separator}{result.LastIterationNumber}{separator}{result.DegenerationCoefficient}";

				writer.WriteLine(data);
			}
		}

		private string GetHeaderString(GAExperimentSettings<TResearch> experimentSettings, GASettings settings)
		{
			var header = $"{"Group"}{separator}{experimentSettings.ResearchedParameterName}{separator}{"Time elapsed"}{separator}{"Minimum"}{separator}{"Maximum"}{separator}{"Average"}{separator}{"Iterations"}{separator}{"Degeneration coef."}";
			return header;
		}
	}
}
