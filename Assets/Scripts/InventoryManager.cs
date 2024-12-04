using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Image[] inventorySlots; // Array of inventory slot UI images
    public Sprite redCubeInventory, greenCubeInventory, blueCubeInventory;
    public Sprite goldCubeInventory, purpleCubeInventory, cyanCubeInventory, godCubeInventory;

    //public Image cube1;

    private string[] inventoryItems = new string[3]; // Tracks the type of block in each inventory slot

    public bool AddToInventory(string cubeType)
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i] == null)
            {
                inventoryItems[i] = cubeType;
                UpdateSlotVisual(i, cubeType); // Update the inventory slot visuals
                return true;
            }
        }
        Debug.Log("Inventory is full!");
        return false; // Inventory full
    }

    public void RemoveFromInventory(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventoryItems.Length || inventoryItems[slotIndex] == null)
        {
            Debug.Log($"Cannot delete: Slot {slotIndex} is invalid or empty.");
            return;
        }

        Debug.Log($"Removing cube from slot {slotIndex}.");
        inventoryItems[slotIndex] = null; // Clear the inventory slot
        ClearSlotVisual(slotIndex); // Clear the slot visuals
    }


    public string GetItemInSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < inventoryItems.Length)
            return inventoryItems[slotIndex];
        return null;
    }

    private void UpdateSlotVisual(int slotIndex, string cubeType)
    {
        Sprite sprite = GetInventorySprite(cubeType);
        if (sprite != null)
        {
            inventorySlots[slotIndex].sprite = sprite;
            inventorySlots[slotIndex].enabled = true;
        }
    }

    private void ClearSlotVisual(int slotIndex)
    {
        inventorySlots[slotIndex].sprite = null;
        inventorySlots[slotIndex].enabled = false;
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
            _ => null,
        };
    }

    private void Start()
    {
        //cube1.sprite = redCubeInventory;
    }
}
