using UnityEngine;

public struct ShotInfo
{
    // Point of impact.
    public Vector3 point;
    // Direction the shot came from.
    public Vector3 fromDir;

    /// <summary>
    /// Constructs shot info.
    /// </summary>
    /// <param name="_point_">Point of impact.</param>
    /// <param name="_fromDirection_">Direction the shot came from. Should be normalized.</param>
    public ShotInfo(Vector3 _point_, Vector3 _fromDirection_)
    {
        point = _point_;
        fromDir = _fromDirection_;
    }
}
