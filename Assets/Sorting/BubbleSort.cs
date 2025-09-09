using System;
using System.Collections.Generic;

namespace GG.Sorting
{
    public static class BubbleSort
    {
        /// <summary>
        /// Sorts an array of items in place using the bubble sort algorithm
        /// </summary>
        /// <param name="items">The items to be sorted</param>
        /// <param name="comparer">The comparer used to determine the order of the items</param>
        /// <typeparam name="T">The type of the items to be sorted</typeparam>
        public static void Sort<T>(IList<T> items, IComparer<T> comparer = null)
        {
            comparer = comparer ?? Comparer<T>.Default;

            var hadSwap = true;
            while (hadSwap)
            {
                hadSwap = false;
                for (var i = 0; i < items.Count - 1; i++)
                {
                    if (comparer.Compare(items[i], items[i + 1]) > 0)
                    {
                        (items[i], items[i + 1]) = (items[i + 1], items[i]);
                        hadSwap = true;
                    }
                }
            }
        }
    }
}
