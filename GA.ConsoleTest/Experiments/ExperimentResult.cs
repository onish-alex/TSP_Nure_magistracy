using GA.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GA.ConsoleApp.Experiments
{
    public class ExperimentResult<TNode>
    {
        [JsonProperty("group")] public bool IsGroupResult { get; set; }
        [JsonProperty("guid")] public Guid GroupGuid { get; set; }

        [JsonProperty("spop")] public IList<Individual<TNode>> StartPopulation { get; set; }
        [JsonProperty("fpop")] public IList<Individual<TNode>> FinishPopulation { get; set; }

        [JsonProperty("min")] public double MinResult { get; set; }
        [JsonProperty("max")] public double MaxResult { get; set; }
        [JsonProperty("avg")] public double AverageResult { get; set; }
        [JsonProperty("dcoef")] public double DegenerationCoefficient { get; set; }
        [JsonProperty("iter")] public double LastIterationNumber { get; set; }

        [JsonProperty("time")] public TimeSpan Time { get; set; }

        [JsonProperty("param")] public string ResearchedParameterName { get; set; }
        [JsonProperty("value")] public object ResearchedParameterValue { get; set; }
    }
}
