using System.Configuration;

namespace TSP.Desktop.ViewModels.Validators.Algorithm
{
	public class GeneticAlgorithmPopulationSizeValidator : IValidator<string>
	{
		public int MinValue { get; private set; }
		public int MaxValue { get; private set; }

		public GeneticAlgorithmPopulationSizeValidator()
		{
			MinValue = int.Parse(ConfigurationManager.AppSettings["GeneticAlgorithmMinPopulationSize"]);
			MaxValue = int.Parse(ConfigurationManager.AppSettings["GeneticAlgorithmMaxPopulationSize"]);
		}

		public bool Validate(string entity)
		{
			return int.TryParse(entity, out int populationSize)
				&& populationSize >= MinValue && populationSize <= MaxValue;
		}
	}
}
