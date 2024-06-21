using TSP.Core;

namespace TSP.Desktop.Models.Entities
{
	public class Map
	{
		public string Name { get; set; }
		public int NodeCount { get; set; }

		public TSPModel TSPModel { get; set; }
	}

	public enum MapState
	{
		MapCreated,
		MapLoaded,
		MapSaved
	}
}
