using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IHittable hittable))
        {
            hittable.Hit(new Hit(gameObject));
        }
    }
    */
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out IHittable hittable))
        {
            hittable.Hit(new Hit(collision));
        }
    }
}
