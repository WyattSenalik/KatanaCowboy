using System;
using UnityEngine;

[Serializable]
public struct Capsule
{
    [SerializeField] private Vector3 _center;
    [SerializeField] [Min(0.0f)] private float _radius;
    [SerializeField] [Min(0.0f)] private float _height;

    public Vector3 localCenter => _center;
    public float radius => _radius;
    public float height => _height;


    public Capsule(Vector3 center, float radius = 0.5f, float height = 1f)
    {
        _center = center;
        _radius = radius;
        _height = height;
    }


    public Vector3 GetPoint0(Vector3 globalPosition)
    {
        return globalPosition + localCenter - new Vector3(0, height / 2, 0);
    }
    public Vector3 GetPoint1(Vector3 globalPosition)
    {
        return globalPosition + localCenter + new Vector3(0, height / 2, 0);
    }
    public void DrawWireCapsuleGizmo(Vector3 globalPosition)
    {
        Vector3 point0 = GetPoint0(globalPosition);
        Vector3 point1 = GetPoint1(globalPosition);
        Gizmos.DrawWireSphere(point0, radius);
        Gizmos.DrawWireSphere(point1, radius);
        Vector3 xOffset = new Vector3(radius, 0, 0);
        Vector3 zOffset = new Vector3(0, 0, radius);
        Gizmos.DrawLine(point0 + xOffset, point1 + xOffset);
        Gizmos.DrawLine(point0 - xOffset, point1 - xOffset);
        Gizmos.DrawLine(point0 + zOffset, point1 + zOffset);
        Gizmos.DrawLine(point0 - zOffset, point1 - zOffset);
    }
}
