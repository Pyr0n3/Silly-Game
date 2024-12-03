using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float distanceFromPlayer = 5f;
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

    private float yaw = 0f;
    private float pitch = 0f;
    private bool isPaused = false;
    private bool isOptionsMenuOpen = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        pausedCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        inventoryUI.SetActive(false);

        quitButton.onClick.AddListener(QuitGame);
        optionsButton.onClick.AddListener(OpenOptionsMenu);
        backButton.onClick.AddListener(CloseOptionsMenu);
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

        if (!inventoryUI.activeSelf && !isPaused && !isOptionsMenuOpen)
        {
            // Handle camera movement
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, verticalClampMin, verticalClampMax);

            Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 desiredPosition = player.position - (targetRotation * Vector3.forward * distanceFromPlayer);

            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.LookAt(player.position);
        }

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
