using GA.Core.Models;

namespace GA.Analytics
{
	public static class PopulationHelper
	{
		public static double GetDegenerationCoefficient<TGene>(this IList<Individual<TGene>> population)
		{
			var equalIndividuals = new Dictionary<Individual<TGene>, int>();

			bool isSolutionContains;
			foreach (var individual in population)
			{
				isSolutionContains = false;

				foreach (var eqInd in equalIndividuals.Keys)
				{
					if (eqInd.SequenceEqual(individual))
					{
						equalIndividuals[eqInd]++;
						isSolutionContains = true;
						break;
					}
				}

				if (!isSolutionContains)
					equalIndividuals.Add(individual, 1);
			}

			var coef = 1 - (double)(equalIndividuals.Count - 1) / (population.Count - 1);

			return coef;
		}
	}
}
