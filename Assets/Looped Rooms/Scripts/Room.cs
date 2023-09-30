using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.LoopedRooms
{

    public class Room : MonoBehaviour
    {
        public event System.Action OnRoomInited;

        public struct Connection
        {
            public Door door;
            public Room room;

            public Connection(Door connectedDoor, Room connectedRoom)
            {
                door = connectedDoor;
                room = connectedRoom;
            }
        }

        private Room prototype;
        public Room Prototype => prototype;

        [SerializeField]
        private Transform[] wallsPositions;
        public IReadOnlyList<Transform> WallsPositions => wallsPositions;

        private Door[] doors;
        public IReadOnlyList<Door> Doors
        {
            get 
            {
                if (doors == null || doors.Length < 1)
                    PopulateDoors();
                return doors;
            }
        }

        private readonly Dictionary<DoorID, Door> doorsByID = new Dictionary<DoorID, Door>();
        public readonly Dictionary<Door, Connection> connections = new Dictionary<Door, Connection>();

        public void Init(Room prototype)
        {
            this.prototype = prototype;
            OnRoomInited?.Invoke();
        }

        private void PopulateDoors()
        {
            connections.Clear();
            doors = new Door[6];
            for (int i = 0; i < wallsPositions.Length; i++)
                doors[i] = wallsPositions[i].GetComponentInChildren<Door>();

            foreach (var door in doors)
                if (door)
                    doorsByID.Add(door.Id, door);
        }

        public Door GetDoor (DoorID id)
        {
            if (doors == null || doors.Length < 1)
                PopulateDoors();
            return doorsByID[id];
        }
        
        public bool HasDoor (DoorID id)
        {
            return doorsByID.ContainsKey(id);
        }

        public IReadOnlyList<Door> GetOppositeDoors(Door door)
        {
            var oppositeDoors =  new List<Door>();
            int doorIndex = System.Array.IndexOf(doors, door);
            for (int i = 2; i <= 4; i++)
            {
                int oppositeIndex = (doorIndex + i) % 6;
                var oppositeDoor = doors[oppositeIndex];
                if (oppositeDoor != null)
                    oppositeDoors.Add(oppositeDoor);
            }

            return oppositeDoors;
        }
    }
}
