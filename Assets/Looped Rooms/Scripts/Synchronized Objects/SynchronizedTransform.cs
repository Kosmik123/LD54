using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.LoopedRooms
{
    [CreateAssetMenu(menuName = "Looped Rooms/Synchronized Transform")]
    public class SynchronizedTransform : UniqueID, ILocalTransform
    {
        public event System.Action<Vector3> OnLocalPositionChanged;
        public event System.Action<Quaternion> OnLocalRotationChanged;
        public event System.Action<Vector3> OnLocalScaleChanged;

        [SerializeField] 
        private Vector3 localPosition; 
        
        [SerializeField] 
        private Quaternion localRotation; 
        
        [SerializeField] 
        private Vector3 localScale;

        [SerializeField]
        private List<Transform> sceneTransforms = new List<Transform>();
        public IReadOnlyList<Transform> SceneTransforms => sceneTransforms;

        protected override void Reset()
        {
            base.Reset();
            ResetTransform();
        }

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

        [ContextMenu("Reset Transform")]
        protected void ResetTransform()
        {
            localPosition = Vector3.zero;
            localRotation = Quaternion.identity;
            localScale = Vector3.one;
        }

        public void Add(Transform transform)
        {
            sceneTransforms.Add(transform);
        }

        public void Remove(Transform transform)
        {
            sceneTransforms.Remove(transform);
        }
    } 
}
