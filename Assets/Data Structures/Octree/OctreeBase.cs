using System.Collections.Generic;
using UnityEngine;

namespace GG.DataStructures.Octree
{
    public abstract class OctreeBase<T>
    {
        protected readonly OctreeNode root;
        protected readonly int maxObjectsPerNode;

        protected abstract bool DoesItemIntersectBounds(T item, Bounds bounds);
        protected abstract bool IsItemContainedInBounds(T item, Bounds bounds);
        protected abstract float GetItemSqrDistance(T item, Vector3 point);
        protected abstract bool DoesItemIntersectRay(T item, Ray ray);
        protected abstract bool DoesItemIntersectSphere(T item, Vector3 center, float radius);
        protected abstract bool IsItemInFrustum(T item, Plane[] planes);
        protected abstract Bounds GetItemBounds(T item);

        protected OctreeBase(Vector3 center, float size, int maxObjects)
        {
            root = new OctreeNode(new Bounds(center, Vector3.one * size), 0, this);
            this.maxObjectsPerNode = maxObjects;
        }

        public void Add(T item)
        {
            root.Add(item);
        }

        public bool Remove(T item)
        {
            return root.Remove(item);
        }

        public List<T> GetObjectsInBounds(Bounds bounds)
        {
            var result = new List<T>();
            root.GetObjectsInBounds(bounds, result);
            return result;
        }

        public bool GetClosestObject(Vector3 point, out T closestObject)
        {
            float minSqrDistance = float.MaxValue;
            closestObject = default(T);
            root.GetClosestObject(point, ref closestObject, ref minSqrDistance);
            return !EqualityComparer<T>.Default.Equals(closestObject, default(T));
        }

        public List<T> GetObjectsAlongRay(Ray ray)
        {
            var result = new List<T>();
            root.GetObjectsAlongRay(ray, result);
            return result;
        }

        public List<T> GetObjectsInSphere(Vector3 center, float radius)
        {
            var result = new List<T>();
            root.GetObjectsInSphere(center, radius, result);
            return result;
        }

        public List<T> GetObjectsInFrustum(Plane[] frustumPlanes)
        {
            var result = new List<T>();
            root.GetObjectsInFrustum(frustumPlanes, result);
            return result;
        }

        public void DrawAllBounds(int maxDepth = -1)
        {
            root.DrawNodeBounds(maxDepth);
        }

        public void DrawAllObjects(int maxDepth = -1)
        {
            root.DrawNodeObjects(maxDepth);
        }

        protected class OctreeNode
        {
            public Bounds NodeBounds { get; }
            public int Level { get; }

            private readonly List<T> objects = new List<T>();
            private OctreeNode[] children;
            private bool IsLeaf => children == null;
            private readonly OctreeBase<T> octree;

            public OctreeNode(Bounds bounds, int level, OctreeBase<T> octree)
            {
                this.NodeBounds = bounds;
                this.Level = level;
                this.octree = octree;
            }

            public void Add(T item)
            {
                if (!octree.DoesItemIntersectBounds(item, NodeBounds)) return;

                if (IsLeaf)
                {
                    objects.Add(item);
                    if (objects.Count > octree.maxObjectsPerNode && Level < 10) // Add a depth limit to prevent infinite subdivision
                    {
                        Subdivide();
                    }
                }
                else
                {
                    int index = GetChildIndex(item);
                    if (index != -1)
                    {
                        children[index].Add(item);
                    }
                    else
                    {
                        objects.Add(item);
                    }
                }
            }

            public bool Remove(T item)
            {
                if (!octree.DoesItemIntersectBounds(item, NodeBounds)) return false;

                if (objects.Remove(item)) return true;

                if (!IsLeaf)
                {
                    int index = GetChildIndex(item);
                    if (index != -1)
                    {
                        if (children[index].Remove(item))
                        {
                            TryCollapse();
                            return true;
                        }
                    }
                }
                return false;
            }

            public void GetObjectsInBounds(Bounds bounds, List<T> result)
            {
                if (!NodeBounds.Intersects(bounds)) return;

                foreach (var obj in objects)
                {
                    if (octree.DoesItemIntersectBounds(obj, bounds))
                    {
                        result.Add(obj);
                    }
                }

                if (!IsLeaf)
                {
                    foreach (var child in children)
                    {
                        child.GetObjectsInBounds(bounds, result);
                    }
                }
            }

            public void GetClosestObject(Vector3 point, ref T closestObject, ref float minSqrDistance)
            {
                if (NodeBounds.SqrDistance(point) > minSqrDistance) return;

                foreach (var obj in objects)
                {
                    float sqrDistance = octree.GetItemSqrDistance(obj, point);
                    if (sqrDistance < minSqrDistance)
                    {
                        minSqrDistance = sqrDistance;
                        closestObject = obj;
                    }
                }

                if (!IsLeaf)
                {
                    var childOrder = GetChildIndicesByDistance(point);
                    foreach (var index in childOrder)
                    {
                        children[index].GetClosestObject(point, ref closestObject, ref minSqrDistance);
                    }
                }
            }

