using Newtonsoft.Json;
using System;
using System.IO;

namespace AntColony.Experiments.Writer
{
	public class JsonWriter<TResearch> : FileWriter<TResearch> where TResearch : struct, IComparable<TResearch>
	{
		private bool _wroteSettings = false;

		public JsonWriter(string path, string fileName, ACExperimentSettings<TResearch> experimentSettings)
			: base(path, fileName, experimentSettings)
		{
		}

		protected override string _extension => "json";

		public override void Write<TNode>(ACExperimentResult<TNode> result)
		{
			if (!_wroteSettings)
			{
				using (var experimentSettingsStream = File.OpenWrite(Path.Combine(_path, $"{_fileName}{nameof(_experimentSettings)}.{_extension}")))
				using (var experimentSettingsWriter = new StreamWriter(experimentSettingsStream))
				{
					experimentSettingsWriter.WriteLine(JsonConvert.SerializeObject(_experimentSettings));
				}

				_wroteSettings = true;

				_writer.Write("[");
			}
			else
			{
				_writer.Write(",");
			}

			_writer.Write(JsonConvert.SerializeObject(result));
		}

		protected override void Dispose(bool disposing)
		{
			if (!disposedValue)
				if (disposing)
					_writer.Write("]");

			base.Dispose(disposing);
		}
	}
}
