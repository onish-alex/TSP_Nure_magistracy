namespace TSP.Desktop.ViewModels.Validators
{
	public interface IValidator<T>
	{
		bool Validate(T entity);
	}
}
