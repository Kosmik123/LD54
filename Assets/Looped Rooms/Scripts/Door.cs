using UnityEngine;

namespace Bipolar.LoopedRooms
{

    [System.Serializable]
    public struct DoorMapping
    {
        [field: SerializeField] 
        public DoorID Door1 { get; private set; }
        
        [field: SerializeField] 
        public DoorID Door2 {get; private set; }
    }

    public class Door : MonoBehaviour
    {
        [SerializeField]
        private DoorID id;
        public DoorID Id => id;


    }
}

