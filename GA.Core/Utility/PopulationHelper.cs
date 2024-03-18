using GA.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GA.Core
{
	public static class PopulationHelper
	{
		public static double GetDegenerationCoefficient<TGene>(this IList<Individual<TGene>> population)
		{
			var equalIndividuals = new List<Individual<TGene>>();

			bool isSolutionContains;
			foreach (var individual in population)
			{
				isSolutionContains = false;

				foreach (var eqInd in equalIndividuals)
				{
					if (eqInd.SequenceEqual(individual))
					{
						isSolutionContains = true;
						break;
					}
				}

				if (!isSolutionContains)
					equalIndividuals.Add(individual);
			}

			var coef = 1 - (double)(equalIndividuals.Count - 1) / (population.Count - 1);

			return coef;
		}

		public static double GetDegenerationCoefficient2<TGene>(this IList<Individual<TGene>> population)
		{
			var equalIndividuals = new List<Individual<TGene>>();

			bool isSolutionContains;
			foreach (var individual in population)
			{
				isSolutionContains = false;

				foreach (var eqInd in equalIndividuals)
				{
					if (eqInd.SequenceEqual(individual))
					{
						isSolutionContains = true;
						break;
					}
				}

				if (!isSolutionContains)
					equalIndividuals.Add(individual);
			}

			var coef = 1 - (double)(Math.Pow(equalIndividuals.Count, 2) - 1) / (Math.Pow(population.Count, 2) - 1);

			return coef;
		}
	}
}
