using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.LoopedRooms
{
    public class Room : MonoBehaviour
    {
        public event System.Action OnRoomInited;
        public event System.Action OnRoomEntered;
        public event System.Action OnRoomExited;

        [System.Serializable]
        public struct Connection
        {
            public Passage passage;
            public Room room;

            public Connection(Passage connectedDoor, Room connectedRoom)
            {
                passage = connectedDoor;
                room = connectedRoom;
            }
        }

        private Room prototype;
        public Room Prototype => prototype;

        [SerializeField]
        private Transform[] wallsPositions;
        public IReadOnlyList<Transform> WallsPositions => wallsPositions;

        private Passage[] passages;
        public IReadOnlyList<Passage> Passages
        {
            get
            {
                if (passages == null || passages.Length < 1)
                    PopulatePassages();
                return passages;
            }
        }

        private readonly Dictionary<PassageID, Passage> passagesByID = new Dictionary<PassageID, Passage>();
        public Dictionary<Passage, Connection> Connections { get; } = new Dictionary<Passage, Connection>();

        public void Init(Room prototype)
        {
            this.prototype = prototype;
            OnRoomInited?.Invoke();
        }

        private void PopulatePassages()
        {
            Connections.Clear();
            passages = new Passage[6];
            for (int i = 0; i < wallsPositions.Length; i++)
                passages[i] = wallsPositions[i].GetComponentInChildren<Passage>();

            foreach (var passage in passages)
            {
                if (passage)
                {
                    if (passage.Id == null)
                        Debug.LogError($"Room {name} has null passage");

                    passagesByID.Add(passage.Id, passage);
                }
            }
        
        }

        public Passage GetPassage(PassageID id)
        {
            if (passages == null || passages.Length < 1)
                PopulatePassages();
            return passagesByID[id];
        }

        public bool HasPassage(PassageID id)
        {
            return passagesByID.ContainsKey(id);
        }

        public IReadOnlyList<Passage> GetOppositePassages(Passage passage)
        {
            var oppositePassages = new List<Passage>();
            int passageIndex = System.Array.IndexOf(passages, passage);
            for (int i = 2; i <= 4; i++)
            {
                int oppositeIndex = (passageIndex + i) % 6;
                var oppositeDoor = passages[oppositeIndex];
                if (oppositeDoor != null)
                    oppositePassages.Add(oppositeDoor);
            }

            return oppositePassages;
        }

        public void Enter()
        {
            OnRoomEntered?.Invoke();
        }

        public void Exit()
        {
            OnRoomExited?.Invoke();
        }

        public override string ToString()
        {
            return gameObject.name;
        }
    }
}
