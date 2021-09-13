using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Hit
{
    // Point of impact.
    public Collision collision { get; set; }


    public Hit(Collision collision)
    {
        this.collision = collision;
    }
}
