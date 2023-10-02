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

    [field: SerializeField]
    public MeshRenderer MeshRenderer { get; private set; }

    [SerializeField]
    private Material bloodyMaterial;

    public void ChangeMaterial()
    {
        foreach (var tf in SynchronizedTransformController.SynchronizedTransform.SceneTransforms)
        {
            var angel = tf.GetComponent<Angel>();
            angel.MeshRenderer.material = bloodyMaterial;
        }
    }
}
