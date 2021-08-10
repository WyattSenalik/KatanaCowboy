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
    // Smoothing the turning.
    [SerializeField] private RotateSmoother rotateSmoother = null;

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
    // Called once every frame
    private void Update()
    {
        RotateGunTowardsTarget();
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

            if (Physics.Raycast(camTrans.position, camTrans.forward, out RaycastHit hit, range, shootLayerMask))
            {
                Debug.Log("Shot " + hit.transform.name);
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
    /// <summary>
    /// Returns if a target was hit and the position of that target.
    /// </summary>
    /// <param name="targetPosition">Position of the current target that would be hit.</param>
    /// <returns>If a target was hit.</returns>
    public bool GetTargetPosition(out Vector3 targetPosition)
    {
        if (Physics.Raycast(camTrans.position, camTrans.forward, out RaycastHit hit, range, shootLayerMask))
        {
            targetPosition = hit.point;
            return true;
        }
        targetPosition = Vector3.zero;
        return false;
    }
    /// <summary>
    /// Returns the direction the gun is being pointed in.
    /// </summary>
    /// <returns>Direction the gun is currently being aimed in.</returns>
    public Vector3 GetAimDirection()
    {
        return camTrans.forward;
    }


    /// <summary>
    /// Rotates teh gun towards the current target.
    /// </summary>
    private void RotateGunTowardsTarget()
    {
        // Rotate to face the aim target
        if (GetTargetPosition(out Vector3 aimTarget))
        {
            transform.rotation = rotateSmoother.StepRotationFromLookAt(aimTarget, transform.position, transform.rotation);
        }
        // If we have no target, rotate in the camera's direction
        else
        {
            Vector3 aimDirection = GetAimDirection();
            transform.rotation = rotateSmoother.StepRotationFromHeading(aimDirection, transform.rotation);
        }
    }
}
