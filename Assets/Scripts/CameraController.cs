using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform player; // The player's transform to follow
    public float distanceFromPlayer = 5f; // Distance from the player (horizontal)
    public float mouseSensitivity = 100f;
    public float verticalClampMin = -30f;
    public float verticalClampMax = 60f;
    public float smoothSpeed = 10f;
    public LayerMask groundMask;

    public GameObject pausedCanvas;
    public GameObject optionsCanvas;
    public GameObject inventoryUI;
    public Button quitButton;
    public Button optionsButton;
    public Button backButton;
    public Button switchCameraButton;

    private float yaw = 0f;
    private float pitch = 0f;
    private bool isCameraActive = false;
    private bool isPaused = false;
    private bool isOptionsMenuOpen = false;

    private Vector3 offset;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        switchCameraButton.onClick.AddListener(ActivateCamera);

        pausedCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        inventoryUI.SetActive(false);

        quitButton.onClick.AddListener(QuitGame);
        optionsButton.onClick.AddListener(OpenOptionsMenu);
        backButton.onClick.AddListener(CloseOptionsMenu);
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Calculate the camera's position relative to the player (distanceFromPlayer behind the player)
            offset = new Vector3(0, 5f, -distanceFromPlayer); // You can adjust the height (5f) as needed.
            Vector3 desiredPosition = player.position + offset;

            // Update the camera position smoothly
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // Rotate camera based on mouse input (only when RMB is held)
            if (isCameraActive && Input.GetMouseButton(1) && !isPaused && !isOptionsMenuOpen && !inventoryUI.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                // Get mouse movement input for rotation
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

                yaw += mouseX;
                pitch -= mouseY;
                pitch = Mathf.Clamp(pitch, verticalClampMin, verticalClampMax);

                // Apply the rotation based on the mouse input
                Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);
                transform.rotation = targetRotation;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            // Always make the camera look at the player (slightly above their position)
            transform.LookAt(player.position + Vector3.up * 2f); // Looking slightly above the player
        }
    }

    void Update()
    {
        // Open/close inventory UI
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            Cursor.lockState = inventoryUI.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = inventoryUI.activeSelf;
        }

        // Pause menu logic
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOptionsMenuOpen)
            {
                CloseOptionsMenu();
            }
            else
            {
                TogglePause();
            }
        }
    }

    public void ActivateCamera()
    {
        isCameraActive = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
        pausedCanvas.SetActive(isPaused);
    }

    private void OpenOptionsMenu()
    {
        isOptionsMenuOpen = true;
        optionsCanvas.SetActive(true);
        pausedCanvas.SetActive(false);
    }

    private void CloseOptionsMenu()
    {
        isOptionsMenuOpen = false;
        optionsCanvas.SetActive(false);
        pausedCanvas.SetActive(true);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
