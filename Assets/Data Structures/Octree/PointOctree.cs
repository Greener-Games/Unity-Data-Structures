using UnityEngine;

namespace GG.DataStructures.Octree
{
    public class PointOctree : OctreeBase<Vector3>
    {
        public PointOctree(Vector3 center, float size, int maxObjects = 8) : base(center, size, maxObjects)
        {
        }

        public override void Add(Vector3 point)
        {
            root.Add(point);
        }

        public override bool Remove(Vector3 point)
        {
            return root.Remove(point);
        }

        protected override bool DoesItemIntersectBounds(Vector3 point, Bounds bounds)
        {
            return bounds.Contains(point);
        }

        protected override bool IsItemContainedInBounds(Vector3 point, Bounds bounds)
        {
            return bounds.Contains(point);
        }

        protected override float GetItemSqrDistance(Vector3 point, Vector3 otherPoint)
        {
            return (point - otherPoint).sqrMagnitude;
        }

        protected override bool DoesItemIntersectRay(Vector3 point, Ray ray)
        {
            float sqrDist = Vector3.Cross(ray.direction, point - ray.origin).sqrMagnitude;
            const float Epsilon = 1e-6f;
            return sqrDist < Epsilon;
        }

        protected override bool DoesItemIntersectSphere(Vector3 point, Vector3 center, float radius)
        {
            return (point - center).sqrMagnitude <= radius * radius;
        }

        protected override bool IsItemInFrustum(Vector3 point, Plane[] planes)
        {
            foreach (var plane in planes)
            {
                if (plane.GetSide(point) == false)
                {
                    return false;
                }
            }
            return true;
        }

        protected override Bounds GetItemBounds(Vector3 point)
        {
            return new Bounds(point, Vector3.one * 0.1f); // Represent point as a small cube
        }
    }
}
