using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Image[] inventorySlots; // Array of inventory slot UI images
    public Sprite redCubeInventory, greenCubeInventory, blueCubeInventory;
    public Sprite goldCubeInventory, purpleCubeInventory, cyanCubeInventory, godCubeInventory, blackCubeInventory;

    private string[] inventoryItems = new string[10]; // Tracks the type of block in each inventory slot
    private GameManager gameManager; // Reference to GameManager to save the inventory

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager in the scene
        Debug.Log("InventoryManager initialized.");
    }

    public string[] GetInventory()
    {
        return inventoryItems;
    }

    public string GetItemInSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < inventoryItems.Length)
        {
            Debug.Log($"Retrieved item from slot {slotIndex}: {inventoryItems[slotIndex]}");
            return inventoryItems[slotIndex];
        }
        Debug.Log($"Invalid slot index: {slotIndex}");
        return null; // Return null if the slot index is invalid or empty
    }

    public void SetInventory(string[] loadedItems)
    {
        inventoryItems = loadedItems;

        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (string.IsNullOrEmpty(inventoryItems[i]))
            {
                inventoryItems[i] = null;
                ClearSlotVisual(i);
                Debug.Log($"Slot {i} cleared during inventory load.");
            }
            else
            {
                UpdateSlotVisual(i, inventoryItems[i]);
                Debug.Log($"Slot {i} set to {inventoryItems[i]} during inventory load.");
            }
        }
    }

    public bool AddToInventory(string cubeType)
    {
        for (int i = 0; i < inventoryItems.Length; i++) // Loop through all 10 slots
        {
            if (inventoryItems[i] == null) // Find the first empty slot
            {
                inventoryItems[i] = cubeType;
                UpdateSlotVisual(i, cubeType);
                gameManager.SaveInventory(inventoryItems);
                Debug.Log($"Added {cubeType} to slot {i}");
                return true; // Successfully added
            }
        }
        Debug.Log("Inventory is full!");
        return false; // Inventory full
    }

    public void RemoveFromInventory(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventoryItems.Length || inventoryItems[slotIndex] == null)
        {
            Debug.Log($"Cannot remove item: Invalid or empty slot at index {slotIndex}");
            return;
        }

        Debug.Log($"Removed {inventoryItems[slotIndex]} from slot {slotIndex}");
        inventoryItems[slotIndex] = null; // Clear the inventory slot
        ClearSlotVisual(slotIndex);
        gameManager.SaveInventory(inventoryItems);
    }

    private void UpdateSlotVisual(int slotIndex, string cubeType)
    {
        Sprite sprite = GetInventorySprite(cubeType);
        if (sprite != null)
        {
            inventorySlots[slotIndex].sprite = sprite;
            inventorySlots[slotIndex].enabled = true;
            Debug.Log($"Slot {slotIndex} updated with sprite for {cubeType}");
        }
        else
        {
            Debug.LogError($"Sprite for {cubeType} not found for slot {slotIndex}");
        }
    }

    private void ClearSlotVisual(int slotIndex)
    {
        inventorySlots[slotIndex].sprite = null;
        inventorySlots[slotIndex].enabled = false;
        Debug.Log($"Cleared visuals for slot {slotIndex}");
    }

    private Sprite GetInventorySprite(string cubeType)
    {
        return cubeType switch
        {
            "red" => redCubeInventory,
            "green" => greenCubeInventory,
            "blue" => blueCubeInventory,
            "gold" => goldCubeInventory,
            "purple" => purpleCubeInventory,
            "cyan" => cyanCubeInventory,
            "god" => godCubeInventory,
            "black" => blackCubeInventory,
            _ => null,
        };
    }
}
