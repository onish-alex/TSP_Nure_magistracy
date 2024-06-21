using Newtonsoft.Json;
using System;

namespace SOM.Experiments
{
	public class SOMExperimentResult
	{
		[JsonProperty("group")] public bool IsGroupResult { get; set; }
		[JsonProperty("guid")] public Guid GroupGuid { get; set; }
		[JsonProperty("length")] public double Length { get; set; }
		[JsonProperty("time")] public TimeSpan Time { get; set; }

		[JsonProperty("param")] public string ResearchedParameterName { get; set; }
		[JsonProperty("value")] public object ResearchedParameterValue { get; set; }
	}
}
