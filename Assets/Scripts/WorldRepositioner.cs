using UnityEngine;

[DisallowMultipleComponent]
public class WorldRepositioner : MonoBehaviour
{
    [SerializeField]
    private Transform observer;
    [SerializeField]
    private float limitDistance;

    private bool oldAutoSyncTransforms;

    private void Update()
    {
        Vector3 observerHorizontalPosition = observer.position;
        observerHorizontalPosition.y = 0f;
        if (observerHorizontalPosition.sqrMagnitude > limitDistance * limitDistance)
        {
            ShiftWorld();
            enabled = false;
            Invoke(nameof(Reenable), 0.5f);
        }
    }

    private void Reenable()
    {
        enabled = true;
        Physics.autoSyncTransforms = oldAutoSyncTransforms;
    }

    private void ShiftWorld()
    {
        oldAutoSyncTransforms = Physics.autoSyncTransforms;
        Physics.autoSyncTransforms = true;
        Vector3 shift = -observer.position;
        //shift.y = 0;
        for (int i = 0; i < transform.childCount; i++) 
            transform.GetChild(i).position += shift;

        //observer.position += shift;
        observer.position = Vector3.zero;
        Physics.SyncTransforms();
    }
}
