using GA.Core.Utility;
using System;
using System.IO;

namespace GA.ConsoleApp.Experiments.Writer
{
    public abstract class FileWriter<TResearch> : IExperimentResultWriter<TResearch>, IDisposable where TResearch : struct, IComparable<TResearch>
    {
        protected StreamWriter _writer;
        protected FileStream _stream;
        private bool disposedValue;
        
        protected GASettings _settings;
        protected GAExperimentSettings<TResearch> _experimentSettings;

        protected abstract string _extension { get; } 

        protected FileWriter(string path, string fileName, GASettings settings, GAExperimentSettings<TResearch> experimentSettings)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            _stream = File.OpenWrite(Path.Combine(path, $"{fileName}.{_extension}"));
            _writer = new StreamWriter(_stream);
            _settings = settings;
            _experimentSettings = experimentSettings;
        }

        public abstract void Write<TNode>(ExperimentResult<TNode> result); 

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
