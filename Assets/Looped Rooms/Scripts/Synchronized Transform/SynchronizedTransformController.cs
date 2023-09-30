using UnityEngine;

namespace Bipolar.LoopedRooms
{
    public class SynchronizedTransformController : MonoBehaviour
    {
        [SerializeField]
        private SynchronizedTransform id;

        private void Awake()
        {
            id.OnLocalPositionChanged += UpdatePosition;
            id.OnLocalRotationChanged += UpdateRotation;
            id.OnLocalScaleChanged += UpdateScale;
        }

        private void OnEnable()
        {
            transform.localPosition = id.LocalPosition;
            transform.localRotation = id.LocalRotation;
            transform.localScale = id.LocalScale;
        }

        private void Start()
        {
            Synchronize();
        }

        [ContextMenu("Synchronize")]
        public void Synchronize()
        {
            id.LocalPosition = transform.localPosition;
            id.LocalRotation = transform.localRotation;
            id.LocalScale = transform.localScale;
        }

        private void UpdatePosition(Vector3 localPosition)
        {
            transform.localPosition = localPosition;
        }

        private void UpdateRotation(Quaternion localRotation)
        {
            transform.localRotation = localRotation;
        }

        private void UpdateScale(Vector3 localScale)
        {
            transform.localScale = localScale;
        }

        private void OnDestroy()
        {
            id.OnLocalPositionChanged -= UpdatePosition;
            id.OnLocalRotationChanged -= UpdateRotation;
            id.OnLocalScaleChanged -= UpdateScale;
        }
    }
}
