using GG.DataStructures.Octree;
using NUnit.Framework;
using UnityEngine;

namespace GG.DataStructures.Tests
{
    public class BoundsOctreeTests
    {
        private struct TestBoundsObject : IHaveBounds
        {
            public Bounds Bounds { get; set; }
            public int ID { get; set; }

            public override bool Equals(object obj) => obj is TestBoundsObject other && other.ID == ID;
            public override int GetHashCode() => ID;
        }

        [Test]
        public void Add_SingleObject_CanBeFound()
        {
            var octree = new BoundsOctree<TestBoundsObject>(Vector3.zero, 10);
            var obj = new TestBoundsObject { Bounds = new Bounds(new Vector3(1, 1, 1), Vector3.one), ID = 1 };
            octree.Add(obj);

            var results = octree.GetObjectsInBounds(new Bounds(new Vector3(1, 1, 1), Vector3.one * 2));
            Assert.IsTrue(results.Contains(obj));
        }

        [Test]
        public void Remove_SingleObject_IsRemoved()
        {
            var octree = new BoundsOctree<TestBoundsObject>(Vector3.zero, 10);
            var obj = new TestBoundsObject { Bounds = new Bounds(new Vector3(1, 1, 1), Vector3.one), ID = 1 };
            octree.Add(obj);
            octree.Remove(obj);

            var results = octree.GetObjectsInBounds(new Bounds(new Vector3(1, 1, 1), Vector3.one * 2));
            Assert.IsFalse(results.Contains(obj));
        }

        [Test]
        public void GetObjectsAlongRay_FindsCorrectObject()
        {
            var octree = new BoundsOctree<TestBoundsObject>(Vector3.zero, 20);
            var obj1 = new TestBoundsObject { Bounds = new Bounds(new Vector3(5, 0, 0), Vector3.one), ID = 1 };
            var obj2 = new TestBoundsObject { Bounds = new Bounds(new Vector3(-5, 0, 0), Vector3.one), ID = 2 };
            octree.Add(obj1);
            octree.Add(obj2);

            var ray = new Ray(Vector3.zero, Vector3.right);
            var results = octree.GetObjectsAlongRay(ray);

            Assert.IsTrue(results.Contains(obj1));
            Assert.IsFalse(results.Contains(obj2));
        }
    }
}
