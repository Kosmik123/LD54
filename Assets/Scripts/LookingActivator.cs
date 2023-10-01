using Bipolar.LoopedRooms;
using UnityEngine;

public class LookingActivator : MonoBehaviour
{
    [SerializeField]
    private Collider viewBoundsCollider;
    [SerializeField]
    private ChaseTarget chaseTarget;
    [SerializeField]
    private Collider grabCollider;

    private Camera viewCamera;
    private readonly Plane[] cameraFrustum = new Plane[6];

    private void Awake()
    {
        viewCamera = Camera.main;
    }

    private void Update()
    {
        GeometryUtility.CalculateFrustumPlanes(viewCamera, cameraFrustum);
        var bounds = viewBoundsCollider.bounds;

        bool isVisible = GeometryUtility.TestPlanesAABB(cameraFrustum, bounds);
        grabCollider.enabled = !isVisible;
        chaseTarget.enabled = !isVisible;
    }
}
