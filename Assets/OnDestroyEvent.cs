using UnityEngine;
using UnityEngine.Events;

public class OnDestroyEvent : MonoBehaviour
{
    public event System.Action OnDestroyed;

    [SerializeField]
    private UnityEvent onDestroy;

    private void OnDestroy()
    {
        onDestroy.Invoke();
        OnDestroyed?.Invoke();
    }
}
