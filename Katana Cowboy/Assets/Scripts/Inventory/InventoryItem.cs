

public class InventoryItem
{
    // The name of the item.
    public string Name { private set; get; }
    // The amount of the item.
    public int Amount { set; get; }


    /// <summary>
    /// Constructs an Inventory Item with the given name and amount.
    /// </summary>
    /// <param name="name">Name of the item.</param>
    /// <param name="initialAmount">Initial amount of the item.</param>
    public InventoryItem(string name, int initialAmount)
    {
        Name = name;
        Amount = initialAmount;
    }

    // Names of all the inventory items.
    // Static references to the names of the available items.
    public static string BULLET = "Bullet";
}
