using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Core.Extensions;
using GA.Core.Models;
using GA.Core.Operations.Mutations;

namespace GA.Operations.Mutations
{
	public class SwapMutation : BaseMutation
	{
		private int swapSectionLength = 1;
		public int SwapSectionLength
		{
			get => swapSectionLength;
			set => swapSectionLength = value;
		}

		public override void ProcessMutation<TIndividual, TGene>(IList<TIndividual> population, double probability)
		{
			for (var i = 0; i < population.Count; i++)
			{
				if (Random.CheckProbability(probability))
				{
					//TODO: also handle values less than 1
					if (swapSectionLength > population[i].Count / 2)
						swapSectionLength = population[i].Count / 2;

					//var individual = population[i].ToList();

					var firstSectionIndex = Random.Next(0, population[i].Count - swapSectionLength + 1);
					var firstRange = population[i].GetRange(firstSectionIndex, swapSectionLength);
					population[i].RemoveRange(firstSectionIndex, swapSectionLength);

					var secondSectionIndex = Random.Next(0, population[i].Count - swapSectionLength + 1);
					var secondRange = population[i].GetRange(secondSectionIndex, swapSectionLength);
					population[i].RemoveRange(secondSectionIndex, swapSectionLength);

					for (var j = 0; j < swapSectionLength; j++)
						if (firstSectionIndex + j > population[i].Count - 1)
							population[i].Add(secondRange[j]);
						else
							population[i].Insert(firstSectionIndex + j, secondRange[j]);

					for (var j = 0; j < swapSectionLength; j++)
						if (secondSectionIndex + j > population[i].Count - 1)
							population[i].Add(firstRange[j]);
						else
							population[i].Insert(secondSectionIndex + j, firstRange[j]);
				}
			}
		}
	}
}
