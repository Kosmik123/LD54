using UnityEngine;

namespace Bipolar.LoopedRooms
{
    public class ChaseTarget : MonoBehaviour
    {
        [SerializeField]
        private SynchronizedTransformController synchronizedTransformController;
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
        private float maxDistanceToTarget;

        private void Awake()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0;
            float squareDistance = direction.sqrMagnitude;
            if (squareDistance > maxDistanceToTarget * maxDistanceToTarget)
                return;

            direction.Normalize();
            float dt = Time.deltaTime;
            transform.forward = Vector3.MoveTowards(transform.forward, direction, rotationSpeed * dt);
            transform.Translate(moveSpeed * dt * direction, Space.World);
            synchronizedTransformController.Synchronize();
        }
    }
}
