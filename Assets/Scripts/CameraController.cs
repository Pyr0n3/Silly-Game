using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float distanceFromPlayer = 5f; // Desired distance between camera and player
    public float mouseSensitivity = 100f; // Mouse sensitivity
    public float verticalClampMin = -30f; // Minimum pitch (up/down) angle
    public float verticalClampMax = 60f;  // Maximum pitch angle
    public float smoothSpeed = 10f; // Speed of camera smoothing
    public LayerMask groundMask; // Layer mask for ground detection
    public Button switchCameraButton; // Button to trigger camera switch
    public GameObject pausedCanvas; // Reference to the paused canvas
    public Button quitButton; // Button to quit the game

    private float yaw = 0f; // Horizontal rotation (left/right)
    private float pitch = 0f; // Vertical rotation (up/down)
    private bool isCameraActive = false; // Track if the camera is active
    private bool isPaused = false; // Track if the game is paused

    void Start()
    {
        Cursor.lockState = CursorLockMode.None; // Keep cursor unlocked initially
        Cursor.visible = true;

        // Initialize camera rotation based on current rotation
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        // Assign the button's onClick event
        switchCameraButton.onClick.AddListener(ActivateCamera); // Link the button to the function

        // Ensure the paused canvas is hidden at the start
        pausedCanvas.SetActive(false);

        // Assign the quit button event
        quitButton.onClick.AddListener(QuitGame); // Link the quit button to QuitGame function
    }

    void Update()
    {
        if (isCameraActive && !isPaused)
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

            // Check for ground collision (if necessary)
            RaycastHit hit;
            if (Physics.Raycast(player.position, -Vector3.up, out hit, distanceFromPlayer, groundMask))
            {
                // Move the camera above the ground
                desiredPosition.y = hit.point.y + 0.5f;
            }

            // Smoothly move the camera to the desired position and rotation
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.LookAt(player.position);
        }

        // Check if Escape is pressed to toggle pause state
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // Function to activate the camera and lock the cursor
    public void ActivateCamera()
    {
        isCameraActive = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Function to toggle camera and cursor lock/unlock states and pause the game
    private void TogglePause()
    {
        if (!isPaused)
        {
            // Pause the game
            isPaused = true;
            Time.timeScale = 0f; // Freeze the game
            Cursor.lockState = CursorLockMode.None; // Unlock cursor
            Cursor.visible = true;
            pausedCanvas.SetActive(true); // Show paused canvas
        }
        else
        {
            // Unpause the game
            isPaused = false;
            Time.timeScale = 1f; // Resume the game
            Cursor.lockState = CursorLockMode.Locked; // Lock cursor again
            Cursor.visible = false;
            pausedCanvas.SetActive(false); // Hide paused canvas
        }
    }

    // Function to quit the game
    public void QuitGame()
    {
        // Check if running in the editor or a build
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop the editor play mode
#else
        Application.Quit(); // Quit the application
#endif
    }
}