            public void GetObjectsAlongRay(Ray ray, List<T> result)
            {
                if (!NodeBounds.IntersectRay(ray)) return;

                foreach (var obj in objects)
                {
                    if (octree.DoesItemIntersectRay(obj, ray))
                    {
                        result.Add(obj);
                    }
                }

                if (!IsLeaf)
                {
                    foreach (var child in children)
                    {
                        child.GetObjectsAlongRay(ray, result);
                    }
                }
            }

            public void GetObjectsInSphere(Vector3 center, float radius, List<T> result)
            {
                if ((NodeBounds.ClosestPoint(center) - center).sqrMagnitude > radius * radius) return;

                foreach (var obj in objects)
                {
                    if (octree.DoesItemIntersectSphere(obj, center, radius))
                    {
                        result.Add(obj);
                    }
                }

                if (!IsLeaf)
                {
                    foreach (var child in children)
                    {
                        child.GetObjectsInSphere(center, radius, result);
                    }
                }
            }

            public void GetObjectsInFrustum(Plane[] frustumPlanes, List<T> result)
            {
                if (!GeometryUtility.TestPlanesAABB(frustumPlanes, NodeBounds)) return;

                foreach (var obj in objects)
                {
                    if (octree.IsItemInFrustum(obj, frustumPlanes))
                    {
                        result.Add(obj);
                    }
                }

                if (!IsLeaf)
                {
                    foreach (var child in children)
                    {
                        child.GetObjectsInFrustum(frustumPlanes, result);
                    }
                }
            }

            public void DrawNodeBounds(int maxDepth)
            {
                if (maxDepth != -1 && Level > maxDepth) return;

                Gizmos.DrawWireCube(NodeBounds.center, NodeBounds.size);

                if (!IsLeaf)
                {
                    foreach (var child in children)
                    {
                        child.DrawNodeBounds(maxDepth);
                    }
                }
            }

            public void DrawNodeObjects(int maxDepth)
            {
                if (maxDepth != -1 && Level > maxDepth) return;

                foreach (var obj in objects)
                {
                    Gizmos.DrawWireCube(octree.GetItemBounds(obj).center, octree.GetItemBounds(obj).size);
                }
                if (!IsLeaf)
                {
                    foreach (var child in children)
                    {
                        child.DrawNodeObjects(maxDepth);
                    }
                }
            }

            private int[] GetChildIndicesByDistance(Vector3 point)
            {
                int[] indices = { 0, 1, 2, 3, 4, 5, 6, 7 };
                System.Array.Sort(indices, (a, b) =>
                    children[a].NodeBounds.SqrDistance(point).CompareTo(
                    children[b].NodeBounds.SqrDistance(point)));
                return indices;
            }

            private int GetChildIndex(T item)
            {
                int index = -1;
                for (int i = 0; i < 8; i++)
                {
                    if (children[i] == null) continue;

                    if (octree.IsItemContainedInBounds(item, children[i].NodeBounds))
                    {
                        if (index != -1) return -1;
                        index = i;
                    }
                }
                return index;
            }

            private void Subdivide()
            {
                children = new OctreeNode[8];
                float childSize = NodeBounds.size.y / 2.0f;
                var childSizeVec = new Vector3(childSize, childSize, childSize);
                Vector3 parentCenter = NodeBounds.center;

                for (int i = 0; i < 8; i++)
                {
                    Vector3 childCenter = parentCenter;
                    childCenter.x += (i & 1) == 0 ? -childSize / 2.0f : childSize / 2.0f;
                    childCenter.y += (i & 2) == 0 ? -childSize / 2.0f : childSize / 2.0f;
                    childCenter.z += (i & 4) == 0 ? -childSize / 2.0f : childSize / 2.0f;
                    children[i] = new OctreeNode(new Bounds(childCenter, childSizeVec), Level + 1, octree);
                }

                for (int i = objects.Count - 1; i >= 0; i--)
                {
                    T obj = objects[i];
                    int index = GetChildIndex(obj);
                    if (index != -1)
                    {
                        children[index].Add(obj);
                        objects.RemoveAt(i);
                    }
                }
            }

            private void TryCollapse()
            {
                if (IsLeaf) return;

                int totalObjects = objects.Count;
                foreach (var child in children)
                {
                    if (!child.IsLeaf) return;
                    totalObjects += child.objects.Count;
                }

                if (totalObjects <= octree.maxObjectsPerNode)
                {
                    foreach (var child in children)
                    {
                        objects.AddRange(child.objects);
                    }
                    children = null;
                }
            }
        }
    }
}
