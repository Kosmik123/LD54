using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Bipolar.LoopedRooms
{
    public class RoomsSpawner : MonoBehaviour
    {
        private readonly Dictionary<Room, ObjectPool<Room>> poolByPrototypes = new Dictionary<Room, ObjectPool<Room>>();

        public Room GetRoom(Room prototype)
        {
            if (poolByPrototypes.TryGetValue(prototype, out var pool) == false)
            {
                pool = new ObjectPool<Room>(() => SpawnRoom(prototype));
                poolByPrototypes.Add(prototype, pool);
            }

            var room = pool.Get();
            room.gameObject.SetActive(true);
            room.transform.rotation = Quaternion.identity;
            return room;
        }

        private Room SpawnRoom(Room prototype)
        {
            var room = Instantiate(prototype, transform);
            room.gameObject.name = $"{prototype.name} ({poolByPrototypes[prototype].CountAll})";
            room.Init(prototype);
            return room;
        }

        public void Release (Room room)
        {
            var pool = poolByPrototypes[room.Prototype];
            room.connections.Clear();
            room.gameObject.SetActive(false);
            pool.Release(room); 
        }
    }
}
