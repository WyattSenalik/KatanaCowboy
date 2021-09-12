using UnityEngine;

public class Picker : MonoBehaviour
{
    // Constants
    /// <summary>The maximum amount of pickups that can be picked up each frame.</summary>
    private const int MAX_COLS = 16;

    // Specifications.
    /// <summary>Range from the picker things can be picked up.</summary>
    [SerializeField] private Capsule m_pickUpCapsule = new Capsule();

    /// <summary>Layermask to check for pickups on.</summary>
    private int m_pickLayerMask;
    /// <summary>Colliders that are hit for pickups.</summary>
    private Collider[] m_hitcolliders;


    // Start is called before the first frame update
    private void Start()
    {
        // Get the layer mask.
        m_pickLayerMask = LayerMask.GetMask(Pickup.PICKUP_LAYER_NAME);
        // Create the hit colliders list.
        m_hitcolliders = new Collider[MAX_COLS];
    }
    // Update is called once per frame
    private void Update()
    {
        // Check for pickups.
        CheckForPickups();
    }
    // Draws gizmos in editor.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        m_pickUpCapsule.DrawWireCapsuleGizmo(transform.position);
    }


    /// <summary>
    /// Checks if there are any pickups close to this object.
    /// </summary>
    private void CheckForPickups()
    {
        // Look for pickups close to the picker.
        int numCols = Physics.OverlapCapsuleNonAlloc(m_pickUpCapsule.GetPoint0(transform.position),
            m_pickUpCapsule.GetPoint1(transform.position), m_pickUpCapsule.radius,
            m_hitcolliders, m_pickLayerMask, QueryTriggerInteraction.Collide);
        for (int i = 0; i < numCols; ++i)
        {
            // Try to pull a pick up off the collider.
            Pickup pick = m_hitcolliders[i].GetComponent<Pickup>();
            if (pick != null)
            {
                // Call pick for that object.
                pick.Pick(this);
            }
            else
            {
                Debug.LogError("Found " + m_hitcolliders[i].name + " on layer " +
                    LayerMask.LayerToName(m_pickLayerMask) + " but there was no Pickup attached to it.");
            }
        }
    }
}
