using System;

namespace AntColony.Experiments.Writer
{
	public class CSVWriter<TResearch> : FileWriter<TResearch> where TResearch : struct, IComparable<TResearch>
	{
		private const string CSV_SEPARATOR_DEFINITION = "sep=";

		private bool placedHeader = false;
		private string separator;
		protected override string _extension => "csv";

		public CSVWriter(string path, string fileName, ACExperimentSettings<TResearch> experimentSettings, string separator = ";")
			: base(path, fileName, experimentSettings)
		{
			this.separator = separator;
		}

		public override void Write<TNode>(ACExperimentResult<TNode> result)
		{
			if (!placedHeader)
			{
				_writer.WriteLine(CSV_SEPARATOR_DEFINITION + separator);

				var header = GetHeaderString(_experimentSettings);
				_writer.WriteLine(header);
				placedHeader = true;
			}

			var data = $"{result.IsGroupResult}{separator}{result.ResearchedParameterValue}{separator}{result.Time}{separator}{result.MinResult}{separator}{result.MaxResult}{separator}{result.AverageResult}";

			_writer.WriteLine(data);
		}

		private string GetHeaderString(ACExperimentSettings<TResearch> experimentSettings)
		{
			var header = $"{"Group"}{separator}{experimentSettings.ResearchedParameterName}{separator}{"Time elapsed"}{separator}{"Minimum"}{separator}{"Maximum"}{separator}{"Average"}{separator}{"Iterations"}{separator}{"Degeneration coef."}";
			return header;
		}
	}
}
