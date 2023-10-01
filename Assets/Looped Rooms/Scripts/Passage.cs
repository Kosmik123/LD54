using UnityEngine;

namespace Bipolar.LoopedRooms
{
    public class Passage : MonoBehaviour
    {
        [SerializeField]
        private PassageID id;
        public PassageID Id
        {
            get => id;
            set
            {
                id = value;
            }
        }
    }
}
