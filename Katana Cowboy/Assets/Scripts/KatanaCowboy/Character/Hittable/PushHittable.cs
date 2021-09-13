using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PushHittable : MonoBehaviour, IHittable
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
        rb = GetComponent<Rigidbody>();
    }

    public void Hit(Hit hit)
    {
        Vector3 hitPoint = hit.collision.contacts[0].point;
        Vector3 fromDir = transform.position - hitPoint;
        Vector3 force = fromDir * forceMag;
        rb.AddForceAtPosition(force, hit.collision.contacts[0].point);
    }
}
