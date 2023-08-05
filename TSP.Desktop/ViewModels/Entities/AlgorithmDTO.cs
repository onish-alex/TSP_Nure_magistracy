using TSP.Desktop.Common;
using TSP.Desktop.Models.Entities;

namespace TSP.Desktop.ViewModels.Entities
{
    public class AlgorithmDTO
    {
        public string Name { get; set; }
        public AlgorithmType Type { get; set; }

        public AntColonyAlgorithmSettings antColonySettings { get; set; }
        public GeneticAlgorithmSettings geneticSettings { get; set; }
    }
}
