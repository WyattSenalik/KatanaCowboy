using UnityEngine;

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

    // If shoot is being held down.
    private bool isShootHeld;
    // Layermask int to shoot at.
    private int shootLayerMask;


    // Start is called before the first frame update
    private void Start()
    {
        isShootHeld = false;
        shootLayerMask = LayerMask.GetMask(Shootable.SHOOT_LAYER_NAME);
    }
    // Update is called once per frame
    private void Update()
    {
        // Get shoot input.
        bool shouldShoot = Input.GetAxisRaw("Shoot") != 0f;
        if (shouldShoot)
        {
            // If shoot isn't being held down and instead was pressed again, then actually fire again.
            if (!isShootHeld)
            {
                Shoot();
                isShootHeld = true;
            }
        }
        // Reset shoot held when it is released
        else
        {
            isShootHeld = false;
        }
    }


    /// <summary>
    /// Shoots a bullet.
    /// </summary>
    private void Shoot()
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
