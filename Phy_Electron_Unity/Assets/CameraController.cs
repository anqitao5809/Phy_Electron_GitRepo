using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform centerPoint;
    public float rotationSpeed = 5f;
    public float minYAngle = -20f;
    public float maxYAngle = 80f;
    public float zoomSpeed = 2f;
    public float minDistance = 2f;
    public float maxDistance = 15f;

    private float currentYAngle = 0f;
    private float currentDistance;

    private void Start()
    {
        // Initialize current distance based on the initial camera position
        currentDistance = Vector3.Distance(transform.position, centerPoint.position);
    }

    private void Update()
    {
        // Rotate the camera when the left mouse button is held down
        if (Input.GetMouseButton(0))
        {
            float horizontalInput = Input.GetAxis("Mouse X");
            float verticalInput = Input.GetAxis("Mouse Y");

            transform.RotateAround(centerPoint.position, Vector3.up, horizontalInput * rotationSpeed);

            float desiredYAngle = currentYAngle - verticalInput * rotationSpeed;
            desiredYAngle = Mathf.Clamp(desiredYAngle, minYAngle, maxYAngle);

            float rotationChange = desiredYAngle - currentYAngle;
            currentYAngle = desiredYAngle;

            transform.RotateAround(centerPoint.position, transform.right, rotationChange);

            // Keep the camera's orientation fixed (optional)
            transform.LookAt(centerPoint);
        }

        // Zoom the camera based on scroll input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            currentDistance -= scrollInput * zoomSpeed;
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

            Vector3 direction = (transform.position - centerPoint.position).normalized;
            transform.position = centerPoint.position + direction * currentDistance;
        }
    }
}
