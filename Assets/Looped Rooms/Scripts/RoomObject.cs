using UnityEngine;
using UnityEngine.Events;

namespace Bipolar.LoopedRooms
{

    public class RoomObject : MonoBehaviour
    {
        private Room room;
        public Room Room
        {
            get
            {
                if (room == null)   
                    AssignRoom();
                return room;
            }
        }

        [SerializeField]
        private UnityEvent onRoomEnter;
        [SerializeField]
        private UnityEvent onRoomExit;

        private void AssignRoom()
        {
            room = GetComponentInParent<Room>();
        }

        private void Awake()
        {
            Room.OnRoomEntered += Room_OnRoomEntered;
            Room.OnRoomExited += Room_OnRoomExited;
        }

        private void Room_OnRoomExited()
        {
            onRoomExit.Invoke();
        }

        private void Room_OnRoomEntered()
        {
            onRoomEnter.Invoke();
        }

        private void OnDestroy()
        {
            Room.OnRoomEntered -= Room_OnRoomEntered;
            Room.OnRoomExited -= Room_OnRoomExited;
        }
    }
}
