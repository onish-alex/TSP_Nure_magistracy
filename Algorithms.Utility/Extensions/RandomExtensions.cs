using System;
using System.Collections.Generic;

namespace Algorithms.Utility.Extensions
{
    public static class RandomExtensions
    {
        /// <summary>
        /// returns randomly shuffled collection from specified collection of unique items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rand"></param>
        /// <param name="set">collection with unique elements</param>
        /// <param name="size">size of result collection</param>
        /// <returns></returns>
        public static IList<T> GetUniqueRandomSet<T>(this Random rand, IList<T> set, int size)
        {
            var localSet = new List<T>(set);
            var randSet = new List<T>();

            if (size > localSet.Count)
                size = localSet.Count;

            int index;

            for (int i = 0; i < size; i++)
            {
                index = rand.Next(0, localSet.Count);
                randSet.Add(localSet[index]);
                localSet.RemoveAt(index);
            }

            return randSet;
        }

        /// <summary>
        /// returns true with specified probability
        /// </summary>
        /// <param name="rand"></param>
        /// <param name="probability">probability of positive result</param>
        /// <returns></returns>
        public static bool CheckProbability(this Random rand, double probability)
        {
            if (probability < 0D || probability > 100D)
                throw new ArgumentException("\"probability\" argument value out of range");

            if (probability == 0D)
                return false;

            if (probability == 100D)
                return true;

            var number = rand.NextDouble();

            if (number <= probability / 100)
                return true;

            return false;
        }

        /// <summary>
        /// returns collection of random pairs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rand"></param>
        /// <param name="set">source of elements for pairs</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<(T, T)> GetPairs<T>(this Random rand, IList<T> set, int count)
        {
            var pairs = new List<(T firstParent, T secondParent)>();

            for (int i = 0; i < count; i++)
            {
                var firstParentIndex = rand.Next(0, set.Count);
                var firstParent = set[firstParentIndex];

                set.Remove(set[firstParentIndex]);

                var secondParentIndex = rand.Next(0, set.Count);
                var secondParent = set[secondParentIndex];

                set.Add(firstParent);

                pairs.Add((firstParent, secondParent));
            }

            return pairs;
        }

        public static int[] GetNumbers(this Random rand, int count, int minValue, int maxValue, bool unique)
        {
            var result = new int[count];

            if (!unique)
            {
                for (int i = 0; i < count; i++)
                    result[i] = rand.Next(minValue, maxValue);
            }
            else
            {
                int needed = count;
                int remaining = maxValue - minValue;
                int taken = 0;

                for (int i = 0; i < maxValue - minValue; i++)
                {
                    if (taken == count)
                        break;

                    if (rand.NextDouble() <= needed / (double)remaining)
                    {
                        result[taken++] = minValue + i;
                        needed--;
                    }
                    remaining--;
                }
            }

            return result;
        }
    }
}
