namespace AntColony.Core.Utilities
{
    public class AntPopulationSettings
    {
        public int AntCount { get; set; }
        public int[] AntPersonalPheromoneAmounts { get; set; }

        public int EliteAntCount { get; set; }
        public int[] EliteAntPersonalPheromoneAmounts { get; set; }
    }
}
