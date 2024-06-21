using System;
using System.Collections.Generic;
using TSP.Desktop.Common;

namespace TSP.Desktop.ViewModels.Algorithms
{
	public class SetupGeneticAlgorithmViewModel
	{
		public IEnumerable<CrossoverType> CrossoverTypes { get; set; }
		public IEnumerable<SelectionType> SelectionTypes { get; set; }
		public IEnumerable<MutationType> MutationTypes { get; set; }

		public bool IsUseElite { get; set; }

		public GeneticAlgorithmSettings AlgorithmSettings { get; private set; }

		public SetupGeneticAlgorithmViewModel()
		{
			this.CrossoverTypes = Enum.GetValues<CrossoverType>();
			this.SelectionTypes = Enum.GetValues<SelectionType>();
			this.MutationTypes = Enum.GetValues<MutationType>();

			this.AlgorithmSettings = new GeneticAlgorithmSettings();
			this.AlgorithmSettings.GASettings = new GA.Core.Utility.GASettings();
		}

		public void SetSettings(GeneticAlgorithmSettings geneticAlgorithmSettings)
		{
			this.AlgorithmSettings.SelectionType = geneticAlgorithmSettings.SelectionType;
			this.AlgorithmSettings.CrossoverType = geneticAlgorithmSettings.CrossoverType;
			this.AlgorithmSettings.MutationType = geneticAlgorithmSettings.MutationType;

			this.AlgorithmSettings.GASettings.MutationProbability = geneticAlgorithmSettings.GASettings.MutationProbability;
			this.AlgorithmSettings.GASettings.OnlyChildrenInNewGeneration = geneticAlgorithmSettings.GASettings.OnlyChildrenInNewGeneration;
			this.AlgorithmSettings.GASettings.ElitePercent = geneticAlgorithmSettings.GASettings.ElitePercent;
		}
	}
}
