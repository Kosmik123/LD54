using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.LoopedRooms
{
    public class Room : MonoBehaviour
    {
        private Room prototype;
        public Room Prototype => prototype;

        [SerializeField]
        private Door[] doors;
        public IReadOnlyList<Door> Doors => doors;

        private readonly Dictionary<DoorID, Door> doorsByID = new Dictionary<DoorID, Door>();

        public readonly Dictionary<Door, Room> connectedRooms = new Dictionary<Door, Room>();

        [ContextMenu("Gather Doors")]
        private void GatherDoors()
        {
            doors = GetComponentsInChildren<Door>();
        }

        public void Init(Room prototype)
        {
            this.prototype = prototype;
        }

        private void Awake()
        {
            connectedRooms.Clear();
            foreach (var door in doors)
            {
                doorsByID.Add(door.Id, door);
            }
        }

        public Door GetDoor (DoorID id)
        {
            return doorsByID[id];
        }
    }
}
