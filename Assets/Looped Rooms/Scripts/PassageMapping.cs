using UnityEngine;

namespace Bipolar.LoopedRooms
{
    [System.Serializable]
    public struct PassageMapping
    {
        [field: SerializeField] 
        public PassageID Passage1 { get; private set; }
        
        [field: SerializeField] 
        public PassageID Passage2 {get; private set; }
    }
}
