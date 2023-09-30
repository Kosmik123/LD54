using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bipolar.LoopedRooms
{
    public class RoomsManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private LevelRoomsSettings settings;
        [SerializeField]
        private RoomsSpawner roomsSpawner;
        [SerializeField]
        private Room startingRoomPrefab;
        [SerializeField]
        private Transform observer;

        [Header("States")]
        [SerializeField, ReadOnly]
        private Room currentRoom;
        [SerializeField, ReadOnly]
        private Room nearestNeighbour = null;
        [SerializeField, ReadOnly]
        private List<Room> activeRooms;

        private readonly Dictionary<DoorID, DoorID> doorMappings = new Dictionary<DoorID, DoorID>();
        private readonly Dictionary<DoorID, Room> roomPrototypesByDoorID = new Dictionary<DoorID, Room>();

        [SerializeField, ReadOnly]
        private List<Room.Connection> roomsBehindDoorsToLoad = new List<Room.Connection>();
        public bool IsLoading => roomsBehindDoorsToLoad.Count > 0;

        private void Awake()
        {
            activeRooms = new List<Room>();
            LoadMappings();
            var room = roomsSpawner.GetRoom(startingRoomPrefab);
            room.transform.position = Vector3.zero;
            activeRooms.Add(room);
            currentRoom = room;

            LoadMissingNeighbours(room);
        }

        private void LoadMappings()
        {
            foreach (var doorPair in settings.DoorMappings)
            {
                doorMappings.Add(doorPair.Door1, doorPair.Door2);
                doorMappings.Add(doorPair.Door2, doorPair.Door1);
            }

            foreach (var roomPrototype in settings.AllRoomsPrototypes)
            {
                var roomDoors = roomPrototype.Doors;
                foreach (var door in roomDoors)
                    if (door)
                        roomPrototypesByDoorID.Add(door.Id, roomPrototype);
            }
        }

        private void LoadMissingNeighbours(Room room)
        {
            var doors = room.Doors;
            LoadNeighbours(room, doors);
        }

        private void LoadNeighbours(Room room, IReadOnlyList<Door> doors)
        {
            foreach (var door in doors)
            {
                if (door == null)
                    continue;

                if (room.connections.ContainsKey(door))
                    continue;

                roomsBehindDoorsToLoad.Add(new Room.Connection(door, room));
            }
        }

        private void LoadRoomBehindDoor(Room room, Door door)
        {
            var neighbour = CreateRoomBehindDoor(door);
            activeRooms.Add(neighbour.room);

            room.connections[door] = new Room.Connection(neighbour.door, neighbour.room);
            neighbour.room.connections[neighbour.door] = new Room.Connection(door, room);
        }

        private Room.Connection CreateRoomBehindDoor(Door door)
        {
            var otherDoorId = doorMappings[door.Id];
            var roomPrototype = roomPrototypesByDoorID[otherDoorId];
            var room = roomsSpawner.GetRoom(roomPrototype);
            var otherDoorID = doorMappings[door.Id];
            var otherDoor = room.GetDoor(otherDoorID);

            var distance = Vector3.Distance(room.transform.position, otherDoor.transform.position);
            Vector3 roomPosition = door.transform.position + door.transform.rotation * Vector3.forward * distance;

            Quaternion roomRotation = Quaternion.Inverse(otherDoor.transform.rotation);
            roomRotation *= Quaternion.AngleAxis(180, Vector3.up);
            roomRotation *= door.transform.rotation;

            room.transform.SetPositionAndRotation(roomPosition, roomRotation);
            return new Room.Connection(otherDoor, room);
        }

        private void Update()
        {
            activeRooms.Sort(RoomsToObserverDistanceComparison);

            if (IsLoading)
            {
                var roomAndDoor = roomsBehindDoorsToLoad[0];
                roomsBehindDoorsToLoad.RemoveAt(0);
                LoadRoomBehindDoor(roomAndDoor.room, roomAndDoor.door);
            }
            else
            {
                UpdateCurrentRoom();
                UpdateNearestNeighbour();
            }
        }

        private void UpdateCurrentRoom()
        {
            var nearestRoom = activeRooms[0];
            if (nearestRoom == currentRoom)
                return;

            var previousRoom = currentRoom;
            currentRoom = nearestRoom;

            foreach (var connection in previousRoom.connections)
            {
                var room = connection.Value.room;
                if (room == currentRoom)
                    continue;

                roomsSpawner.Release(room);
                activeRooms.Remove(room);
            }

            DeleteAllConnectionsExceptOne(previousRoom, currentRoom);
            //DeleteAllConnectionsExceptOne(currentRoom, previousRoom);
            //for (int i = activeRooms.Count - 1; i >= 0; i--)
            //{
            //    var room = activeRooms[i];
            //    if (room == previousRoom || room == currentRoom) 
            //        continue;

            //    roomsSpawner.Release(room);
            //    activeRooms.RemoveAt(i);
            //}

            LoadMissingNeighbours(currentRoom);
        }

        private void UpdateNearestNeighbour()
        {
            if (nearestNeighbour == activeRooms[1])
                return;
            
            if (nearestNeighbour != null && nearestNeighbour != currentRoom)
            {
                foreach (var connection in nearestNeighbour.connections.Values)
                {
                    if (connection.room == currentRoom)
                        continue;

                    activeRooms.Remove(connection.room);
                    roomsSpawner.Release(connection.room);
                }
                DeleteAllConnectionsExceptOne(nearestNeighbour, currentRoom);
            }

            nearestNeighbour = activeRooms[1];

            var connectionToCurrentRoom = nearestNeighbour.connections.First(kvp => kvp.Value.room == currentRoom);
            var oppositeDoors = nearestNeighbour.GetOppositeDoors(connectionToCurrentRoom.Key);

            LoadNeighbours(nearestNeighbour, oppositeDoors);
        }

        private int RoomsToObserverDistanceComparison(Room lhs, Room rhs)
        {
            float leftDistance = (lhs.transform.position - observer.transform.position).sqrMagnitude;
            float rightDistance = (rhs.transform.position - observer.transform.position).sqrMagnitude;
            return leftDistance.CompareTo(rightDistance);
        }

        private void DeleteAllConnectionsExceptOne(Room room, Room exception)
        {
            var door = room.connections.First(kvp => kvp.Value.room == exception).Key;
            var exceptionConnection = room.connections[door];
            room.connections.Clear();
            room.connections.Add(door, exceptionConnection);
        }
    }
}
