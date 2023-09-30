using Bipolar.LoopedRooms;
using UnityEngine;

[SelectionBase]
public class RoomShaper : MonoBehaviour
{
    private const float halfSqrt3 = 0.86602540378f;

    [SerializeField]
    private Room room;
    [SerializeField]
    private float size;
    [SerializeField]
    private float wallThickness;

    private void OnValidate()
    {
        foreach (var wall in room.WallsPositions)
        {
            wall.localPosition = (size / 2 * halfSqrt3 - wallThickness / 2) * Vector3.forward;
        }
    }
}
