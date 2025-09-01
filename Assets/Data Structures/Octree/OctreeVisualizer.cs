using UnityEngine;

namespace GG.DataStructures.Octree
{
    public abstract class OctreeVisualizer : MonoBehaviour
    {
        [Tooltip("Toggle drawing of the Octree node bounds.")]
        public bool drawNodeBounds = true;

        [Tooltip("Toggle drawing of the object bounds within the Octree.")]
        public bool drawNodeObjects = true;

        [Tooltip("The maximum depth to draw the Octree to. -1 means no limit.")]
        public int maxDrawingDepth = -1;

        private void OnDrawGizmos()
        {
            if (drawNodeBounds)
            {
                DrawBoundsGizmos();
            }

            if (drawNodeObjects)
            {
                DrawObjectsGizmos();
            }
        }

        protected abstract void DrawBoundsGizmos();
        protected abstract void DrawObjectsGizmos();
    }
}
