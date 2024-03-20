using Algorithms.Utility.NumberWrapper;
using System;

namespace SOM.Experiments
{
	public class SOMExperimentSettings<T> where T : struct, IComparable<T>
	{
		public string ResearchedParameterName { get; set; }

		public (INumber<T> Min, INumber<T> Max) ResearchedParameterRange { get; set; }

		public T ResearchedParameterIncrement { get; set; }

		public int ControlRepeatingCount { get; set; } = 1;
	}
}
