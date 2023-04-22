using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA.Core.Helpers
{
    public static class SearchHelper
    {
        public static int BinarySearch<T>(IList<T> collection, T searchedItem) where T : IComparable<T>
        {
            return BinarySearch(collection, searchedItem, 0, collection.Count);
        }

        private static int BinarySearch<T>(IList<T> collection, T searchedItem, int leftIndex, int rightIndex) where T : IComparable<T>
        {
            var index = (rightIndex + leftIndex) / 2;
            var pivot = collection[index];

            if (searchedItem.CompareTo(pivot) < 0)
                return BinarySearch(collection, searchedItem, leftIndex, index);
            else if (searchedItem.CompareTo(pivot) > 0)
                return BinarySearch(collection, searchedItem, index, rightIndex);
            else return index;
        }
    }
}
