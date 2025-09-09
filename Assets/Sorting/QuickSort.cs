using System;
using System.Collections.Generic;

namespace GG.Sorting
{
    public static class QuickSort
    {
        /// <summary>
        /// Sorts a list of items in place using the quicksort algorithm
        /// </summary>
        /// <param name="items">The items to be sorted</param>
        /// <param name="comparer">The comparer used to determine the order of the items</param>
        /// <typeparam name="T">The type of the items to be sorted</typeparam>
        public static void Sort<T>(IList<T> items, IComparer<T> comparer = null)
        {
            comparer = comparer ?? Comparer<T>.Default;
            InternalQuickSort(items, 0, items.Count - 1, comparer);
        }

        private static void InternalQuickSort<T>(IList<T> items, int left, int right, IComparer<T> comparer)
        {
            if (left >= right)
            {
                return;
            }

            var pivotIndex = Partition(items, left, right, comparer);
            InternalQuickSort(items, left, pivotIndex - 1, comparer);
            InternalQuickSort(items, pivotIndex + 1, right, comparer);
        }

        private static int Partition<T>(IList<T> items, int left, int right, IComparer<T> comparer)
        {
            var pivot = items[right];
            var i = left - 1;

            for (var j = left; j < right; j++)
            {
                if (comparer.Compare(items[j], pivot) <= 0)
                {
                    i++;
                    (items[i], items[j]) = (items[j], items[i]);
                }
            }

            (items[i + 1], items[right]) = (items[right], items[i + 1]);
            return i + 1;
        }
    }
}
