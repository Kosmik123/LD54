using UnityEngine;

[ExecuteInEditMode]
public class RandomRotationInEditor : MonoBehaviour
{
    private void Start()
    {
        transform.localRotation = Quaternion.AngleAxis(Random.Range(-180, 180), Vector2.up);       
    }
}
