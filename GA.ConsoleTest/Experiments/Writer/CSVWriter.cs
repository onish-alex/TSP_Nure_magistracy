using GA.Core.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.ConsoleApp.Experiments.Writer
{
    public class CSVWriter<TResearch> : IExperimentResultWriter<TResearch>, IDisposable where TResearch : struct, IComparable<TResearch>
    {
        private StreamWriter _writer;
        private FileStream _stream;
        private bool disposedValue;
        private bool placedHeader = false;
        private string separator;

        private GASettings _settings;
        private GAExperimentSettings<TResearch> _experimentSettings;

        internal CSVWriter(string path, string separator, GASettings settings, GAExperimentSettings<TResearch> experimentSettings)
        {
            _stream = File.OpenWrite(path);
            _writer = new StreamWriter(_stream);
            _settings = settings;
            _experimentSettings = experimentSettings;
            this.separator = separator;
        }

        void IExperimentResultWriter<TResearch>.Write<TNode>(ExperimentResult<TNode> result)
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
            var header = $"{"Group"}{separator}{experimentSettings.ResearchedParameterName(settings)}{separator}{"Time elapsed"}{separator}{"Minimum"}{separator}{"Maximum"}{separator}{"Average"}{separator}{"Iterations"}{separator}{"Degeneration coef."}";
            return header;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _writer.Dispose();
                    _stream.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~CSVWriter()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
