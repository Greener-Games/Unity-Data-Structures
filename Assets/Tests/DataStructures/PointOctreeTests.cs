using GG.DataStructures.Octree;
using NUnit.Framework;
using UnityEngine;

namespace GG.DataStructures.Tests
{
    public class PointOctreeTests
    {
        [Test]
        public void Add_SinglePoint_CanBeFound()
        {
            var octree = new PointOctree(Vector3.zero, 10);
            var point = new Vector3(1, 2, 3);
            octree.Add(point);

            octree.GetClosestObject(point, out var closest);
            Assert.AreEqual(point, closest);
        }

        [Test]
        public void Remove_SinglePoint_IsRemoved()
        {
            var octree = new PointOctree(Vector3.zero, 10);
            var point = new Vector3(1, 2, 3);
            octree.Add(point);
            octree.Remove(point);

            bool found = octree.GetClosestObject(point, out _);
            Assert.IsFalse(found);
        }

        [Test]
        public void GetClosestObject_FindsCorrectPoint()
        {
            var octree = new PointOctree(Vector3.zero, 10);
            var p1 = new Vector3(1, 1, 1);
            var p2 = new Vector3(4, 4, 4);
            octree.Add(p1);
            octree.Add(p2);

            octree.GetClosestObject(Vector3.zero, out var closest);
            Assert.AreEqual(p1, closest);
        }
    }
}
