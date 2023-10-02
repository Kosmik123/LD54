using Bipolar.LoopedRooms;
using UnityEngine;

public class Room6PassageChanger : MonoBehaviour
{
    [SerializeField]
    private Room room;

    [SerializeField]
    private Door[] doors;

    [SerializeField]
    private PassageID firstPassage;
    [SerializeField]
    private PassageID secondPassage;

    private void Awake()
    {
        foreach (var door in doors)
        {
            door.OnDoorOpened += Door_OnDoorOpened;
        }    
    }

    private void Door_OnDoorOpened(Door door)
    {
        foreach (var d in doors)
            d.OnDoorOpened -= Door_OnDoorOpened;

        SetDoorPassage(door, firstPassage);
        foreach (var otherDoor in doors)
        {
            if (otherDoor == door)
                continue;

            SetDoorPassage(otherDoor, secondPassage);
        }

        var roomPrototype = room.Prototype;
        var prototypeDoors = roomPrototype.GetComponentsInChildren<Door>();
        foreach (var prototypeDoor in prototypeDoors)
        {
            if (prototypeDoor.RotatingTransform == door.RotatingTransform)
                SetDoorPassage(prototypeDoor, firstPassage);
            else 
                SetDoorPassage(prototypeDoor, secondPassage);
        }
    }

    private void SetDoorPassage(Door door, PassageID passageID)
    {
        var passage = door.GetComponentInParent<Passage>();
        passage.Id = passageID;
    }
}
