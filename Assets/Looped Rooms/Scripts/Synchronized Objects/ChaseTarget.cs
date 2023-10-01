using UnityEngine;

namespace Bipolar.LoopedRooms
{
    public class ChaseTarget : MonoBehaviour
    {
        [SerializeField]
        private SynchronizedTransformController synchronizedTransformController;
        public SynchronizedTransformController SynchronizedTransformController => synchronizedTransformController;

        [SerializeField]
        private float moveSpeed;
        [SerializeField]
        private float rotationSpeed;

        [Space]
        [SerializeField]
        private Transform target;
        public Transform Target
        {
            get => target;
            set => target = value;
        }

        [SerializeField]
        private float minDistanceToTarget;
        [SerializeField]
        private float maxDistanceToTarget;

        [SerializeField]
        private bool isMoving;

        [SerializeField]
        private AudioSource audioSource;
        private float soundProgress;

        private void Awake()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            UpdateMove();
            if (isMoving && audioSource.isPlaying == false)
            {
                audioSource.Play();
                audioSource.time = soundProgress;
            }
            else if (isMoving == false && audioSource.isPlaying)
            {
                soundProgress = audioSource.time;
                audioSource.Stop();
            }
        }

        private void UpdateMove()
        {
            isMoving = false;
            var movedTransform = synchronizedTransformController.transform;

            Vector3 direction = target.position - movedTransform.position;
            direction.y = 0;
            float squareDistance = direction.sqrMagnitude;
            if (squareDistance > maxDistanceToTarget * maxDistanceToTarget)
                return;

            if (squareDistance < minDistanceToTarget * minDistanceToTarget)
                return;

            isMoving = true;
            direction.Normalize();
            float dt = Time.deltaTime;
            movedTransform.forward = Vector3.MoveTowards(movedTransform.forward, direction, rotationSpeed * dt);
            movedTransform.Translate(moveSpeed * dt * direction, Space.World);
            synchronizedTransformController.Synchronize();
        }

        private void OnDisable()
        {
            audioSource.Stop();
        }
    }
}
