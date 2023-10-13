using Bipolar.Input;
using UnityEngine;

public class ConstantlyForward : MonoBehaviour, IMoveInputProvider
{
    public Vector2 GetMotion()
    {
        return Vector2.up;
    }
}
