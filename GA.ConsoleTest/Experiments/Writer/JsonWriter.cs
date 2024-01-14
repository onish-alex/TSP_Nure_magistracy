using GA.Core.Utility;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GA.ConsoleApp.Experiments.Writer
{
    public class JsonWriter<TResearch> : FileWriter<TResearch> where TResearch : struct, IComparable<TResearch>
    {
        private bool _wroteSettings = false;

        public JsonWriter(string path, string fileName, GASettings settings, GAExperimentSettings<TResearch> experimentSettings) 
            : base(path, fileName, settings, experimentSettings)
        {
        }

        protected override string _extension => "json";

        public override void Write<TNode>(ExperimentResult<TNode> result)
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
