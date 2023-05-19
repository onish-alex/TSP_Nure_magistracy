namespace GA.Core.Utility
{
	public class GASettings
	{

		/// <summary>
		/// probability of mutation for each generation individual. Can take values from 0 to 100. <c>Zero by default</c>
		/// </summary>
		public double MutationProbability { get; set; } = 0;

		/// <summary>
		/// percent of individuals with best fitness that may form parent pairs. Set <c>null</c> to allow selection for all individuals
		/// </summary>
		public double? ElitePercent { get; set; } = null;

		/// <summary>
		/// if <c>true</c>, new generation include only new individuals (children). Otherwise, generation will include concatenation of best (by fitness) parents and children
		/// </summary>
		public bool OnlyChildrenInNewGeneration { get; set; } = false;
	}
}
