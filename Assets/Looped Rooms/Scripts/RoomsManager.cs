using NaughtyAttributes;
using System;
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
        private Room nearestNeighbour;
        [SerializeField, ReadOnly]
        private List<Room> activeRooms;

        private readonly Dictionary<DoorID, DoorID> doorMappings = new Dictionary<DoorID, DoorID>();
        private readonly Dictionary<DoorID, Room> roomsByDoorID = new Dictionary<DoorID, Room>();

        private void Awake()
        {
            activeRooms = new List<Room>();
            LoadMappings();
            var room = roomsSpawner.GetRoom(startingRoomPrefab);
            activeRooms.Add(room);
            currentRoom = room;

            CreateMissingNeighbours(room);
        }

        private void CreateMissingNeighbours(Room room)
        {
            foreach (var door in room.Doors)
            {
                if (room.connectedRooms.ContainsKey(door))
                    continue;

                var neighbour = CreateRoomBehindDoor(door);
                activeRooms.Add(neighbour.room);

                room.connectedRooms[door] = neighbour.room;
                neighbour.room.connectedRooms[neighbour.door] = room;
            }
        }

        private (Room room, Door door) CreateRoomBehindDoor(Door door)
        {
            var roomPrototype = roomsByDoorID[door.Id];
            var room = roomsSpawner.GetRoom(roomPrototype);
            var otherDoorID = doorMappings[door.Id];
            var otherDoor = room.GetDoor(otherDoorID);

            var distance = Vector3.Distance(room.transform.position, otherDoor.transform.position);
            Vector3 roomPosition = door.transform.position + door.transform.rotation * Vector3.forward * distance;

            Quaternion roomRotation = Quaternion.Inverse(otherDoor.transform.rotation);
            roomRotation *= Quaternion.AngleAxis(180, Vector3.up);
            roomRotation *= door.transform.rotation;

            room.transform.SetPositionAndRotation(roomPosition, roomRotation);
            return (room, otherDoor);
        }

        private void Update()
        {
            activeRooms.Sort(RoomsToObserverDistanceComparison);
            UpdateCurrentRoom();
            nearestNeighbour = activeRooms[1];
        }

        private void UpdateCurrentRoom()
        {
            var nearestRoom = activeRooms[0];
            if (nearestRoom == currentRoom)
                return;

            var previousRoom = currentRoom;
            currentRoom = nearestRoom;

            DeleteAllConnectionsExceptOne(previousRoom, currentRoom);
            DeleteAllConnectionsExceptOne(currentRoom, previousRoom);

            for (int i = activeRooms.Count - 1; i >= 0; i--)
            {
                var room = activeRooms[i];
                if (room == previousRoom || room == currentRoom) 
                    continue;

                roomsSpawner.Release(room);
                activeRooms.RemoveAt(i);
            }

            CreateMissingNeighbours(currentRoom);
        }

        private int RoomsToObserverDistanceComparison(Room lhs, Room rhs)
        {
            float leftDistance = (lhs.transform.position - observer.transform.position).sqrMagnitude;
            float rightDistance = (rhs.transform.position - observer.transform.position).sqrMagnitude;
            return leftDistance.CompareTo(rightDistance);
        }

        private void DeleteAllConnectionsExceptOne(Room room, Room exception)
        {
            var door = room.connectedRooms.First(kvp => kvp.Value == exception).Key;
            room.connectedRooms.Clear();
            room.connectedRooms.Add(door, exception);
        }

        private void LoadMappings()
        {
            foreach (var doorPair in settings.DoorMappings)
            {
                doorMappings.Add(doorPair.Door1, doorPair.Door2);
                doorMappings.Add(doorPair.Door2, doorPair.Door1);
            }

            foreach (var room in settings.AllRoomsPrototypes)
            {
                var roomDoors = room.Doors;
                foreach (var door in roomDoors)
                {
                    roomsByDoorID.Add(door.Id, room);
                }
            }
        }
    }
}
