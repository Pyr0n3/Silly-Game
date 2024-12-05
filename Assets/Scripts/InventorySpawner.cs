using UnityEngine;

public class InventorySpawner : MonoBehaviour
{
    public InventoryManager inventoryManager;

    public GameObject redCubePrefab, greenCubePrefab, blueCubePrefab;
    public GameObject goldCubePrefab, purpleCubePrefab, cyanCubePrefab, godCubePrefab;

    public void SpawnCubeFromInventory(int slotIndex)
    {
        string cubeType = inventoryManager.GetItemInSlot(slotIndex);
        if (cubeType == null)
        {
            Debug.Log($"No cube in slot {slotIndex} to spawn!");
            return;
        }

        GameObject cubePrefab = cubeType switch
        {
            "red" => redCubePrefab,
            "green" => greenCubePrefab,
            "blue" => blueCubePrefab,
            "gold" => goldCubePrefab,
            "purple" => purpleCubePrefab,
            "cyan" => cyanCubePrefab,
            "god" => godCubePrefab,
            _ => null,
        };

        if (cubePrefab == null)
        {
            Debug.LogError($"Cube prefab for type {cubeType} not found!");
            return;
        }

        Vector3 spawnOffset = cubeType switch
        {
            "god" => new Vector3(0, 108f, 200f),
            "cyan" => new Vector3(0, 18f, 100f),
            "purple" => new Vector3(0, 8f, 50f),
            "gold" => new Vector3(0, 2f, 25f),
            _ => new Vector3(0, 1.5f, 5f),
        };

        Instantiate(cubePrefab, transform.position + transform.forward * spawnOffset.z + Vector3.up * spawnOffset.y, Quaternion.identity);
        Debug.Log($"Spawned {cubeType} cube from slot {slotIndex}!");

        inventoryManager.RemoveFromInventory(slotIndex);
    }

    public void DeleteCubeFromInventory(int slotIndex)
    {
        inventoryManager.RemoveFromInventory(slotIndex);
    }
}
