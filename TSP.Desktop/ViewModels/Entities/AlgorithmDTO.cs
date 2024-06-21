using TSP.Desktop.Common;
using TSP.Desktop.Models.Entities;

namespace TSP.Desktop.ViewModels.Entities
{
	public class AlgorithmDTO
	{
		public string Name { get; set; }
		public AlgorithmType Type { get; set; }

		public AntColonyAlgorithmSettings AntColonySettings { get; set; }
		public GeneticAlgorithmSettings GeneticSettings { get; set; }
	}
}
