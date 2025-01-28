using UnityEngine;

public class BlackHoleScreenQuad : MonoBehaviour
{
    public Camera mainCamera; // Assign your main camera
    public float depthOffset = 0.1f; // Slightly in front of near clip plane

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main; // Auto-assign if not set

        ResizeQuad();
    }

    void Update()
    {
        ResizeQuad();
    }

    void ResizeQuad()
    {
        float nearClip = mainCamera.nearClipPlane + depthOffset;
        transform.position = mainCamera.transform.position + mainCamera.transform.forward * nearClip;

        float height = 2.0f * nearClip * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float width = height * mainCamera.aspect;

        transform.localScale = new Vector3(width, height, 1);
        transform.rotation = mainCamera.transform.rotation; // Align with camera
    }
}
