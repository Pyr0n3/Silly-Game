using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Image[] inventorySlots; // Array of inventory slot UI images
    public Sprite redCubeInventory, greenCubeInventory, blueCubeInventory;
    public Sprite goldCubeInventory, purpleCubeInventory, cyanCubeInventory, godCubeInventory;

    private string[] inventoryItems = new string[10]; // Tracks the type of block in each inventory slot
    private GameManager gameManager; // Reference to GameManager to save the inventory


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); //(Ignore the green) Find the GameManager in the scene
    }

    // Get the current inventory items
    public string[] GetInventory()
    {
        return inventoryItems;
    }

    // Get the item in a specific inventory slot
    public string GetItemInSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < inventoryItems.Length)
            return inventoryItems[slotIndex];
        return null; // Return null if the slot index is invalid or empty
    }

    // Sets the loaded inventory data and updates the UI
    public void SetInventory(string[] loadedItems)
    {
        inventoryItems = loadedItems;

        // Update the visuals of the inventory slots after loading
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (string.IsNullOrEmpty(inventoryItems[i]))
            {
                inventoryItems[i] = null;
                ClearSlotVisual(i); // Clear the slot visuals (make it empty)
            }
            else
            {
                UpdateSlotVisual(i, inventoryItems[i]); // Update the UI slot with the correct item
            }
        }
    }

    // Adds an item to the inventory and triggers save through GameManager
    // Adds an item to the inventory and triggers save through GameManager
    public bool AddToInventory(string cubeType)
    {
        for (int i = 0; i < inventoryItems.Length; i++) // Loop through all 10 slots
        {
            if (inventoryItems[i] == null) // Find the first empty slot
            {
                inventoryItems[i] = cubeType; // Add the item to that slot
                UpdateSlotVisual(i, cubeType); // Update the inventory slot visuals
                gameManager.SaveInventory(inventoryItems); // Save the inventory whenever it changes
                return true; // Successfully added
            }
        }
        Debug.Log("Inventory is full!"); // If no slots are empty, inventory is full
        return false; // Inventory full
    }


    // Removes an item from the inventory and triggers save through GameManager
    public void RemoveFromInventory(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventoryItems.Length || inventoryItems[slotIndex] == null)
        {
            Debug.Log("Invalid slot or empty slot.");
            return;
        }

        inventoryItems[slotIndex] = null; // Clear the inventory slot
        ClearSlotVisual(slotIndex); // Clear the slot visuals
        gameManager.SaveInventory(inventoryItems); // Save the inventory whenever it changes
    }

    // Updates the UI to reflect the correct item in a slot
    private void UpdateSlotVisual(int slotIndex, string cubeType)
    {
        Sprite sprite = GetInventorySprite(cubeType);
        if (sprite != null)
        {
            inventorySlots[slotIndex].sprite = sprite;
            inventorySlots[slotIndex].enabled = true;
        }
    }

    // Clears the visuals of a given slot
    private void ClearSlotVisual(int slotIndex)
    {
        inventorySlots[slotIndex].sprite = null;
        inventorySlots[slotIndex].enabled = false;
    }

    // Returns the correct sprite based on the cube type
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
            _ => null,
        };
    }
}
