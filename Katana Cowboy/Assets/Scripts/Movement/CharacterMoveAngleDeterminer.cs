using UnityEngine;

/// <summary>
/// Base class that is used by CharacterMovement to determine
/// which angle the character should move at.
/// </summary>
public abstract class CharacterMoveAngleDeterminer : MonoBehaviour
{
    /// <summary>
    /// Determines at what angle the character should move in.
    /// </summary>
    /// <param name="rawMoveDir">Raw movement input axis.</param>
    /// <returns>Angle (Degrees) to move the character at.</returns>
    public abstract float DetermineTargetMovementAngle(Vector3 rawMoveDir);
}
