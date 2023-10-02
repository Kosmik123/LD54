using Bipolar.LoopedRooms;
using NaughtyAttributes;
using UnityEngine;

public class WallsHelper : MonoBehaviour
{
    [Button]
    private void SetCorrectMesh()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var wall = transform.GetChild(i);
            bool isPassage = wall.GetComponentInChildren<Passage>();
            var allChildren = wall.GetComponentsInChildren<MeshFilter>(true);
            foreach (var child in allChildren)
            {
                if (child.gameObject.name == "Wall")
                    child.gameObject.SetActive(!isPassage);
                else if (child.gameObject.name == "Hole")
                    child.gameObject.SetActive(isPassage);
            }
        }
    }
}
