using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float distanceFromPlayer = 5f; // Desired distance between camera and player
    public float mouseSensitivity = 100f; // Mouse sensitivity
    public float verticalClampMin = -30f; // Minimum pitch (up/down) angle
    public float verticalClampMax = 60f;  // Maximum pitch angle
    public float smoothSpeed = 10f; // Speed of camera smoothing
    public LayerMask groundMask; // Layer mask for ground detection

    private float yaw = 0f; // Horizontal rotation (left/right)
    private float pitch = 0f; // Vertical rotation (up/down)

    void Start()
    {
        // Lock the cursor to the game window
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize camera rotation based on current rotation
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust the yaw (horizontal rotation) and pitch (vertical rotation)
        yaw += mouseX;
        pitch -= mouseY;

        // Clamp the vertical rotation to prevent flipping
        pitch = Mathf.Clamp(pitch, verticalClampMin, verticalClampMax);

        // Calculate the desired rotation
        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);

        // Calculate the target position of the camera
        Vector3 desiredPosition = player.position - (targetRotation * Vector3.forward * distanceFromPlayer);

        // Check for ground collision (Doesn't work yet)
        RaycastHit hit;
        if (Physics.Raycast(player.position, -Vector3.up, out hit, distanceFromPlayer, groundMask))
        {
            // Move the camera above the ground
            desiredPosition.y = hit.point.y + 0.5f; // Adjust the offset as needed
        }

        // Smoothly move the camera to the desired position and rotation
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(player.position);
    }
}
