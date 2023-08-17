using Newtonsoft.Json;

namespace AntColony.Core.Utilities
{
    public class AntColonySettings
    {
        /// <summary>
        /// Initial pheromone amount on each edge, 0 by default. 
        /// </summary>
        public double InitialPheromoneAmount { get; set; }

        /// <summary>
        /// Weight coefficient for edge distance (β)
        /// </summary>
        [JsonProperty("beta")]
        public double DistanceWeight { get; set; }

        /// <summary>
        /// Weight coefficient for edge pheromone amount (α) 
        /// </summary>
        [JsonProperty("alpha")]
        public double PheromoneWeight { get; set; }

        /// <summary>
        /// Pheromone evaporation percent (0.0 - 1.0)
        /// </summary>
        [JsonProperty("evaporation")]
        public double EvaporationCoefficient { get; set; }

        /// <summary>
        /// Defines if the same pheromone amount will be used for all ants. Otherwise, set up personal amounts in <c>AntPopulationSettings</c>
        /// </summary>
        [JsonProperty("is_common_pheromone")]
        public bool UseCommonAntPheromoneAmount { get; set; } = true;

        /// <summary>
        /// Defines pheromone amount that will be used by all ants if UseCommonAntPheromoneAmount is "true"
        /// </summary>
        [JsonProperty("common_pheromone")]
        public double CommonAntPheromoneAmount { get; set; }

        /// <summary>
        /// Defines if the same pheromone amount will be used for all <c>elite</c> ants. Otherwise, set up personal amounts in <c>AntPopulationSettings</c>
        /// </summary>
        [JsonProperty("is_elite_common_pheromone")]
        public bool UseCommonEliteAntPheromoneAmount { get; set; } = true;

        /// <summary>
        /// Defines pheromone amount that will be used by all <c>elite</c> ants if UseCommonEliteAntPheromoneAmount is "true"
        /// </summary>
        [JsonProperty("elite_common_pheromone")]
        public double CommonEliteAntPheromoneAmount { get; set; }

        #region MAXMIN

        /// <summary>
        /// Defines minimum pheromone amount on edges, for MAX-MIN only
        /// </summary>
        public double MinPheromoneAmount { get; set; }

        /// <summary>
        /// Defines maximum pheromone amount on edges, for MAX-MIN only
        /// </summary>
        public double MaxPheromoneAmount { get; set; }
        
        /// <summary>
        /// Defines if pheromone growing must perform for global-best way, or for current iteration best way. For MAX-MIN only
        /// </summary>
        public bool UpdatePheromonesForGlobalBestWay { get; set; }

        #endregion
    }
}
