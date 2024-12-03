using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 10f;  // Distance to interact with CubeSpawner
    public float pickupDistance = 100f;      // Distance to pick up cubes
    public float initialHoldDelay = 0.5f;    // Delay before starting continuous spawn
    public float continuousSpawnInterval = 0.01f; // Interval for continuous spawn
    public InventoryManager inventoryManager; // Reference to the inventory manager

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
                if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire3gamepad"))
                {
                    currentSpawner.SpawnSingleCubeInstance(); // Call spawn method from CubeSpawner
                }

                // Check if the player is holding down the interact key
                if (Input.GetKey(KeyCode.E) || Input.GetButton("Fire3gamepad"))
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

        // Left-click to pick up a cube (within pickup distance)
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit cubeHit, pickupDistance))
            {
                if (cubeHit.collider.CompareTag("Cube"))
                {
                    string cubeType = cubeHit.collider.gameObject.name.Replace("CubePrefab", "").Trim();
                    bool added = inventoryManager.AddToInventory(cubeType);

                    if (added)
                    {
                        Destroy(cubeHit.collider.gameObject); // Remove the cube
                    }
                    else
                    {
                        Debug.Log("Inventory is full!");
                    }
                }
            }
        }
    }

    private IEnumerator SpawnContinuously()
    {
        while (true)
        {
            // Spawn multiple cubes during continuous spawn
            currentSpawner.SpawnMultipleCubes(5);
            yield return new WaitForSeconds(continuousSpawnInterval);
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize interaction distance and pickup distance in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * interactionDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * pickupDistance);
    }
}
