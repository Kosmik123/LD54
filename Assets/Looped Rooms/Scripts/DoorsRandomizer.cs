using System.Collections.Generic;
using UnityEngine;

namespace Bipolar.LoopedRooms
{
    public class DoorsRandomizer : MonoBehaviour
    {
        [SerializeField]
        private Room[] rooms;

        private void Start()
        {
            RandomizeRooms();
        }

        [ContextMenu("Randomize")]
        private void RandomizeRooms()
        {
            foreach (var room in rooms)
            {
                Randomize(room);
            }
        }

        private void Randomize(Room room)
        {
            var doors = room.Doors;
            var doorIDs = new List<DoorID>();
            foreach (var door in doors)
                if (door)
                    doorIDs.Add(door.Id);

            foreach (var door in doors)
            {
                if (door == null)
                    continue;

                int randomIndex = Random.Range(0, doorIDs.Count);
                door.Id = doorIDs[randomIndex];
                doorIDs.RemoveAt(randomIndex);
            }
        }
    }
}
