using GA.Core.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace GA.Experiments.Writer
{
    public class ConsoleWriter<TResearch> : IExperimentResultWriter<TResearch> where TResearch : struct, IComparable<TResearch>
    {
        protected GASettings _settings;
        protected GAExperimentSettings<TResearch> _experimentSettings;
        private bool placedHeader = false;

        public ConsoleWriter(GASettings settings, GAExperimentSettings<TResearch> experimentSettings)
        {
            _settings = settings;
            _experimentSettings = experimentSettings;
        }

        public void Write<TNode>(ExperimentResult<TNode> result)
        {
            if (!placedHeader)
            {
                var header = GetHeaderString(_experimentSettings, _settings);
                System.Console.WriteLine(header);
                placedHeader = true;
            }

            Console.WriteLine(
                $"| {result.IsGroupResult,10} " +
                $"| {result.ResearchedParameterValue,20} " +
                $"| {result.Time,19} " +
                $"| {Math.Round(result.MinResult, 2),9} " +
                $"| {Math.Round(result.MaxResult, 2),9} " +
                $"| {Math.Round(result.AverageResult, 2),9} " +
                $"| {result.LastIterationNumber,12} " +
                $"| {Math.Round(result.DegenerationCoefficient, 2).ToString() + "%",17}");
        }

        private string GetHeaderString(GAExperimentSettings<TResearch> experimentSettings, GASettings settings)
        {
            var header = $"| {"Group",10} | {experimentSettings.ResearchedParameterName,20} | {"Time elapsed",19} | {"Minimum",9} | {"Maximum",9} | {"Average",9} | {"Iterations",12} | {"Degeneration coef.",17}";
            return header;
        }
    }
}
