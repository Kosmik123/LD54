﻿using UnityEngine;

namespace Bipolar.LoopedRooms
{
    public class Door : MonoBehaviour
    {
        [SerializeField]
        private DoorID id;
        public DoorID Id => id;
    }
}
