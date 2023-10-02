using UnityEngine;

public class KeysManager : MonoBehaviour
{
    public static KeysManager instance;

    [SerializeField]
    private int yellowKeys;
    [SerializeField]
    private int redKeys;
    [SerializeField]
    private int greenKeys;

    private void Awake()
    {
        instance = this;
    }

    public bool HasKey(Key.Type keyType)
    {
        int count = 0;
        switch (keyType)
        {
            case Key.Type.Yellow:
                count = yellowKeys;
                break;
            case Key.Type.Red:
                count = redKeys;
                break;
            case Key.Type.Green:
                count = greenKeys;
                break;
        }
        return count > 0;
    }

    public void ChangeKeys(Key.Type keyType, int count)
    {
        switch (keyType)
        {
            case Key.Type.Yellow:
                yellowKeys += count;
                break;
            case Key.Type.Red:
                redKeys += count;
                break;
            case Key.Type.Green:
                greenKeys += count;
                break;
        }
    }
}
