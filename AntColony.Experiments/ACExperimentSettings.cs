using Algorithms.Utility.NumberWrapper;
using AntColony.Core.Utilities;
using System;

namespace AntColony.Experiments
{
	public class ACExperimentSettings<T> where T : struct, IComparable<T>
	{
		public string ResearchedParameterName { get; set; }

		public (INumber<T> Min, INumber<T> Max) ResearchedParameterRange { get; set; }

		public T ResearchedParameterIncrement { get; set; }

		public AntColonyEnum AntColonyType { get; set; }

		#region AntColonySettings_Fields

		public double InitialPheromoneAmount { get; set; }

		public double DistanceWeight { get; set; }

		public double PheromoneWeight { get; set; }

		public double EvaporationCoefficient { get; set; }

		public bool UseCommonAntPheromoneAmount { get; set; } = true;

		public double CommonAntPheromoneAmount { get; set; }

		public bool UseCommonEliteAntPheromoneAmount { get; set; } = true;

		public double CommonEliteAntPheromoneAmount { get; set; }

		public double MinPheromoneAmount { get; set; }

		public double MaxPheromoneAmount { get; set; }

		public bool UpdatePheromonesForGlobalBestWay { get; set; }
		#endregion

		#region AntPopulationSettings_Fields

		public int AntCount { get; set; }
		public int[] AntPersonalPheromoneAmounts { get; set; }

		public int EliteAntCount { get; set; }
		public int[] EliteAntPersonalPheromoneAmounts { get; set; }

		#endregion

		public int ControlRepeatingCount { get; set; } = 1;

		public int IterationsCount { get; set; } = 1;
	}
}
