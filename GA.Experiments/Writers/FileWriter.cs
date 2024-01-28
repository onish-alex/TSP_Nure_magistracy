using GA.Core.Utility;
using System;
using System.IO;

namespace GA.Experiments.Writer
{
	public abstract class FileWriter<TResearch> : IExperimentResultWriter<TResearch>, IDisposable where TResearch : struct, IComparable<TResearch>
	{
		protected string _path;
		protected string _fileName;
		protected string _fullFilePath;
		//protected StreamWriter _writer;
		//protected FileStream _stream;
		protected bool disposedValue;

		protected GASettings _settings;
		protected GAExperimentSettings<TResearch> _experimentSettings;

		protected abstract string _extension { get; }

		protected FileWriter(string path, string fileName, GASettings settings, GAExperimentSettings<TResearch> experimentSettings)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));

			if (string.IsNullOrEmpty(fileName))
				throw new ArgumentNullException(nameof(fileName));

			_path = path;
			_fileName = fileName;
			_fullFilePath = Path.Combine(path, $"{fileName}.{_extension}");

			//_stream = File.OpenWrite(_fullFilePath);
			//_writer = new StreamWriter(_stream);

			_settings = settings;
			_experimentSettings = experimentSettings;
		}

		public abstract void Write<TNode>(GAExperimentResult<TNode> result);

		public abstract void WriteLog(Exception ex);

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					//_writer.Close();
					//_stream.Close();
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
