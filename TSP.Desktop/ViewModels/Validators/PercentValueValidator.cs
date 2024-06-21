using System.Globalization;

namespace TSP.Desktop.ViewModels.Validators
{
	public class PercentValueValidator : IValidator<string>
	{
		public PercentValueValidator()
		{
			//var culture = CultureInfo.)
		}

		public bool Validate(string entity)
		{
			return double.TryParse(entity, NumberStyles.Float, CultureInfo.InvariantCulture, out double percent)
				&& percent >= 0d && percent <= 100d;
		}
	}
}
