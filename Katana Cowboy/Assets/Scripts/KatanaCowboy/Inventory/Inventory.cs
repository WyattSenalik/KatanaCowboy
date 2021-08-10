using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages an inventory for the player.
/// </summary>
public class Inventory : MonoBehaviour
{
    // List of the inventory.
    // Kept sorted by name A to Z.
    private List<InventoryItem> items = new List<InventoryItem>();


    // Start is called before the first frame update
    private void Start()
    {
        items = new List<InventoryItem>();
    }


    /// <summary>
    /// Adds x amount of an item to the inventory.
    /// </summary>
    /// <param name="itemName">Name of the item being added.</param>
    /// <param name="gainCount">Amount of items to be added.</param>
    public void GainItem(string itemName, int gainCount)
    {
        Debug.Log("Gain " + gainCount + " " + itemName);
        // Check if the item is in the list.
        int itemIndex = FindItem(itemName);
        // If it is in the list, add the add amount to it.
        if (itemIndex != -1)
        {
            items[itemIndex].Amount += gainCount;
        }
        // If the item is not in the list, insert it.
        else
        {
            InsertItem(itemName, gainCount);
        }
    }
    /// <summary>
    /// Removes x of an item from the inventory.
    /// </summary>
    /// <param name="itemName">Name of the item being lost.</param>
    /// <param name="lossCount">Amount of items to be lost.</param>
    public void LoseItem(string itemName, int lossCount)
    {
        Debug.Log("Lose " + lossCount + " " + itemName);
        // Check if the item is in the list.
        int itemIndex = FindItem(itemName);
        // If it is in the list, subtract the lose amount to it.
        if (itemIndex != -1)
        {
            items[itemIndex].Amount -= lossCount;
            // If it reduced the amount of the item, to zero, remove it from the list.
            if (items[itemIndex].Amount == 0)
            {
                items.RemoveAt(itemIndex);
            }
            // If the reduced amount is less than zero, throw an error.
            else if (items[itemIndex].Amount < 0)
            {
                Debug.LogError("Tried to remove " + lossCount + " " + itemName + " from inventory, but there was only " 
                    + items[itemIndex].Amount + lossCount + " " + itemName + " in the inventory.");
                items.RemoveAt(itemIndex);
            }
        }
        // If the item is not in the list, throw an error.
        else
        {
            Debug.LogError("Tried to remove " + lossCount + " " + itemName + " from inventory, but there is no " + itemName + " in the inventory");
        }
    }
    /// <summary>
    /// Gets the amount of the item in the inventory.
    /// </summary>
    /// <param name="itemName">Name of the item in the inventory.</param>
    /// <returns>int - amount of the item in the inventory.</returns>
    public int GetAmount(string itemName)
    {
        // Try to find the item.
        int itemIndex = FindItem(itemName);
        // If we found the item, return its amount.
        if (itemIndex != -1)
        {
            return items[itemIndex].Amount;
        }
        // If we did not find the item, return  0.
        else
        {
            return 0;
        }
    }


    /// <summary>
    /// Finds an item with the given name to the inventory.
    /// Returns the index of the item in the list or -1 if the item is not in the list.
    /// </summary>
    /// <param name="itemName">Name of the item to find.</param>
    /// <returns>Index of the item.</returns>
    private int FindItem(string itemName)
    {
        int low = 0;
        int high = items.Count - 1;
        // While low has not passed high.
        while (low <= high)
        {
            // Get the new middle.
            int mid = (low + high) / 2;
            // If name is left of the middle,
            // test the left half of the current list.
            if (items[mid].Name.GreaterThan(itemName))
            {
                high = mid - 1;
            }
            // The name is right of the middle,
            // test the right half of the current list.
            else if (items[mid].Name.LessThan(itemName))
            {
                low = mid + 1;
            }
            // If name is neither greater than nor less than, it is equal to.
            else
            {
                return mid;
            }
        }
        // If we do not find it, return -1.
        return -1;
    }

    /// <summary>
    /// Inserts a new item into the list.
    /// Asummes the item is not already in the list.
    /// </summary>
    /// <param name="itemName">Name of the item to insert.</param>
    /// <param name="amount">Initial amount of the item to insert.</param>
    private void InsertItem(string itemName, int amount)
    {
        // Inser the item at its insert index.
        int insertIndex = FindItemInsertIndex(itemName);
        items.Insert(insertIndex, new InventoryItem(itemName, amount));
    }

    /// <summary>
    /// Finds where the object with the given name should be inserted in the list.
    /// Assumes the item is not already in the list.
    /// </summary>
    /// <param name="itemName">Name of the item to find the insert index for.</param>
    /// <returns>int - index to insert the item at.</returns>
    private int FindItemInsertIndex(string itemName)
    {
        int low = 0;
        int high = items.Count - 1;
        int mid = 0;
        // While low has not passed high.
        while (low <= high)
        {
            // Get the new middle.
            mid = (low + high) / 2;
            // If name is left of the middle,
            // test the left half of the current list.
            if (items[mid].Name.GreaterThan(itemName))
            {
                high = mid - 1;
            }
            // The name is right of the middle,
            // test the right half of the current list.
            else if (items[mid].Name.LessThan(itemName))
            {
                low = mid + 1;
            }
            // If name is neither greater than nor less than, it is equal to.
            else
            {
                return mid;
            }
        }
        // If we do not find it, return -1.
        return mid;
    }
}
