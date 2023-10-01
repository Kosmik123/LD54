using Bipolar.LoopedRooms;
using NaughtyAttributes;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    [SerializeField]
    private float radius;
    [SerializeField]
    private float speed;
    [SerializeField]
    private SynchronizedTransform synchronizedTransform;

    [SerializeField, ReadOnly]
    private float angle;

    private void Update()
    {
        angle += Time.deltaTime * speed;
        if (angle > 180)
            angle -= 360;
        else if (angle < -180)
            angle += 360;

        synchronizedTransform.LocalPosition = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * radius;
    }
}
