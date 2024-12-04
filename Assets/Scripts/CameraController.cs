using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform player; // The player to look at
    public float distanceFromLookPoint = 10f; // Distance behind where the camera looks
    public float mouseSensitivity = 100f;
    public float verticalClampMin = -30f;
    public float verticalClampMax = 60f;
    public float smoothSpeed = 10f;

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
            // Calculate the look point (player position, slightly above)
            Vector3 lookPoint = player.position + Vector3.up * 2f;

            // Rotate camera only when RMB is held
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
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            // Calculate the camera's rotation based on yaw and pitch
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

            // Calculate the camera's position: 10 units behind the look point in the opposite direction of the rotation
            Vector3 cameraPosition = lookPoint - rotation * Vector3.forward * distanceFromLookPoint;

            // Smoothly move the camera to the new position
            transform.position = Vector3.Lerp(transform.position, cameraPosition, smoothSpeed * Time.deltaTime);

            // Make the camera look at the look point
            transform.LookAt(lookPoint);
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
