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
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        // Set initial position and rotation for isometric view
        Vector3 direction = (new Vector3(30, 30, -30)).normalized;
        initialPosition = centerPoint.position + direction * 30f;  // 5 units away from the center
        initialRotation = Quaternion.LookRotation(centerPoint.position - initialPosition);

        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Initialize current distance
        currentDistance = Vector3.Distance(transform.position, centerPoint.position);
    }

    private void Update()
    {
        // Reset the camera position and rotation if the game is paused
        if (!Application.isPlaying)
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            currentDistance = Vector3.Distance(transform.position, centerPoint.position);
            return;
        }

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

            // Keep the camera's orientation fixed
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
