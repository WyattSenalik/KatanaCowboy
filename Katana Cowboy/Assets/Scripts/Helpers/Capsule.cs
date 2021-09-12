using System;
using UnityEngine;

[Serializable]
public struct Capsule
{
    [SerializeField] private Vector3 m_center;
    [SerializeField] [Min(0.0f)] private float m_radius;
    [SerializeField] [Min(0.0f)] private float m_height;

    public Vector3 localCenter => m_center;
    public float radius => m_radius;
    public float height => m_height;


    public Capsule(Vector3 center, float radius = 0.5f, float height = 1f)
    {
        m_center = center;
        m_radius = radius;
        m_height = height;
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
