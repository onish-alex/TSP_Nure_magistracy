﻿using Algorithms.Utility.Extensions;
using Algorithms.Utility;
using GA.Core.Models;
using GA.Core.Operations.Selections;
using GA.Core.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Operations.Selections
{
    /*
     Панмиксия — оба родителя выбираются случайно, каждая особь популяции имеет равные шансы быть выбранной
     Инбридинг — первый родитель выбирается случайно, а вторым выбирается такой, который наиболее похож на первого родителя
     Аутбридинг — первый родитель выбирается случайно, а вторым выбирается такой, который наименее похож на первого родителя
     */

    public class TournamentSelection : BaseSelection
    {
        //public ParentSelectingPrinciple ParentSelecting { get; set; }

        public bool IndividualCanJoinOnlyOnePair { get; set; }
        public int TournamentDimension { get; set; } = 2;

        public TournamentSelection(GAOperationSettings operationSettings) : base(operationSettings)
        {
        }

        public override IList<(TIndividual, TIndividual)> GetParentPairs<TIndividual>(IDictionary<TIndividual, double> populationFitnesses)
        {
            //var parentCandidates = new List<(TIndividual, double)>();
            var population = populationFitnesses.Keys.ToList();

            var pairsCount = population.Count / 2;
            var pairs = new List<(TIndividual, TIndividual)>(pairsCount);

            for (var i = 0; i < pairsCount; i++)
            {
                var firstParent = GetTournamentResult(populationFitnesses, population);
                var secondParent = GetTournamentResult(populationFitnesses, population);

                pairs.Add((firstParent, secondParent));
            }

            //var pairsCount = populationFitnesses.Count / 2;
            //var pairs = new List<(TIndividual, TIndividual)>(pairsCount);

            //switch (ParentSelecting)
            //{
            //    case ParentSelectingPrinciple.Panmixia:
            //        pairs = FormPairsPanmixia(parentCandidates, pairsCount);
            //        break;

            //    case ParentSelectingPrinciple.Inbreeding:
            //        pairs = FormPairsInbreeding(parentCandidates, pairsCount);
            //        break;

            //    case ParentSelectingPrinciple.Outbreeding:
            //        pairs = FormPairsOutbreeding(parentCandidates, pairsCount);
            //        break;
            //}

            return pairs;
        }

        private TIndividual GetTournamentResult<TIndividual>(
            IDictionary<TIndividual, double> populationFitnesses, 
            IList<TIndividual> population,
            int dimension = 2)
        {
            if (dimension < 2)
                throw new ArgumentException($"Parameter {nameof(dimension)} should be greater or equal 2");

            var candidateIndexes = Random.Shared.GetNumbers(dimension, 0, population.Count, true);

            var candidates = new List<TIndividual>();

            foreach (var candidateIndex in candidateIndexes)
                candidates.Add(population[candidateIndex]);

            var winner = candidates.OrderByDescending(x => populationFitnesses[x]).FirstOrDefault();

            return winner;
        }

        private List<(TIndividual, TIndividual)> FormPairsOutbreeding<TIndividual>(List<(TIndividual, double)> parentCandidates, int pairsCount)
        {
            var pairs = new List<(TIndividual, TIndividual)>();

            while (pairs.Count < pairsCount)
            {
                var firstParent = parentCandidates[Random.Shared.Next(parentCandidates.Count)];
                var secondParent = parentCandidates
                    .Except(new List<(TIndividual, double)>() { firstParent })
                    .OrderByDescending(x => Math.Abs(firstParent.Item2 - x.Item2))
                    .First();

                pairs.Add((firstParent.Item1, secondParent.Item1));

                if (IndividualCanJoinOnlyOnePair)
                {
                    parentCandidates.Remove(firstParent);
                    parentCandidates.Remove(secondParent);

                    if (parentCandidates.Count < 2)
                        break;
                }
            }

            return pairs;
        }

        private List<(TIndividual, TIndividual)> FormPairsInbreeding<TIndividual>(List<(TIndividual, double)> parentCandidates, int pairsCount)
        {
            var pairs = new List<(TIndividual, TIndividual)>();

            while (pairs.Count < pairsCount)
            {
                var firstParent = parentCandidates[Random.Shared.Next(parentCandidates.Count)];
                var secondParent = parentCandidates
                    .Except(new List<(TIndividual, double)>() { firstParent })
                    .OrderBy(x => Math.Abs(firstParent.Item2 - x.Item2))
                    .First();

                pairs.Add((firstParent.Item1, secondParent.Item1));

                if (IndividualCanJoinOnlyOnePair)
                {
                    parentCandidates.Remove(firstParent);
                    parentCandidates.Remove(secondParent);

                    if (parentCandidates.Count < 2)
                        break;
                }
            }

            return pairs;
        }

        private List<(TIndividual, TIndividual)> FormPairsPanmixia<TIndividual>(List<(TIndividual, double)> parentCandidates, int pairsCount)
        {
            var pairs = new List<(TIndividual, TIndividual)>();

            for (int i = 0; i < pairsCount; i++)
            {
                var parentIndexes = Random.Shared.GetNumbers(2, 0, parentCandidates.Count, true);

                var firstParent = parentCandidates[parentIndexes[0]];
                var secondParent = parentCandidates[parentIndexes[1]];

                pairs.Add((firstParent.Item1, secondParent.Item1));

                if (IndividualCanJoinOnlyOnePair)
                {
                    parentCandidates.Remove(firstParent);
                    parentCandidates.Remove(secondParent);

                    if (parentCandidates.Count < 2)
                        break;
                }
            }

            return pairs;
        }

        protected override void InitSettings()
        {
            //var parentSelectingValues = Enum.GetValues<ParentSelectingPrinciple>().Select(x => (int)x);
            //ParentSelecting = (ParentSelectingPrinciple)Random.Shared.Next(0, parentSelectingValues.Max() + 1);
        }
    }
}
