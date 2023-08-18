using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSP.Core;
using TSP.Desktop.Models.Entities;
using TSP.Desktop.ViewModels.Entities;
using TSP.Desktop.ViewModels.Mappers;

namespace TSP.Desktop.Models.Managers
{
	public class AlgorithmManager
	{
		private static AlgorithmManager Instance { get; set; }

		private AlgorithmManager()
		{
		}

		public Algorithm Algorithm { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public static AlgorithmManager GetInstance()
		{
			Instance ??= new AlgorithmManager();

			return Instance;
		}

		public void CreateAlgorithm(AlgorithmDTO algoDto)
		{
			Instance.Algorithm = Mappers.Algorithm.Map<AlgorithmDTO, Algorithm>(algoDto);

			switch (Instance.Algorithm.Type)
			{
				case AlgorithmType.AntColony:
					
					break;

				case AlgorithmType.Genetic:

					break;
			}

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AlgorithmState.AlgorithmCreated)));
		}

		public bool SaveAlgorithm(string fileName)
		{
			StreamWriter stream = null;

			try
			{
				stream = File.CreateText(fileName);
				stream.WriteLine(JsonConvert.SerializeObject(Algorithm));
				stream?.Dispose();
			}
			catch
			{
				stream?.Dispose();
				return false;
			}

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AlgorithmState.AlgorithmSaved)));
			return true;
		}

		public bool LoadAlgorithm(string fileName)
		{
			if (File.Exists(fileName))
			{
				using var stream = File.OpenText(fileName);
				try
				{
					Algorithm = JsonConvert.DeserializeObject<Algorithm>(stream.ReadLine());
				}
				catch
				{
					stream?.Dispose();
					return false;
				}

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AlgorithmState.AlgorithmLoaded)));
				return true;
			}
			return false;
		}
	}
}
