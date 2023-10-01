using UnityEngine;

namespace Bipolar.LoopedRooms
{
    public interface ILocalTransform
    {
        Vector3 LocalPosition { get; set; }
        Quaternion LocalRotation { get; set; }
        Vector3 LocalScale { get; set; }
    }
}
