using Newtonsoft.Json;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using TSP.Core;
using TSP.Desktop.Models.Entities;
using TSP.Desktop.ViewModels.Entities;
using TSP.Desktop.ViewModels.Mappers;

namespace TSP.Desktop.Models.Managers
{
	public class MapManager : INotifyPropertyChanged
	{
		private static MapManager Instance { get; set; }

		private MapManager()
		{
		}

		public Map Map { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public static MapManager GetInstance()
		{
			Instance ??= new MapManager();

			return Instance;
		}

		public void CreateMap(MapDTO mapDto)
		{
			Instance.Map = Mappers.TSPMap.Map<MapDTO, Map>(mapDto);

			var minX = int.Parse(ConfigurationManager.AppSettings["MinMapXCoord"]);
			var maxX = int.Parse(ConfigurationManager.AppSettings["MaxMapXCoord"]);
			var minY = int.Parse(ConfigurationManager.AppSettings["MinMapYCoord"]);
			var maxY = int.Parse(ConfigurationManager.AppSettings["MaxMapYCoord"]);

			Instance.Map.TSPModel = TSPModelGenerator.GetNewModel(
				Instance.Map.NodeCount,
				(minX, maxX), (minY, maxY));

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MapState.MapCreated)));
		}

		public bool SaveMap(string fileName)
		{
			StreamWriter stream = null;

			try
			{
				stream = File.CreateText(fileName);
				stream.WriteLine(JsonConvert.SerializeObject(Map));
				stream?.Dispose();
			}
			catch
			{
				stream?.Dispose();
				return false;
			}

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MapState.MapSaved)));
			return true;
		}

		public bool LoadMap(string fileName)
		{
			if (File.Exists(fileName))
			{
				using var stream = File.OpenText(fileName);
				try
				{
					Map = JsonConvert.DeserializeObject<Map>(stream.ReadLine());
				}
				catch
				{
					stream?.Dispose();
					return false;
				}

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MapState.MapLoaded)));
				return true;
			}
			return false;
		}
	}
}
