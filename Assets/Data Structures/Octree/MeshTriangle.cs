using UnityEngine;

namespace GG.DataStructures.Octree
{
    public struct MeshTriangle : IHaveBounds
    {
        public GameObject ParentObject { get; }
        public Vector3 V1 { get; }
        public Vector3 V2 { get; }
        public Vector3 V3 { get; }
        public Bounds Bounds { get; }

        public MeshTriangle(GameObject parent, Vector3 v1, Vector3 v2, Vector3 v3)
        {
            ParentObject = parent;
            V1 = v1;
            V2 = v2;
            V3 = v3;

            Vector3 min = Vector3.Min(v1, Vector3.Min(v2, v3));
            Vector3 max = Vector3.Max(v1, Vector3.Max(v2, v3));
            Bounds = new Bounds((min + max) / 2, max - min);
        }
    }
}
