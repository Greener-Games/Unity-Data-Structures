using UnityEngine;

namespace GG.DataStructures.Octree
{
    public class MeshOctreeVisualizer : OctreeVisualizer
    {
        [Tooltip("The target GameObject to build the MeshOctree from.")]
        public GameObject targetObject;

        public MeshOctree octree;

        // Example usage:
        // void Start()
        // {
        //     if (targetObject != null)
        //     {
        //         // Find the bounds of the target object to size the octree correctly.
        //         var renderers = targetObject.GetComponentsInChildren<Renderer>();
        //         if (renderers.Length > 0)
        //         {
        //             var totalBounds = renderers[0].bounds;
        //             for (int i = 1; i < renderers.Length; i++)
        //             {
        //                 totalBounds.Encapsulate(renderers[i].bounds);
        //             }
        //
        //             octree = new MeshOctree(totalBounds.center, totalBounds.size.magnitude);
        //             octree.AddGameObject(targetObject);
        //         }
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
            Gizmos.color = Color.green;
            octree.DrawAllObjects(maxDrawingDepth);
        }
    }
}
