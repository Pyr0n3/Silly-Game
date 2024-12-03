using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Image[] inventorySlots; // Array of inventory slot images
    public Sprite redCubeInventory, greenCubeInventory, blueCubeInventory, goldCubeInventory, purpleCubeInventory, cyanCubeInventory, godCubeInventory;
    public GameObject inventoryUI;

    private string[] inventoryItems = new string[3]; // Stores the type of cube in each slot

    void Start()
    {
        inventoryUI.SetActive(false); // Hide inventory at start
    }

    void Update()
    {
        // Toggle inventory UI with Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    public bool AddToInventory(string cubeType)
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i] == null)
            {
                inventoryItems[i] = cubeType;
                inventorySlots[i].sprite = GetInventorySprite(cubeType);
                inventorySlots[i].enabled = true;
                return true;
            }
        }
        return false; // Inventory full
    }

    public void RemoveFromInventory(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventoryItems.Length || inventoryItems[slotIndex] == null)
            return;

        inventoryItems[slotIndex] = null;
        inventorySlots[slotIndex].sprite = null;
        inventorySlots[slotIndex].enabled = false;
    }

    public string GetItemInSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < inventoryItems.Length)
            return inventoryItems[slotIndex];
        return null;
    }

    private Sprite GetInventorySprite(string cubeType)
    {
        return cubeType switch
        {
            "Red" => redCubeInventory,
            "Green" => greenCubeInventory,
            "Blue" => blueCubeInventory,
            "Gold" => goldCubeInventory,
            "Purple" => purpleCubeInventory,
            "Cyan" => cyanCubeInventory,
            "God" => godCubeInventory,
            _ => null,
        };
    }
}
