using UnityEngine;

namespace Bipolar.LoopedRooms
{
    public class SynchronizedTransformController : MonoBehaviour
    {
        [SerializeField]
        private SynchronizedTransform synchronizedTransform;
        public SynchronizedTransform SynchronizedTransform => synchronizedTransform;

        private void Awake()
        {
            synchronizedTransform.OnLocalPositionChanged += UpdatePosition;
            synchronizedTransform.OnLocalRotationChanged += UpdateRotation;
            synchronizedTransform.OnLocalScaleChanged += UpdateScale;
        }

        private void OnEnable()
        {
            transform.localPosition = synchronizedTransform.LocalPosition;
            transform.localRotation = synchronizedTransform.LocalRotation;
            transform.localScale = synchronizedTransform.LocalScale;
        }

        private void Start()
        {
            Synchronize();
        }

        [ContextMenu("Synchronize")]
        public void Synchronize()
        {
            synchronizedTransform.LocalPosition = transform.localPosition;
            synchronizedTransform.LocalRotation = transform.localRotation;
            synchronizedTransform.LocalScale = transform.localScale;
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
            synchronizedTransform.OnLocalPositionChanged -= UpdatePosition;
            synchronizedTransform.OnLocalRotationChanged -= UpdateRotation;
            synchronizedTransform.OnLocalScaleChanged -= UpdateScale;
        }
    }
}
