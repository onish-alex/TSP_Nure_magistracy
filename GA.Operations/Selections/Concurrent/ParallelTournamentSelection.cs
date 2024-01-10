using Algorithms.Utility.Extensions;
using GA.Core.Operations;
using GA.Core.Operations.Selections;
using GA.Core.Operations.Selections.Concurrent;
using GA.Core.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GA.Operations.Selections.Concurrent
{
    public class ParallelTournamentSelection : ParallelBaseSelection
    {
        private readonly object _lock = new object();

        public bool IndividualCanJoinOnlyOnePair { get; set; }

        public ParallelTournamentSelection(GAOperationSettings operationSettings) : base(operationSettings)
        {
        }

        public override IList<(TIndividual, TIndividual)> GetParentPairs<TIndividual>(IDictionary<TIndividual, double> populationFitnesses)
        {
            //var parentCandidates = new ConcurrentDictionary<int, (TIndividual, double)>();
            //var parentCandidates = new List<(TIndividual, double)>(populationFitnesses.Count);
            //var population = populationFitnesses.Keys.ToList();

            //Parallel.For(0, populationFitnesses.Count, parallelOptions, (i) =>
            //{
            //    var candidateIndexes = Random.Shared.GetNumbers(2, 0, populationFitnesses.Count, true);

            //    var firstCandidateFitness = populationFitnesses[population[candidateIndexes[0]]];
            //    var secondCandidateFitness = populationFitnesses[population[candidateIndexes[1]]];

            //    //parentCandidates.TryAdd(i, 
            //    //    (firstCandidateFitness > secondCandidateFitness)
            //    //        ? (population[firstCandidateIndex], firstCandidateFitness)
            //    //        : (population[secondCandidateIndex], secondCandidateFitness));

            //    //lock (_lock)
            //    //{
            //        parentCandidates.Add(
            //            (firstCandidateFitness > secondCandidateFitness)
            //                ? (population[candidateIndexes[0]], firstCandidateFitness)
            //                : (population[candidateIndexes[1]], secondCandidateFitness));
            //    //}
            //});

            //var pairsCount = populationFitnesses.Count / 2;
            //var pairs = FormPairsPanmixia(parentCandidates, pairsCount);

            var population = populationFitnesses.Keys.ToList();

            var pairsCount = population.Count / 2;
            var pairs = new List<(TIndividual, TIndividual)>(pairsCount);

            Parallel.For(0, population.Count, parallelOptions, (i) =>
            {
                var firstParent = GetTournamentResult(populationFitnesses, population);
                var secondParent = GetTournamentResult(populationFitnesses, population);

                pairs.Add((firstParent, secondParent));
            });

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

            var candidates = new ConcurrentBag<TIndividual>();

            //foreach (var candidateIndex in candidateIndexes)
            Parallel.ForEach(candidateIndexes, (x) =>
            {
                candidates.Add(population[x]);
            });

            var winner = candidates.OrderByDescending(x => populationFitnesses[x]).FirstOrDefault();

            return winner;
        }

        //private List<(TIndividual, TIndividual)> FormPairsPanmixia<TIndividual>(ConcurrentDictionary<int, (TIndividual, double)> parentCandidates, int pairsCount)
        //private List<(TIndividual, TIndividual)> FormPairsPanmixia<TIndividual>(List<(TIndividual, double)> parentCandidates, int pairsCount)
        //{
        //    //var pairs = new ConcurrentDictionary<int, (TIndividual, TIndividual)>();
        //    var pairs = new List<(TIndividual, TIndividual)>(pairsCount);

        //    Parallel.For(0, pairsCount, parallelOptions, (i, state) =>
        //    {
        //        var parentIndexes = Random.Shared.GetNumbers(2, 0, parentCandidates.Count, true);

        //        var firstParent = parentCandidates[parentIndexes[0]];
        //        var secondParent = parentCandidates[parentIndexes[1]];

        //        //lock (_lock)
        //        //{
        //            //pairs.TryAdd(i, (firstParent.Item1, secondParent.Item1));
        //            pairs.Add((firstParent.Item1, secondParent.Item1));

        //            if (IndividualCanJoinOnlyOnePair)
        //            {
        //                //parentCandidates.TryRemove(i, out firstParent);
        //                //parentCandidates.TryRemove(i, out secondParent);
        //                parentCandidates.Remove(firstParent);
        //                parentCandidates.Remove(secondParent);

        //                if (parentCandidates.Count < 2)
        //                    state.Break();
        //            }
        //        //}
        //    });

        //    //return pairs.Select(x => x.Value).ToList();
        //    return pairs;
        //}

        protected override void InitSettings()
        {
        }
    }
}
