using GG.DataStructures.Octree;
using NUnit.Framework;
using UnityEngine;

namespace GG.DataStructures.Tests
{
    public class MeshOctreeTests
    {
        private GameObject CreateTestQuad()
        {
            var go = new GameObject("TestQuad");
            var mf = go.AddComponent<MeshFilter>();
            var mesh = new Mesh();

            mesh.vertices = new[]
            {
                new Vector3(-1, -1, 0),
                new Vector3(1, -1, 0),
                new Vector3(-1, 1, 0),
                new Vector3(1, 1, 0)
            };

            mesh.triangles = new[]
            {
                0, 2, 1,
                2, 3, 1
            };

            mf.mesh = mesh;
            return go;
        }

        [Test]
        public void AddGameObject_And_Raycast_HitsCorrectObject()
        {
            // Arrange
            var octree = new MeshOctree(Vector3.zero, 20);
            var quad = CreateTestQuad();
            octree.AddGameObject(quad);

            // Act
            var ray = new Ray(new Vector3(0, 0, -5), Vector3.forward);
            bool didHit = octree.Raycast(ray, out var hit);

            // Assert
            Assert.IsTrue(didHit);
            Assert.AreEqual(quad, hit.ParentObject);

            // Cleanup
            Object.DestroyImmediate(quad);
        }

        [Test]
        public void Raycast_Misses_ReturnsFalse()
        {
            // Arrange
            var octree = new MeshOctree(Vector3.zero, 20);
            var quad = CreateTestQuad();
            octree.AddGameObject(quad);

            // Act
            var ray = new Ray(new Vector3(5, 5, -5), Vector3.forward);
            bool didHit = octree.Raycast(ray, out _);

            // Assert
            Assert.IsFalse(didHit);

            // Cleanup
            Object.DestroyImmediate(quad);
        }
    }
}
