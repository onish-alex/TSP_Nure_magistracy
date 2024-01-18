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

		public int GenerationsMaxCount { get; set; } = 1;

		public int? StagnatingGenerationsLimit { get; set; } = null;

		public double? DegenerationMaxPercent { get; set; } = null;

		#region ForExperimentsOnly

		public CrossoversEnum CrossoverType { get; set; }

		public MutationsEnum MutationsType { get; set; }

		public SelectionsEnum SelectionType { get; set; }

		public GAOperationSettings SelectionSettings { get; set; }
		public GAOperationSettings CrossoverSettings { get; set; }
		public GAOperationSettings MutationSettings { get; set; }

		#endregion
	}
}
