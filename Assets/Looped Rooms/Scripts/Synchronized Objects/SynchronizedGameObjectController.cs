using UnityEngine;

namespace Bipolar.LoopedRooms
{
    public class SynchronizedGameObjectController : MonoBehaviour
    {
        [SerializeField]
        private SynchronizedGameObject synchronizedGameObject;
        public SynchronizedGameObject SynchronizedGameObject => synchronizedGameObject;

        private void Awake()
        {
            synchronizedGameObject.OnActiveChanged += UpdateActive;
        }

        private void Start()
        {
            Synchronize();
        }

        [ContextMenu("Synchronize")]
        public void Synchronize()
        {
            gameObject.SetActive(synchronizedGameObject.ActiveSelf);
        }

        private void UpdateActive(bool active)
        {
            gameObject.SetActive(active);
        }

        private void OnDestroy()
        {
            synchronizedGameObject.OnActiveChanged -= UpdateActive;
        }
    }
}
