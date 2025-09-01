using UnityEngine;

namespace GG.DataStructures.Octree
{
    public class PointOctreeVisualizer : OctreeVisualizer
    {
        public PointOctree octree;

        // Example usage:
        // void Awake()
        // {
        //     octree = new PointOctree(Vector3.zero, 100);
        //     for (int i = 0; i < 1000; i++)
        //     {
        //         octree.Add(Random.insideUnitSphere * 50);
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
            Gizmos.color = Color.yellow;
            octree.DrawAllObjects(maxDrawingDepth);
        }
    }
}
