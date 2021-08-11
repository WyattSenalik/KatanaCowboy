using UnityEngine;

public class BulletPickup : Pickup
{
    // The amount of bullets to give the gun on pickup.
    [SerializeField] private int bulletsToGive = 1;


    /// <summary>
    /// Override of Pick.
    /// Gives the gun more bullets.
    /// </summary>
    /// <param name="_picker_"></param>
    public override void Pick(Picker _picker_)
    {
        // Look for an inventory on the picker.
        Inventory invent = _picker_.GetComponent<Inventory>();
        if (invent != null)
        {
            // Give the gun bullets.
            invent.GainItem(InventoryItem.BULLET, bulletsToGive);
        }
        else
        {
            Debug.Log("Could not find a Inventory attached to " + _picker_.name);
        }

        // Destroy the pickup.
        Destroy(this.gameObject);
    }
}
