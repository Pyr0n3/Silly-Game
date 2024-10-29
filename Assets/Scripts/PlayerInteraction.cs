using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    private CubeSpawner currentSpawner;

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

                // Check for player input to interact
                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentSpawner.SpawnRandomCube();
                }
            }
        }
        else
        {
            currentSpawner = null;
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize interaction distance in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * interactionDistance);
    }
}
