using UnityEngine;

/// <summary>
/// When this gameobject is shot, a force will be applied.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PushShootable : Shootable
{
    // Customizable.
    // Magnitude of force to apply.
    [SerializeField] private float forceMag = 30f;

    // References.
    // Reference to the attached rigidbody.
    private Rigidbody rb;

    // Called 0th.
    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Override form Shootable.
    /// Applies a force to the object and resets health.
    /// </summary>
    protected override void Die()
    {
        Vector3 force = shotInfo.fromDir * forceMag;
        rb.AddForceAtPosition(force, shotInfo.point);
        curHealth = maxHealth;
    }
}
