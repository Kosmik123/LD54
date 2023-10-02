using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 4;

    [SerializeField]
    private float rotationSpeed = 15;

    [SerializeField]
    private Transform head;

    private float headPitch = 0;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float yaw = Input.GetAxisRaw("Mouse X");
        float pitch = Input.GetAxisRaw("Mouse Y");

        float dt = Time.deltaTime;

        transform.Rotate(Vector3.up, rotationSpeed * dt * yaw);

        headPitch -= rotationSpeed * dt * pitch;
        headPitch = Mathf.Clamp(headPitch, -90, 90);
        head.localRotation = Quaternion.AngleAxis(headPitch, Vector3.right);
        transform.Translate(moveSpeed * dt * new Vector3(horizontal, 0, vertical));
    }
}
