using GA.Core.Utility;
using Newtonsoft.Json;
using System;

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
                _writer.WriteLine(JsonConvert.SerializeObject(_settings));
                _writer.WriteLine(JsonConvert.SerializeObject(_experimentSettings));
                _wroteSettings = true;
            }

            _writer.WriteLine(JsonConvert.SerializeObject(result));
        }
    }
}
