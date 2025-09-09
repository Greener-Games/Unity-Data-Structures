using System;
using System.Collections.Generic;

namespace GG.Sorting
{
    public static class InsertionSort
    {
        /// <summary>
        /// Sorts a list of items in place using the insertion sort algorithm
        /// </summary>
        /// <param name="items">The items to be sorted</param>
        /// <param name="comparer">The comparer used to determine the order of the items</param>
        /// <typeparam name="T">The type of the items to be sorted</typeparam>
        public static void Sort<T>(IList<T> items, IComparer<T> comparer = null)
        {
            comparer = comparer ?? Comparer<T>.Default;

            for (var i = 1; i < items.Count; i++)
            {
                var current = items[i];
                var j = i - 1;
                while (j >= 0 && comparer.Compare(items[j], current) > 0)
                {
                    items[j + 1] = items[j];
                    j--;
                }
                items[j + 1] = current;
            }
        }
    }
}
