using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.LoopedRooms
{
    public class VisitedRoomsTracker : MonoBehaviour
    {
        [SerializeField]
        private RoomsManager roomsManager;

        [SerializeField, ReadOnly]
        private List<Room> visitedRooms;
        public IReadOnlyList<Room> VisitedRooms => visitedRooms;

        private void OnEnable()
        {
            roomsManager.OnRoomEntered += RoomsManager_OnRoomEntered;
        }

        private void RoomsManager_OnRoomEntered(Room room)
        {
            if (visitedRooms.Contains(room.Prototype) == false)
                visitedRooms.Add(room.Prototype);
        }

        private void OnDisable()
        {
            roomsManager.OnRoomEntered -= RoomsManager_OnRoomEntered;
        }
    }
}
