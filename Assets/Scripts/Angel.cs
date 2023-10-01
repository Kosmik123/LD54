using Bipolar.LoopedRooms;
using Cinemachine;
using UnityEngine;

public class Angel : MonoBehaviour
{
    [field: SerializeField]
    public ChaseTarget ChaseTarget { get; private set; }

    [field: SerializeField]
    public SynchronizedTransformController SynchronizedTransformController { get; private set; }


    [field: SerializeField]
    public CinemachineVirtualCamera LookingCamera { get; private set; }
}
