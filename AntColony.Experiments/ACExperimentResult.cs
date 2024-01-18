using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AntColony.Experiments
{
	public class ACExperimentResult<TNode>
	{
		[JsonProperty("group")] public bool IsGroupResult { get; set; }
		[JsonProperty("guid")] public Guid GroupGuid { get; set; }

		[JsonIgnore][JsonProperty("routes")] public IList<IList<TNode>> FoundRoutes { get; set; }

		[JsonProperty("min")] public double MinResult { get; set; }
		[JsonProperty("max")] public double MaxResult { get; set; }
		[JsonProperty("avg")] public double AverageResult { get; set; }
		//[JsonProperty("dcoef")] public double DegenerationCoefficient { get; set; }
		//[JsonProperty("iter")] public double LastIterationNumber { get; set; }

		[JsonProperty("time")] public TimeSpan Time { get; set; }

		[JsonProperty("param")] public string ResearchedParameterName { get; set; }
		[JsonProperty("value")] public object ResearchedParameterValue { get; set; }
	}
}
