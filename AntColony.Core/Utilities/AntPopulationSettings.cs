using Newtonsoft.Json;

namespace AntColony.Core.Utilities
{
    public class AntPopulationSettings
    {
        [JsonProperty("ants")]
        public int AntCount { get; set; }
        public int[] AntPersonalPheromoneAmounts { get; set; }

        [JsonProperty("elite_ants")]
        public int EliteAntCount { get; set; }
        public int[] EliteAntPersonalPheromoneAmounts { get; set; }
    }
}
