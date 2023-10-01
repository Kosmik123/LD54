using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bipolar.LoopedRooms
{
    [CreateAssetMenu(menuName = "Looped Rooms/Level Settings")]
    public class LevelRoomsSettings : ScriptableObject
    {
        [SerializeField]
        private Room[] allRoomsPrototypes;
        public IReadOnlyList<Room> AllRoomsPrototypes => allRoomsPrototypes;

        [SerializeField, FormerlySerializedAs("doorMappings")]
        private PassageMapping[] passageMappings;
        public IReadOnlyList<PassageMapping> PassageMappings => passageMappings;
    }
}
