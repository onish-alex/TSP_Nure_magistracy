namespace TSP.Desktop.ViewModels.Validators.Algorithm
{
    public class AlgorithmNameValidator : IValidator<string>
    {
        public bool Validate(string entity) => !string.IsNullOrEmpty(entity);
    }
}
