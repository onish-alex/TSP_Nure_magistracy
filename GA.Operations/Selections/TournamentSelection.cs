using Algorithms.Utility.Extensions;
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
    public class TournamentSelection : BaseSelection
    {
        //public ParentSelectingPrinciple ParentSelecting { get; set; }

        public bool IndividualCanJoinOnlyOnePair { get; set; }

        public TournamentSelection(GAOperationSettings operationSettings) : base(operationSettings)
        {
        }

        public override IList<(TIndividual, TIndividual)> GetParentPairs<TIndividual>(IDictionary<TIndividual, double> populationFitnesses)
        {
            var parentCandidates = new List<(TIndividual, double)>();
            var population = populationFitnesses.Keys.ToList();

            for (var i = 0; i < populationFitnesses.Count; i++)
            {
                var candidateIndexes = Random.Shared.GetNumbers(2, 0, populationFitnesses.Count, true);

                var firstCandidateFitness = populationFitnesses[population[candidateIndexes[0]]];
                var secondCandidateFitness = populationFitnesses[population[candidateIndexes[1]]];

                parentCandidates.Add(
                    (firstCandidateFitness > secondCandidateFitness)
                        ? (population[candidateIndexes[0]], firstCandidateFitness)
                        : (population[candidateIndexes[1]], secondCandidateFitness));
            }

            var pairsCount = populationFitnesses.Count / 2;
            var pairs = new List<(TIndividual, TIndividual)>(pairsCount);

            //switch (ParentSelecting)
            //{
            //    case ParentSelectingPrinciple.Panmixia:
                      pairs = FormPairsPanmixia(parentCandidates, pairsCount);
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

        //private List<(TIndividual, TIndividual)> FormPairsOutbreeding<TIndividual>(List<(TIndividual, double)> parentCandidates, int pairsCount)
        //{
        //    var pairs = new List<(TIndividual, TIndividual)>();

        //    while (pairs.Count < pairsCount)
        //    {
        //        var firstParent = parentCandidates[Random.Shared.Next(parentCandidates.Count)];
        //        var secondParent =
        //            parentCandidates
        //            .First(x => Math.Abs(firstParent.Item2 - x.Item2) ==
        //                        parentCandidates
        //                            .Where(z => !z.Equals(firstParent))
        //                            .Select(y => Math.Abs(firstParent.Item2 - y.Item2))
        //                            .Max()
        //                  );

        //        pairs.Add((firstParent.Item1, secondParent.Item1));

        //        if (IndividualCanJoinOnlyOnePair)
        //        {
        //            parentCandidates.Remove(firstParent);
        //            parentCandidates.Remove(secondParent);

        //            if (parentCandidates.Count < 2)
        //                break;
        //        }
        //    }

        //    return pairs;
        //}

        //private List<(TIndividual, TIndividual)> FormPairsInbreeding<TIndividual>(List<(TIndividual, double)> parentCandidates, int pairsCount)
        //{
        //    var pairs = new List<(TIndividual, TIndividual)>();

        //    while (pairs.Count < pairsCount)
        //    {
        //        var firstParent = parentCandidates[Random.Shared.Next(parentCandidates.Count)];
        //        var secondParent = 
        //            parentCandidates
        //            .First(x => Math.Abs(firstParent.Item2 - x.Item2) == 
        //                        parentCandidates
        //                            .Where(z => !z.Equals(firstParent))
        //                            .Select(y => Math.Abs(firstParent.Item2 - y.Item2))
        //                            .Min()
        //                  );

        //        pairs.Add((firstParent.Item1, secondParent.Item1));

        //        if (IndividualCanJoinOnlyOnePair)
        //        {
        //            parentCandidates.Remove(firstParent);
        //            parentCandidates.Remove(secondParent);

        //            if (parentCandidates.Count < 2)
        //                break;
        //        }
        //    }

        //    return pairs;
        //}

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
