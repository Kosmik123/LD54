using System;
using UnityEngine;

[SelectionBase]
public class RoomShaper : MonoBehaviour
{
    private const float halfSqrt3 = 0.86602540378f;

    [SerializeField]
    private float size;
    [SerializeField]
    private float wallThickness;
    
    [Space]
    [SerializeField]
    private Transform[] wallsPositions;

    private void OnValidate()
    {
        foreach (var wall in wallsPositions)
        {
            wall.localPosition = (size / 2 * halfSqrt3 - wallThickness / 2) * Vector3.forward;
        }
    }
}
