using AntColony.Core.Utilities;

namespace TSP.Desktop.Common
{
    public class AntColonyAlgorithmSettings
    {
        public AntColonyAlgorithmType Type { get; set; }

        public AntColonySettings AntColonySettings { get; set; }

        public AntPopulationSettings AntPopulationSettings { get; set; }
    }

    public enum AntColonyAlgorithmType
    {
        Classic,
        ParallelClassic,
        Elitist,
        ParallelElitist,
        MaxMin,
        ParallelMaxMin
    }
}