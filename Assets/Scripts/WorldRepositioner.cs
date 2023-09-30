using UnityEngine;

[DisallowMultipleComponent]
public class WorldRepositioner : MonoBehaviour
{
    [SerializeField]
    private Transform observer;
    [SerializeField]
    private float limitDistance;

    private void Update()
    {
        if (observer.position.sqrMagnitude > limitDistance * limitDistance)
        {
            ShiftWorld();
            enabled = false;
            Invoke(nameof(Reenable), 0.5f);
        }
    }

    private void Reenable()
    {
        enabled = true;
    }

    private void ShiftWorld()
    {
        Vector3 shift = -observer.position;
        for (int i = 0; i < transform.childCount; i++) 
            transform.GetChild(i).position += shift;
        
        observer.position = Vector3.zero;
    }
}
