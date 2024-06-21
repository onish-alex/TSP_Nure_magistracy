using Algorithms.Utility.NumberWrapper;
using System;

namespace GA.Experiments
{
	public class GAExperimentSettings<T> where T : struct, IComparable<T>
	{
		public string ResearchedParameterName { get; set; }

		public (INumber<T> Min, INumber<T> Max) ResearchedParameterRange { get; set; }

		public T ResearchedParameterStep { get; set; }

		public int PopulationSize { get; set; }

		//use the same population for each iteration after changing researched parameter
		public bool UseSameInitialPopulation { get; set; }

		public int ControlRepeatingCount { get; set; } = 1;
	}
}
