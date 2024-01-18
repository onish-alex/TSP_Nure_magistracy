using GA.Core.Utility;
using Newtonsoft.Json;
using System;
using System.IO;

namespace GA.Experiments.Writer
{
	public class JsonWriter<TResearch> : FileWriter<TResearch> where TResearch : struct, IComparable<TResearch>
	{
		private bool _wroteSettings = false;

		public JsonWriter(string path, string fileName, GASettings settings, GAExperimentSettings<TResearch> experimentSettings)
			: base(path, fileName, settings, experimentSettings)
		{
		}

		protected override string _extension => "json";

		public override void Write<TNode>(GAExperimentResult<TNode> result)
		{
			using (var stream = System.IO.File.OpenWrite(_fullFilePath))
			using (var writer = new StreamWriter(stream))
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

		protected override void Dispose(bool disposing)
		{
			if (!disposedValue)
				if (disposing)
					using (var stream = System.IO.File.OpenWrite(_fullFilePath))
					using (var writer = new StreamWriter(stream))
						writer.Write("]");

			base.Dispose(disposing);
		}
	}
}
