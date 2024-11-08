using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private Vector3 originalPosition;
    private bool isShaking = false;
    private float shakeDuration = 1f;
    private float shakeMagnitude = .5f;

    void Start()
    {
        // Store the original position of the camera at the start
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (isShaking)
        {
            // Apply random offset for shake effect
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;

            // Reduce shake duration over time
            shakeDuration -= Time.deltaTime;

            if (shakeDuration <= 0f)
            {
                // Reset to original position once shaking stops
                isShaking = false;
                transform.localPosition = originalPosition;
            }
        }
    }

    // Method to trigger the shake effect
    public void TriggerShake(float duration, float magnitude)
    {
        if (!isShaking)
        {
            // Only set the original position once when shake starts
            originalPosition = transform.localPosition;
        }

        shakeDuration = duration;
        shakeMagnitude = magnitude;
        isShaking = true;
    }
}
