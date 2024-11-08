using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public float initialHoldDelay = 0.5f; // Delay before starting continuous spawn
    public float continuousSpawnInterval = 0.01f; // Interval for continuous spawn

    private CubeSpawner currentSpawner;
    private bool isHoldingKey = false;
    private float holdTimer = 0f;
    private bool isSpawningContinuously = false;

    void Update()
    {
        // Check for interactable objects in front of the player
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactionDistance))
        {
            CubeSpawner spawner = hit.collider.GetComponent<CubeSpawner>();

            // If there is a CubeSpawner within range, assign it to currentSpawner
            if (spawner != null)
            {
                currentSpawner = spawner;

                // Spawn a single cube immediately if the key is tapped
                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentSpawner.SpawnSingleCubeInstance(); // Calling the method without arguments
                }

                // Check if the player is holding down the interact key
                if (Input.GetKey(KeyCode.E))
                {
                    if (!isHoldingKey)
                    {
                        isHoldingKey = true;
                        holdTimer = 0f;
                    }

                    // Increment the hold timer
                    holdTimer += Time.deltaTime;

                    // Start continuous spawning if delay has passed
                    if (holdTimer >= initialHoldDelay && !isSpawningContinuously)
                    {
                        StartCoroutine(SpawnContinuously());
                        isSpawningContinuously = true;
                    }
                }
                else
                {
                    // Reset when the key is released
                    isHoldingKey = false;
                    holdTimer = 0f;
                    isSpawningContinuously = false;
                    StopAllCoroutines(); // Stop continuous spawning
                }
            }
        }
        else
        {
            currentSpawner = null;
            isHoldingKey = false;
            holdTimer = 0f;
            isSpawningContinuously = false;
            StopAllCoroutines(); // Stop continuous spawning if out of range
        }
    }

    private IEnumerator SpawnContinuously()
    {
        while (true)
        {
            // Call the new method to spawn a random number of cubes from 1 to 10
            currentSpawner.SpawnMultipleCubes(); // No arguments passed
            yield return new WaitForSeconds(continuousSpawnInterval);
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize interaction distance in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * interactionDistance);
    }
}
