namespace AntColony.Core.Utilities
{
    public class AntColonySettings
    {
        public bool UseCommonWeights { get; set; } = true;
        public double DistanceWeight { get; set; }
        public double PheromoneWeight { get; set; }

        public bool UseCommonEvaporation { get; set; } = true;
        public double EvaporationCoefficient { get; set; }

        public bool UseSymmetricDistances { get; set; } = true;
    }
}
