namespace AntColony.Core.Utilities
{
    public class AntColonySettings
    {
        /// <summary>
        /// Weight coefficient for edge distance (β)
        /// </summary>
        public double DistanceWeight { get; set; }

        /// <summary>
        /// Weight coefficient for edge pheromone amount (α) 
        /// </summary>
        public double PheromoneWeight { get; set; }


        /// <summary>
        /// Pheromone evaporation percent (0.0 - 1.0)
        /// </summary>
        public double EvaporationCoefficient { get; set; }


        /// <summary>
        /// Defines if the same pheromone amount will be used for all ants. Otherwise, set up personal amounts in <c>AntPopulationSettings</c>
        /// </summary>
        public bool UseCommonAntPheromoneAmount { get; set; } = true;

        /// <summary>
        /// Defines pheromone amount that will be used by all ants if UseCommonAntPheromoneAmount is "true"
        /// </summary>
        public double CommonAntPheromoneAmount { get; set; }

        /// <summary>
        /// Defines if the same pheromone amount will be used for all <c>elite</c> ants. Otherwise, set up personal amounts in <c>AntPopulationSettings</c>
        /// </summary>
        public bool UseCommonEliteAntPheromoneAmount { get; set; } = true;

        /// <summary>
        /// Defines pheromone amount that will be used by all <c>elite</c> ants if UseCommonEliteAntPheromoneAmount is "true"
        /// </summary>
        public double CommonEliteAntPheromoneAmount { get; set; }
    }
}
