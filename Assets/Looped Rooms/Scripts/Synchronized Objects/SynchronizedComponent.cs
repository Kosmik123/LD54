using UnityEngine;

namespace Bipolar.LoopedRooms
{
    public abstract class SynchronizedComponent<T> : MonoBehaviour where T : Component
    {
        [SerializeField]
        private SynchronizedID id;
        public SynchronizedID ID
        {
            get { return id; }
            set 
            {
                if (id == value)
                    return;

                if (id)
                    id.RemoveAction(GetType(), Synchronize);

                id = value;
                if (id)
                    id.AddAction(GetType(), Synchronize);
            }
        }

        private void Synchronize()
        {
            OnSynchronize();
        }

        protected abstract void OnSynchronize();

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

    }
}
