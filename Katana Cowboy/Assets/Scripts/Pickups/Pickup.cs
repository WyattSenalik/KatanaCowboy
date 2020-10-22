using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    // Name of the layer that can be shot.
    public static readonly string PICKUP_LAYER_NAME = "Pickup";

    // Called before the first frame.
    private void Start()
    {
        // If we are changing the layer from some other specified layer, let me know.
        if (this.gameObject.layer != LayerMask.NameToLayer("Default") &&
            this.gameObject.layer != LayerMask.NameToLayer(PICKUP_LAYER_NAME))
        {
            Debug.Log("Changed " + this.gameObject.name + "'s layer from " +
               LayerMask.LayerToName(this.gameObject.layer) + " to " + PICKUP_LAYER_NAME);
        }
        // Set the layer for this gameobject.
        this.gameObject.layer = LayerMask.NameToLayer(PICKUP_LAYER_NAME);
    }

    /// <summary>
    /// Called when the pickup is picked up.
    /// <param name="_picker_">Picker script that picked this pickup up.</param>
    /// </summary>
    public abstract void Pick(Picker _picker_);
}
