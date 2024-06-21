using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using TSP.Desktop.Models.Entities;
using TSP.Desktop.ViewModels.Entities;
using TSP.Desktop.ViewModels.Mappers;

namespace TSP.Desktop.Models.Managers
{
	public class AlgorithmManager
	{
		private static AlgorithmManager Instance { get; set; }

		public Dictionary<string, Algorithm> Algorithms { get; private set; }
		public Algorithm SelectedAlgorithm { get; private set; }

		private AlgorithmManager()
		{
			Algorithms = new Dictionary<string, Algorithm>();
		}

		//public Algorithm Algorithm { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public static AlgorithmManager GetInstance()
		{
			Instance ??= new AlgorithmManager();

			return Instance;
		}

		public void CreateAlgorithm(AlgorithmDTO algoDto)
		{
			var algorithm = Mappers.Algorithm.Map<AlgorithmDTO, Algorithm>(algoDto);

			if (!Algorithms.ContainsKey(algorithm.Name))
			{
				switch (algorithm.Type)
				{
					case AlgorithmType.AntColony:

						break;

					case AlgorithmType.Genetic:

						break;
				}

				Algorithms.Add(algorithm.Name, algorithm);

				if (Algorithms.Count == 1)
				{
					SelectedAlgorithm = algorithm;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AlgorithmState.NewUnsaved)));
				}

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AlgorithmState.AlgorithmAdded)));
			}
		}

		public bool SaveAlgorithm(string fileName, string algoName)
		{
			if (!string.IsNullOrEmpty(algoName) && Algorithms.ContainsKey(algoName))
			{
				var algorithm = Algorithms[algoName];

				StreamWriter stream = null;

				try
				{
					stream = File.CreateText(fileName);
					stream.WriteLine(JsonConvert.SerializeObject(algorithm));
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

			return false;
		}

		public bool LoadAlgorithm(string fileName)
		{
			if (File.Exists(fileName))
			{
				using var stream = File.OpenText(fileName);
				try
				{
					var loadedAlgorithm = JsonConvert.DeserializeObject<Algorithm>(stream.ReadLine());

					if (Algorithms.ContainsKey(loadedAlgorithm.Name))
						loadedAlgorithm.Name += "_" + Guid.NewGuid();

					Algorithms.Add(loadedAlgorithm.Name, loadedAlgorithm);

					if (Algorithms.Count == 1)
						SelectedAlgorithm = loadedAlgorithm;
				}
				catch
				{
					stream?.Dispose();
					return false;
				}

				//PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AlgorithmState.AlgorithmLoaded)));
				return true;
			}

			return false;
		}

		public void SelectAlgorithm(string algoName)
		{
			if (!string.IsNullOrEmpty(algoName) && Algorithms.ContainsKey(algoName))
			{
				SelectedAlgorithm = Algorithms[algoName];
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AlgorithmState.AlgorithmSelected)));
			}
		}
	}
}
