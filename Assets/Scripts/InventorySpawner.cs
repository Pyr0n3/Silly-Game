using UnityEngine;

public class InventorySpawner : MonoBehaviour
{
    public InventoryManager inventoryManager;

    public GameObject redCubePrefab, greenCubePrefab, blueCubePrefab;
    public GameObject goldCubePrefab, purpleCubePrefab, cyanCubePrefab, godCubePrefab;

    public void SpawnCubeFromInventory(int slotIndex)
    {
        string cubeType = inventoryManager.GetItemInSlot(slotIndex);
        if (cubeType == null) return;

        GameObject cubePrefab = cubeType switch
        {
            "Red" => redCubePrefab,
            "Green" => greenCubePrefab,
            "Blue" => blueCubePrefab,
            "Gold" => goldCubePrefab,
            "Purple" => purpleCubePrefab,
            "Cyan" => cyanCubePrefab,
            "God" => godCubePrefab,
            _ => null,
        };

        Vector3 spawnOffset = cubeType switch
        {
            "God" => new Vector3(0, 108f, 200f),
            "Cyan" => new Vector3(0, 18f, 100f),
            "Purple" => new Vector3(0, 8f, 50f),
            "Gold" => new Vector3(0, 2f, 25f),
            _ => new Vector3(0, 1.5f, 5f),
        };

        if (cubePrefab != null)
        {
            Instantiate(cubePrefab, transform.position + transform.forward * spawnOffset.z + Vector3.up * spawnOffset.y, Quaternion.identity);
            inventoryManager.RemoveFromInventory(slotIndex);
        }
    }

    public void DeleteCubeFromInventory(int slotIndex)
    {
        inventoryManager.RemoveFromInventory(slotIndex);
    }
}
