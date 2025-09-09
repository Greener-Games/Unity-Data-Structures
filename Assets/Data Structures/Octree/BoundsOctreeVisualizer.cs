using UnityEngine;

namespace GG.DataStructures.Octree
{
    public class BoundsOctreeVisualizer : OctreeVisualizer
    {
        // A simple example object that has bounds.
        private class TestObject : IHaveBounds
        {
            public Bounds Bounds { get; }
            public TestObject(Vector3 center, Vector3 size)
            {
                Bounds = new Bounds(center, size);
            }
        }

        public BoundsOctree<TestObject> octree;

        // Example usage:
        // void Awake()
        // {
        //     octree = new BoundsOctree<TestObject>(Vector3.zero, 100);
        //     for (int i = 0; i < 500; i++)
        //     {
        //         var center = Random.insideUnitSphere * 40;
        //         var size = Vector3.one * Random.Range(1, 5);
        //         octree.Add(new TestObject(center, size));
        //     }
        // }

        protected override void DrawBoundsGizmos()
        {
            if (octree == null) return;
            Gizmos.color = Color.white;
            octree.DrawAllBounds(maxDrawingDepth);
        }

        protected override void DrawObjectsGizmos()
        {
            if (octree == null) return;
            Gizmos.color = Color.cyan;
            octree.DrawAllObjects(maxDrawingDepth);
        }
    }
}
