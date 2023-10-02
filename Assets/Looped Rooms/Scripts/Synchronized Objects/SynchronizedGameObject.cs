using NaughtyAttributes;
using UnityEngine;

namespace Bipolar.LoopedRooms
{
    [CreateAssetMenu(menuName = "Looped Rooms/Synchronized Game Object")]
    public class SynchronizedGameObject : UniqueID
    {
        public event System.Action<bool> OnActiveChanged;

        [SerializeField]
        public bool active;
        public bool ActiveSelf => active;

        private void OnEnable()
        {
            active = true;
        }

        public void SetActive(bool active)
        {
            this.active = active;
            OnActiveChanged?.Invoke(active);
        }
    }
}
