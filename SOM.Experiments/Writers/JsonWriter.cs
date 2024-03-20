using Newtonsoft.Json;
using SOM.Configuration;
using System;
using System.IO;

namespace SOM.Experiments.Writer
{
	public class JsonWriter<TResearch> : FileWriter<TResearch> where TResearch : struct, IComparable<TResearch>
	{
		private bool _wroteSettings = false;

		public JsonWriter(string path, string fileName, SOMSettings settings, SOMExperimentSettings<TResearch> experimentSettings)
			: base(path, fileName, settings, experimentSettings)
		{
		}

		protected override string _extension => "json";

		public override void Write(SOMExperimentResult result)
		{
			using (var writer = System.IO.File.AppendText(_fullFilePath))
			{
				if (!_wroteSettings)
				{
					using (var settingsStream = File.OpenWrite(Path.Combine(_path, $"{_fileName}{nameof(_settings)}.{_extension}")))
					using (var settingsWriter = new StreamWriter(settingsStream))
					{
						settingsWriter.WriteLine(JsonConvert.SerializeObject(_settings));
					}

					using (var experimentSettingsStream = File.OpenWrite(Path.Combine(_path, $"{_fileName}{nameof(_experimentSettings)}.{_extension}")))
					using (var experimentSettingsWriter = new StreamWriter(experimentSettingsStream))
					{
						experimentSettingsWriter.WriteLine(JsonConvert.SerializeObject(_experimentSettings));
					}

					_wroteSettings = true;

					writer.Write("[");
				}
				else
				{
					writer.Write(",");
				}

				writer.Write(JsonConvert.SerializeObject(result));
			}
		}

		public override void WriteLog(Exception ex)
		{
			using (var writer = System.IO.File.AppendText(_fullFilePath))
				writer.WriteLine(ex);
		}

		protected override void Dispose(bool disposing)
		{
			if (!disposedValue)
				if (disposing)
					using (var writer = System.IO.File.AppendText(_fullFilePath))
						writer.Write("]");

			base.Dispose(disposing);
		}
	}
}
