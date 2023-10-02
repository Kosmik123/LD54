using UnityEngine;
using UnityEngine.Events;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField, Multiline(2)]
    private string hint;
    public virtual string Hint => hint;

    [SerializeField]
    private UnityEvent useAction;

    public void Use()
    {
        useAction.Invoke();
    }
}
