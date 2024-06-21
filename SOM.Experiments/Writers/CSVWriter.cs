using SOM.Configuration;
using System;

namespace SOM.Experiments.Writer
{
	public class CSVWriter<TResearch> : FileWriter<TResearch> where TResearch : struct, IComparable<TResearch>
	{
		private const string CSV_SEPARATOR_DEFINITION = "sep=";

		private bool placedHeader = false;
		private string separator;
		protected override string _extension => "csv";

		public CSVWriter(string path, string fileName, SOMSettings settings, SOMExperimentSettings<TResearch> experimentSettings, string separator = ";")
			: base(path, fileName, settings, experimentSettings)
		{
			this.separator = separator;
		}

		public override void Write(SOMExperimentResult result)
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

				var data = $"{result.IsGroupResult}{separator}{result.ResearchedParameterValue}{separator}{result.Length}{separator}{result.Time}";

				writer.WriteLine(data);
			}
		}

		private string GetHeaderString(SOMExperimentSettings<TResearch> experimentSettings, SOMSettings settings)
		{
			var header = $"{"Group"}{separator}{experimentSettings.ResearchedParameterName}{separator}{"Length"}{separator}{"Time elapsed"}";
			return header;
		}

		public override void WriteLog(Exception ex)
		{
			using (var writer = System.IO.File.AppendText(_fullFilePath))
				writer.WriteLine(ex);
		}
	}
}
