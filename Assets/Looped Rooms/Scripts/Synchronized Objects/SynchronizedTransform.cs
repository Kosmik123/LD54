using NaughtyAttributes;
using UnityEngine;

namespace Bipolar.LoopedRooms
{
    [CreateAssetMenu(menuName = "Looped Rooms/Synchronized ID")]
    public class SynchronizedTransform : UniqueID, ILocalTransform
    {
        public event System.Action<Vector3> OnLocalPositionChanged;
        public event System.Action<Quaternion> OnLocalRotationChanged;
        public event System.Action<Vector3> OnLocalScaleChanged;

        [SerializeField, ReadOnly] 
        private Vector3 localPosition; 
        
        [SerializeField, ReadOnly] 
        private Quaternion localRotation; 
        
        [SerializeField, ReadOnly] 
        private Vector3 localScale;

        public Vector3 LocalPosition
        {
            get => localPosition;
            set
            {
                localPosition = value;
                OnLocalPositionChanged?.Invoke(localPosition);
            }
        }

        public Quaternion LocalRotation
        {
            get => localRotation;
            set
            {
                localRotation = value;
                OnLocalRotationChanged?.Invoke(localRotation);
            }
        }

        public Vector3 LocalScale
        {
            get => localScale;
            set
            {
                localScale = value;
                OnLocalScaleChanged?.Invoke(localScale);
            }
        }
    } 
}
