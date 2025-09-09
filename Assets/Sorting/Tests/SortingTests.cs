using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GG.Sorting.Tests
{
    [TestFixture]
    public class SortingTests
    {
        private readonly List<int> _unsortedList = new List<int> { 5, 1, 4, 2, 8 };
        private readonly List<int> _sortedList = new List<int> { 1, 2, 4, 5, 8 };

        [Test]
        public void BubbleSort_Sorts_Correctly()
        {
            var listToSort = new List<int>(_unsortedList);
            BubbleSort.Sort(listToSort);
            Assert.AreEqual(_sortedList, listToSort);
        }

        [Test]
        public void BubbleSort_With_Empty_List()
        {
            var listToSort = new List<int>();
            BubbleSort.Sort(listToSort);
            Assert.AreEqual(new List<int>(), listToSort);
        }

        [Test]
        public void BubbleSort_With_Single_Element()
        {
            var listToSort = new List<int> { 1 };
            BubbleSort.Sort(listToSort);
            Assert.AreEqual(new List<int> { 1 }, listToSort);
        }

        [Test]
        public void InsertionSort_Sorts_Correctly()
        {
            var listToSort = new List<int>(_unsortedList);
            InsertionSort.Sort(listToSort);
            Assert.AreEqual(_sortedList, listToSort);
        }

        [Test]
        public void InsertionSort_With_Empty_List()
        {
            var listToSort = new List<int>();
            InsertionSort.Sort(listToSort);
            Assert.AreEqual(new List<int>(), listToSort);
        }

        [Test]
        public void InsertionSort_With_Single_Element()
        {
            var listToSort = new List<int> { 1 };
            InsertionSort.Sort(listToSort);
            Assert.AreEqual(new List<int> { 1 }, listToSort);
        }

        [Test]
        public void MergeSort_Sorts_Correctly()
        {
            var listToSort = new List<int>(_unsortedList);
            var sortedList = MergeSort.Sort(listToSort);
            Assert.AreEqual(_sortedList, sortedList);
        }

        [Test]
        public void MergeSort_With_Empty_List()
        {
            var listToSort = new List<int>();
            var sortedList = MergeSort.Sort(listToSort);
            Assert.AreEqual(new List<int>(), sortedList);
        }

        [Test]
        public void MergeSort_With_Single_Element()
        {
            var listToSort = new List<int> { 1 };
            var sortedList = MergeSort.Sort(listToSort);
            Assert.AreEqual(new List<int> { 1 }, sortedList);
        }

        [Test]
        public void QuickSort_Sorts_Correctly()
        {
            var listToSort = new List<int>(_unsortedList);
            QuickSort.Sort(listToSort);
            Assert.AreEqual(_sortedList, listToSort);
        }

        [Test]
        public void QuickSort_With_Empty_List()
        {
            var listToSort = new List<int>();
            QuickSort.Sort(listToSort);
            Assert.AreEqual(new List<int>(), listToSort);
        }

        [Test]
        public void QuickSort_With_Single_Element()
        {
            var listToSort = new List<int> { 1 };
            QuickSort.Sort(listToSort);
            Assert.AreEqual(new List<int> { 1 }, listToSort);
        }
    }
}
