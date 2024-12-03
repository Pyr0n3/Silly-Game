using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 100f;
    public InventoryManager inventoryManager;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click to pick up
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, interactionDistance))
            {
                if (hit.collider.CompareTag("Cube"))
                {
                    string cubeType = hit.collider.gameObject.name.Replace("CubePrefab", "").Trim();
                    bool added = inventoryManager.AddToInventory(cubeType);

                    if (added)
                    {
                        Destroy(hit.collider.gameObject); // Remove the cube
                    }
                    else
                    {
                        Debug.Log("Inventory is full!");
                    }
                }
            }
        }
    }
}
