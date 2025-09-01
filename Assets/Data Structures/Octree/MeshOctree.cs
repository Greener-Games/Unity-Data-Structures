using UnityEngine;

namespace GG.DataStructures.Octree
{
    public struct MeshRaycastHit
    {
        public float Distance { get; }
        public Vector3 Point { get; }
        public MeshTriangle Triangle { get; }
        public GameObject ParentObject => Triangle.ParentObject;

        public MeshRaycastHit(float distance, Vector3 point, MeshTriangle triangle)
        {
            Distance = distance;
            Point = point;
            Triangle = triangle;
        }
    }

    public class MeshOctree : BoundsOctree<MeshTriangle>
    {
        public MeshOctree(Vector3 center, float size, int maxObjects = 8) : base(center, size, maxObjects)
        {
        }

        public void AddGameObject(GameObject go)
        {
            var meshFilters = go.GetComponentsInChildren<MeshFilter>();
            foreach (var mf in meshFilters)
            {
                var mesh = mf.sharedMesh;
                if (mesh == null) continue;

                var vertices = mesh.vertices;
                var triangles = mesh.triangles;
                var transform = mf.transform;

                for (int i = 0; i < triangles.Length; i += 3)
                {
                    Vector3 v1 = transform.TransformPoint(vertices[triangles[i]]);
                    Vector3 v2 = transform.TransformPoint(vertices[triangles[i + 1]]);
                    Vector3 v3 = transform.TransformPoint(vertices[triangles[i + 2]]);

                    Add(new MeshTriangle(go, v1, v2, v3));
                }
            }
        }

        public bool Raycast(Ray ray, out MeshRaycastHit hit)
        {
            hit = default;
            float minDistance = float.MaxValue;

            var candidates = GetObjectsAlongRay(ray);
            if (candidates.Count == 0) return false;

            foreach (var triangle in candidates)
            {
                if (RayIntersectsTriangle(ray, triangle, out float distance, out Vector3 point))
                {
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        hit = new MeshRaycastHit(distance, point, triangle);
                    }
                }
            }

            return minDistance != float.MaxValue;
        }

        // Möller–Trumbore intersection algorithm
        private bool RayIntersectsTriangle(Ray ray, MeshTriangle triangle, out float distance, out Vector3 hitPoint)
        {
            distance = 0;
            hitPoint = Vector3.zero;

            const float Epsilon = 1e-8f;
            Vector3 edge1 = triangle.V2 - triangle.V1;
            Vector3 edge2 = triangle.V3 - triangle.V1;
            Vector3 h = Vector3.Cross(ray.direction, edge2);
            float a = Vector3.Dot(edge1, h);

            if (a > -Epsilon && a < Epsilon)
                return false; // Ray is parallel to the triangle

            float f = 1.0f / a;
            Vector3 s = ray.origin - triangle.V1;
            float u = f * Vector3.Dot(s, h);

            if (u < 0.0f || u > 1.0f)
                return false;

            Vector3 q = Vector3.Cross(s, edge1);
            float v = f * Vector3.Dot(ray.direction, q);

            if (v < 0.0f || u + v > 1.0f)
                return false;

            float t = f * Vector3.Dot(edge2, q);

            if (t > Epsilon) // Ray intersection
            {
                distance = t;
                hitPoint = ray.origin + ray.direction * t;
                return true;
            }

            return false;
        }
    }
}
