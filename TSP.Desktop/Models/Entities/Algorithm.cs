using GA.Core;
using TSP.Core;
using TSP.Desktop.Common;

namespace TSP.Desktop.Models.Entities
{
	public class Algorithm
	{
		public string Name { get; set; }

		public AlgorithmType Type { get; set; }

		public GeneticAlgorithm<TSPNode> geneticAlgorithm { get; set; }
		public AntColony.Core.BaseAlgorithm<TSPNode> antColonyAlgorithm { get; set; }

		public AntColonyAlgorithmSettings antColonySettings { get; set; }
		public GeneticAlgorithmSettings geneticSettings { get; set; }
	}

	public enum AlgorithmType
	{
		AntColony,
		Genetic
	}

	public enum AlgorithmState
	{
		NewUnsaved,
		AlgorithmSelected,
		AlgorithmSaved,
		AlgorithmAdded,
	}
}
