using Bipolar.LoopedRooms;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    private SynchronizedGameObjectController synchronizedGameObjectController;

    public enum Type
    {
        Yellow,
        Red,
        Green, 
    };

    [SerializeField]
    private Type color;

    public void CollectKey()
    {
        KeysManager.instance.ChangeKeys(color, 1);
        synchronizedGameObjectController.SynchronizedGameObject.SetActive(false);
    }
}
