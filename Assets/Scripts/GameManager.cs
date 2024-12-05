using UnityEngine;

public class GameManager : MonoBehaviour
{
    public InventoryManager inventoryManager;

    private const string InventoryKey = "Inventory";

    private void Start()
    {
        // Don't automatically load the inventory at the start
    }

    // This is called manually when the player presses the "Load" button
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey(InventoryKey))
        {
            string[] loadedInventory = LoadInventoryData();
            inventoryManager.SetInventory(loadedInventory); // Set the loaded inventory and update UI
            Debug.Log("Game Loaded: Inventory loaded from save.");
        }
        else
        {
            Debug.Log("No saved inventory found.");
        }
    }

    // This is called when the player manually saves the game
    public void SaveGame()
    {
        SaveInventory(inventoryManager.GetInventory()); // Save inventory data
        Debug.Log("Game Saved: Inventory saved to PlayerPrefs.");
    }

    // Save the inventory to PlayerPrefs
    public void SaveInventory(string[] inventoryItems) // <-- Make this public
    {
        SaveInventoryData(inventoryItems);
    }

    // Save the inventory data to PlayerPrefs
    private void SaveInventoryData(string[] inventoryItems)
    {
        string inventoryString = string.Join(",", inventoryItems);
        PlayerPrefs.SetString(InventoryKey, inventoryString);
        PlayerPrefs.Save();
    }

    // Load the inventory from PlayerPrefs
    private string[] LoadInventoryData()
    {
        string inventoryString = PlayerPrefs.GetString(InventoryKey);
        return inventoryString.Split(',');
    }
}
