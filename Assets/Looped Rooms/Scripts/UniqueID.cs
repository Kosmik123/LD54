using NaughtyAttributes;
using System;
using UnityEngine;

namespace Bipolar.LoopedRooms
{
    public abstract class UniqueID : ScriptableObject
    {
        [SerializeField, HideInInspector]
        private byte[] guidBytes;

        [SerializeField, ReadOnly]
        private string guid;
        public Guid Guid => new Guid(guidBytes);

        protected virtual void Reset()
        {
            ResetID();
        }

        [ContextMenu("Reset ID")]
        private void ResetID()
        {
            guidBytes = Guid.NewGuid().ToByteArray();
            RefreshInspector();
        }

        private void OnValidate()
        {
            RefreshInspector();
        }

        private void RefreshInspector()
        {
            guid = Guid.ToString();
        }
    }
}
