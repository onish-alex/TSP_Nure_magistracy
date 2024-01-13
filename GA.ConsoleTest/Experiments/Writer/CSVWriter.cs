using GA.Core.Utility;
using System;

namespace GA.ConsoleApp.Experiments.Writer
{
    public class CSVWriter<TResearch> : FileWriter<TResearch> where TResearch : struct, IComparable<TResearch>
    {
        private bool placedHeader = false;
        private string separator;
        protected override string _extension => "csv";

        public CSVWriter(string path, string fileName, GASettings settings, GAExperimentSettings<TResearch> experimentSettings, string separator = ",")
            :base(path, fileName, settings, experimentSettings)
        {
            this.separator = separator;
        }

        public override void Write<TNode>(ExperimentResult<TNode> result)
        {
            if (!placedHeader)
            {
                var header = GetHeaderString(_experimentSettings, _settings);
                _writer.WriteLine(header);
                placedHeader = true;
            }

            var data = $"{result.IsGroupResult}{separator}{result.ResearchedParameterValue}{separator}{result.Time}{separator}{result.MinResult}{separator}{result.MaxResult}{separator}{result.AverageResult}{separator}{result.LastIterationNumber}{separator}{result.DegenerationCoefficient}";

            _writer.WriteLine(data);
        }

        private string GetHeaderString(GAExperimentSettings<TResearch> experimentSettings, GASettings settings)
        {
            var header = $"{"Group"}{separator}{experimentSettings.ResearchedParameterName}{separator}{"Time elapsed"}{separator}{"Minimum"}{separator}{"Maximum"}{separator}{"Average"}{separator}{"Iterations"}{separator}{"Degeneration coef."}";
            return header;
        }
    }
}
