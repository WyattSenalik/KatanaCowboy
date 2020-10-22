using UnityEngine;

public class Picker : MonoBehaviour
{
    // Specifications.
    // Range from the picker things can be picked up.
    [SerializeField]
    private float pickupRadius = 3f;
    // The maximum amount of pickups that can be picked up each frame.
    private const int MAX_COLS = 16;

    // Layermask to check for pickups on.
    private int pickLayerMask;
    // Colliders that are hit for pickups.
    private Collider[] hitcolliders;

    // Start is called before the first frame update
    private void Start()
    {
        // Get the layer mask.
        pickLayerMask = LayerMask.GetMask(Pickup.PICKUP_LAYER_NAME);
        // Create the hit colliders list.
        hitcolliders = new Collider[MAX_COLS];
    }

    // Update is called once per frame
    private void Update()
    {
        // Check for pickups.
        CheckForPickups();
    }

    /// <summary>
    /// Checks if there are any pickups close to this object.
    /// </summary>
    private void CheckForPickups()
    {
        // Look for pickups close to the picker.
        int numCols = Physics.OverlapSphereNonAlloc(this.transform.position, pickupRadius, hitcolliders, pickLayerMask, QueryTriggerInteraction.Collide);
        for (int i = 0; i < numCols; ++i)
        {
            // Try to pull a pick up off the collider.
            Pickup pick = hitcolliders[i].GetComponent<Pickup>();
            if (pick != null)
            {
                // Call pick for that object.
                pick.Pick(this);
            }
            else
                Debug.LogError("Found " + hitcolliders[i].name + " on layer " + LayerMask.LayerToName(pickLayerMask) + " but there was no Pickup attached to it.");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.gameObject.transform.position, pickupRadius);
    }
}
