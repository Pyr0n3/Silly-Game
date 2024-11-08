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
    public Button switchCameraButton;
    public GameObject pausedCanvas;
    public Button quitButton;
    public GameObject optionsCanvas;
    public Button optionsButton;
    public Button backButton;

    private float yaw = 0f;
    private float pitch = 0f;
    private bool isCameraActive = false;
    private bool isPaused = false;
    private bool isOptionsMenuOpen = false;

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0f;
    private Vector3 shakeOffset = Vector3.zero;

    private Vector3 originalPosition;

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

        quitButton.onClick.AddListener(QuitGame);
        optionsButton.onClick.AddListener(OpenOptionsMenu);
        backButton.onClick.AddListener(CloseOptionsMenu);

        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (isCameraActive && !isPaused && !isOptionsMenuOpen)
        {
            // Get mouse input and adjust yaw/pitch for free camera movement
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, verticalClampMin, verticalClampMax);

            Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 desiredPosition = player.position - (targetRotation * Vector3.forward * distanceFromPlayer);

            // Prevent the camera from going below ground
            RaycastHit hit;
            if (Physics.Raycast(player.position, -Vector3.up, out hit, distanceFromPlayer, groundMask))
            {
                if (desiredPosition.y < hit.point.y + 0.5f)
                {
                    desiredPosition.y = hit.point.y + 0.5f;
                }
            }

            // Apply screen shake effect temporarily if shake is active
            if (shakeDuration > 0)
            {
                shakeOffset = Random.insideUnitSphere * shakeMagnitude;
                shakeDuration -= Time.deltaTime;
            }
            else
            {
                shakeOffset = Vector3.zero; // Reset shake offset when shake is inactive
            }

            // Smoothly move the camera to the desired position with any active shake offset
            transform.position = Vector3.Lerp(transform.position, desiredPosition + shakeOffset, smoothSpeed * Time.deltaTime);
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

    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }

    public void ActivateCamera()
    {
        isCameraActive = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void TogglePause()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pausedCanvas.SetActive(true);
            optionsCanvas.SetActive(false);
        }
        else
        {
            isPaused = false;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pausedCanvas.SetActive(false);
        }
    }

    public void OpenOptionsMenu()
    {
        if (!isPaused) return;

        isPaused = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pausedCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
        isOptionsMenuOpen = true;
    }

    public void CloseOptionsMenu()
    {
        optionsCanvas.SetActive(false);
        pausedCanvas.SetActive(true);
        isOptionsMenuOpen = false;

        isPaused = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
