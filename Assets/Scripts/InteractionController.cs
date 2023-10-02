using Bipolar.RaycastSystem;
using NaughtyAttributes;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public event System.Action<InteractiveObject> OnInteractiveObjectSelected;
    public event System.Action<InteractiveObject> OnInteractiveObjectUnselected;

    [SerializeField]
    private RaycastController raycastController;

    [SerializeField, ReadOnly]
    private InteractiveObject currentInteractiveObject;

    private void OnEnable()
    {
        raycastController.OnRayEntered += RaycastController_OnRayEntered;
        raycastController.OnRayExited += RaycastController_OnRayExited;
    }

    private void RaycastController_OnRayEntered(RaycastTarget target)
    {
        if (target.TryGetComponent(out currentInteractiveObject))
            OnInteractiveObjectSelected?.Invoke(currentInteractiveObject);
    }

    private void RaycastController_OnRayExited(RaycastTarget target)
    {
        OnInteractiveObjectUnselected?.Invoke(currentInteractiveObject);
        currentInteractiveObject = null;
    }

    private void Update()
    {
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        if (currentInteractiveObject == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            currentInteractiveObject.Use();
        }
    }

    private void OnDisable()
    {
        raycastController.OnRayEntered -= RaycastController_OnRayEntered;
        raycastController.OnRayExited -= RaycastController_OnRayExited;
    }
}
