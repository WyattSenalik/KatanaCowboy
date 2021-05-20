using UnityEngine;

/// <summary>
/// Handles shooting with the gun.
/// </summary>
public class GunController : MonoBehaviour
{
    // Configurables.
    // Range the gun can shoot.
    [SerializeField] private float range = 100f;
    // The damage a gunshot does.
    [SerializeField] private float damage = 1f;

    // References.
    // Reference to the camera's transform.
    [SerializeField] private Transform camTrans = null;
    // Reference to the player's inventory.
    [SerializeField] private Inventory inventory = null;
    // Reference to the visuals for the gun
    [SerializeField] private GameObject gunVisuals = null;

    // Layermask int to shoot at.
    private int shootLayerMask = 1 << 0;


    // Start is called before the first frame update
    private void Start()
    {
        shootLayerMask = LayerMask.GetMask(Shootable.SHOOT_LAYER_NAME);
    }


    /// <summary>
    /// Toggles if the gun is active or inactive.
    /// </summary>
    /// <param name="shouldActive">Active if true. Inactive if false.</param>
    public void ToggleActive(bool shouldActive)
    {
        gunVisuals.SetActive(shouldActive);
    }
    /// <summary>
    /// Shoots a bullet.
    /// </summary>
    public void Shoot()
    {
        // Get the amount of bullets we have.
        int amountBullets = inventory.GetAmount(InventoryItem.BULLET);
        if (amountBullets > 0)
        {
            inventory.LoseItem(InventoryItem.BULLET, 1);
            Debug.Log("Shoot");

            RaycastHit hit;
            if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, range, shootLayerMask))
            {
                Debug.Log(hit.transform.name);
                // Pull Shootable off object.
                Shootable shotThing = hit.transform.GetComponent<Shootable>();
                if (shotThing != null)
                {
                    // Shoot the thing.
                    // Get the info of the shot.
                    Vector3 from = (hit.point - camTrans.position).normalized;
                    ShotInfo info = new ShotInfo(hit.point, from);
                    shotThing.GetShot(damage, info);
                }
            }
        }
    }
}
