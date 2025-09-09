using UnityEngine;

namespace GG.DataStructures.Octree
{
    public interface IHaveBounds
    {
        Bounds Bounds { get; }
    }
}
