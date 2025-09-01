using UnityEngine;

namespace GG.DataStructures.Octree
{
    public class BoundsOctree<T> : OctreeBase<T> where T : IHaveBounds
    {
        public BoundsOctree(Vector3 center, float size, int maxObjects = 8) : base(center, size, maxObjects)
        {
        }

        public override void Add(T item)
        {
            root.Add(item);
        }

        public override bool Remove(T item)
        {
            return root.Remove(item);
        }

        protected override bool DoesItemIntersectBounds(T item, Bounds bounds)
        {
            return item.Bounds.Intersects(bounds);
        }

        protected override bool IsItemContainedInBounds(T item, Bounds bounds)
        {
            return bounds.Contains(item.Bounds.min) && bounds.Contains(item.Bounds.max);
        }

        protected override float GetItemSqrDistance(T item, Vector3 point)
        {
            return item.Bounds.SqrDistance(point);
        }

        protected override bool DoesItemIntersectRay(T item, Ray ray)
        {
            return item.Bounds.IntersectRay(ray);
        }

        protected override bool DoesItemIntersectSphere(T item, Vector3 center, float radius)
        {
            return (item.Bounds.ClosestPoint(center) - center).sqrMagnitude <= radius * radius;
        }

        protected override bool IsItemInFrustum(T item, Plane[] planes)
        {
            return GeometryUtility.TestPlanesAABB(planes, item.Bounds);
        }

        protected override Bounds GetItemBounds(T item)
        {
            return item.Bounds;
        }
    }
}
