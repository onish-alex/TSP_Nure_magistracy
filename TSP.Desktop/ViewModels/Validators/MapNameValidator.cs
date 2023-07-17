namespace TSP.Desktop.ViewModels.Validators
{
	public class MapNameValidator : IValidator<string>
	{
		public bool Validate(string entity) => !string.IsNullOrEmpty(entity);
	}
}
