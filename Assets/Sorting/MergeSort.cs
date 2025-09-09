using System;
using System.Collections.Generic;
using System.Linq;

namespace GG.Sorting
{
    public static class MergeSort
    {
        /// <summary>
        /// Sorts a list of items using the merge sort algorithm and returns a new sorted list
        /// </summary>
        /// <param name="items">The items to be sorted</param>
        /// <param name="comparer">The comparer used to determine the order of the items</param>
        /// <typeparam name="T">The type of the items to be sorted</typeparam>
        /// <returns>A new list with the sorted items</returns>
        public static IList<T> Sort<T>(IList<T> items, IComparer<T> comparer = null)
        {
            comparer = comparer ?? Comparer<T>.Default;
            return InternalMergeSort(items.ToList(), comparer);
        }

        private static List<T> InternalMergeSort<T>(List<T> items, IComparer<T> comparer)
        {
            if (items.Count <= 1)
            {
                return items;
            }

            var mid = items.Count / 2;
            var left = items.GetRange(0, mid);
            var right = items.GetRange(mid, items.Count - mid);

            left = InternalMergeSort(left, comparer);
            right = InternalMergeSort(right, comparer);

            return Merge(left, right, comparer);
        }

        private static List<T> Merge<T>(List<T> left, List<T> right, IComparer<T> comparer)
        {
            var result = new List<T>();
            var leftIndex = 0;
            var rightIndex = 0;

            while (leftIndex < left.Count && rightIndex < right.Count)
            {
                if (comparer.Compare(left[leftIndex], right[rightIndex]) <= 0)
                {
                    result.Add(left[leftIndex]);
                    leftIndex++;
                }
                else
                {
                    result.Add(right[rightIndex]);
                    rightIndex++;
                }
            }

            while (leftIndex < left.Count)
            {
                result.Add(left[leftIndex]);
                leftIndex++;
            }

            while (rightIndex < right.Count)
            {
                result.Add(right[rightIndex]);
                rightIndex++;
            }

            return result;
        }
    }
}
