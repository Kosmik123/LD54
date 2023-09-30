using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 4;

    [SerializeField]
    private float rotationSpeed = 15;

    [SerializeField]
    private Transform head;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float yaw = Input.GetAxis("Mouse X");
        float pitch = Input.GetAxis("Mouse Y");

        float dt = Time.deltaTime;

        transform.Rotate(Vector3.up, rotationSpeed * dt * yaw);
        head.Rotate(Vector3.right, rotationSpeed * dt * -pitch);
        transform.Translate(moveSpeed * dt * new Vector3(horizontal, 0, vertical));
    }
}
