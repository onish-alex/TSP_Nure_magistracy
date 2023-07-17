using System.Configuration;

namespace TSP.Desktop.ViewModels.Validators
{
	public class MapNodeCountValidator : IValidator<string>
	{
		public int MinValue { get; private set; } 
		public int MaxValue { get; private set; } 

		public MapNodeCountValidator()
		{
			MinValue = int.Parse(ConfigurationManager.AppSettings["MinMapNodes"]);
			MaxValue = int.Parse(ConfigurationManager.AppSettings["MaxMapNodes"]);
		}

		public bool Validate(string entity)
		{
			return int.TryParse(entity, out int nodeAmount)
				&& (nodeAmount >= MinValue && nodeAmount <= MaxValue);
		}
	}
}
