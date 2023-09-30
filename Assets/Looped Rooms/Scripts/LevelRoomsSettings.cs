using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.LoopedRooms
{
    [CreateAssetMenu(menuName = "Looped Rooms/Level Settings")]
    public class LevelRoomsSettings : ScriptableObject
    {
        [SerializeField]
        private Room[] allRoomsPrototypes;
        public IReadOnlyList<Room> AllRoomsPrototypes => allRoomsPrototypes;

        [SerializeField]
        private DoorMapping[] doorMappings;
        public IReadOnlyList<DoorMapping> DoorMappings => doorMappings;
    }
}
