using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bipolar.LoopedRooms
{
    public class RoomsManager : MonoBehaviour
    {
        public event System.Action<Room> OnRoomEntered;

        [Header("Settings")]
        [SerializeField]
        private LevelRoomsSettings settings;
        [SerializeField]
        private RoomsSpawner roomsSpawner;
        [SerializeField]
        private Room[] startingRoomPrefabs;
        [SerializeField]
        private Transform observer;

        [Header("States")]
        [SerializeField, ReadOnly]
        private Room currentRoom;
        [SerializeField, ReadOnly]
        private Room nearestNeighbour = null;
        [SerializeField, ReadOnly]
        private List<Room> activeRooms;

        private readonly Dictionary<PassageID, PassageID> passagesMappings = new Dictionary<PassageID, PassageID>();
        private readonly Dictionary<PassageID, Room> roomPrototypesByPassageID = new Dictionary<PassageID, Room>();

        [SerializeField, ReadOnly]
        private List<Room.Connection> roomsBehindPassagesToLoad = new List<Room.Connection>();
        public bool IsLoading => roomsBehindPassagesToLoad.Count > 0;

        public void TeleportToRoom(Room room)
        {
            observer.transform.position = Vector3.zero;
            room = room.Prototype != null ? room.Prototype : room;
            nearestNeighbour = null;
            foreach (var r in activeRooms)
                roomsSpawner.Release(r);

            activeRooms.Clear();

            room = roomsSpawner.GetRoom(room);
            room.transform.position = Vector3.zero;
            activeRooms.Add(room);
            SetCurrentRoom(room);
        }

        private void Awake()
        {
            activeRooms = new List<Room>();
            LoadMappings();

            int randomIndex = Random.Range(0, startingRoomPrefabs.Length);
            var startingRoomPrototype = startingRoomPrefabs[randomIndex];
            var room = roomsSpawner.GetRoom(startingRoomPrototype);
            room.transform.position = Vector3.zero;
            activeRooms.Add(room);
            SetCurrentRoom(room);
        }

        private void LoadMappings()
        {
            foreach (var doorPair in settings.PassageMappings)
            {
                if (doorPair.Passage1 == doorPair.Passage2)
                {
                    passagesMappings.Add(doorPair.Passage1, doorPair.Passage2);
                    Debug.Log($"Passage {doorPair.Passage1.name} is self-passing");
                }
                else
                {
                    passagesMappings.Add(doorPair.Passage1, doorPair.Passage2);
                    passagesMappings.Add(doorPair.Passage2, doorPair.Passage1);
                }
            }

            foreach (var roomPrototype in settings.AllRoomsPrototypes)
                foreach (var passage in roomPrototype.Passages)
                    if (passage)
                        AddPassageToRoomMapping(roomPrototype, passage.Id);

            foreach (var additionalMapping in settings.AdditionalMappings)
                foreach (var passageID in additionalMapping.Passages)
                    AddPassageToRoomMapping(additionalMapping.Room, passageID);

        }

        public void AddPassageToRoomMapping(Room roomPrototype, PassageID passageID)
        {
            int p = 0;
            if (roomPrototypesByPassageID.ContainsKey(passageID))
            {
                Debug.LogError($"{roomPrototype.name} contains already existing passage {passageID.name}");
            }
            else
            {
                roomPrototypesByPassageID.Add(passageID, roomPrototype);
            }
            p++;
        }

        private void LoadMissingNeighbours(Room room)
        {
            var passages = room.Passages;
            LoadNeighbours(room, passages);
        }

        private void LoadNeighbours(Room room, IReadOnlyList<Passage> passages)
        {
            foreach (var passage in passages)
            {
                if (passage == null)
                    continue;

                if (room.Connections.ContainsKey(passage))
                    continue;

                roomsBehindPassagesToLoad.Add(new Room.Connection(passage, room));
            }
        }

        private void LoadRoomBehindDoor(Room room, Passage passage)
        {
            var neighbour = CreateRoomBehindDoor(passage);
            activeRooms.Add(neighbour.room);

            room.Connections[passage] = new Room.Connection(neighbour.passage, neighbour.room);
            neighbour.room.Connections[neighbour.passage] = new Room.Connection(passage, room);
        }

        private Room.Connection CreateRoomBehindDoor(Passage passage)
        {
            var otherDoorID = passagesMappings[passage.Id];
            var roomPrototype = roomPrototypesByPassageID[otherDoorID];
            var room = roomsSpawner.GetRoom(roomPrototype);
            var otherPassage = room.GetPassage(otherDoorID);

            var distance = Vector3.Distance(room.transform.position, otherPassage.transform.position);
            Vector3 roomPosition = passage.transform.position + passage.transform.rotation * Vector3.forward * distance;

            Quaternion roomRotation = Quaternion.Inverse(otherPassage.transform.rotation);
            roomRotation *= Quaternion.AngleAxis(180, Vector3.up);
            roomRotation *= passage.transform.rotation;

            room.transform.SetPositionAndRotation(roomPosition, roomRotation);
            return new Room.Connection(otherPassage, room);
        }

        private void Update()
        {
            activeRooms.Sort(RoomsToObserverDistanceComparison);

            if (IsLoading)
            {
                var roomAndPassage = roomsBehindPassagesToLoad[0];
                roomsBehindPassagesToLoad.RemoveAt(0);
                LoadRoomBehindDoor(roomAndPassage.room, roomAndPassage.passage);
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

            currentRoom.Exit();
            var previousRoom = currentRoom;

            foreach (var connection in previousRoom.Connections)
            {
                var room = connection.Value.room;
                if (room == nearestRoom)
                    continue;

                roomsSpawner.Release(room);
                activeRooms.Remove(room);
            }
            DeleteAllConnectionsExceptOne(previousRoom, nearestRoom);
            SetCurrentRoom(nearestRoom);
        }

        private void SetCurrentRoom(Room room)
        {
            currentRoom = room;
            LoadMissingNeighbours(currentRoom);
            currentRoom.Enter();
            OnRoomEntered?.Invoke(currentRoom);
        }

        private void UpdateNearestNeighbour()
        {
            if (nearestNeighbour == activeRooms[1])
                return;
            
            if (nearestNeighbour != null && nearestNeighbour != currentRoom)
            {
                foreach (var connection in nearestNeighbour.Connections.Values)
                {
                    if (connection.room == currentRoom)
                        continue;

                    activeRooms.Remove(connection.room);
                    roomsSpawner.Release(connection.room);
                }
                DeleteAllConnectionsExceptOne(nearestNeighbour, currentRoom);
            }

            nearestNeighbour = activeRooms[1];

            var connectionToCurrentRoom = nearestNeighbour.Connections.First(kvp => kvp.Value.room == currentRoom);
            var oppositeDoors = nearestNeighbour.GetOppositePassages(connectionToCurrentRoom.Key);

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
            var passage = room.Connections.FirstOrDefault(kvp => kvp.Value.room == exception).Key;
            if (passage == null)
                Debug.LogError("BRAK POŁĄCZEŃ?");
            var exceptionConnection = room.Connections[passage];
            room.Connections.Clear();
            room.Connections.Add(passage, exceptionConnection);
        }
    }
}
