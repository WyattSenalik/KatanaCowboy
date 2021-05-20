using UnityEngine;

/// <summary>
/// Extends the CharacterMoveAngleDeterminer to
/// have the character's movement direction be determined by
/// which way which way the character is facing.
/// </summary>
public class OverShoulderMoveAngleDeterminer : CharacterMoveAngleDeterminer
{
    /// <summary>
    /// Helper function to determine which direction the character should move in
    /// when the camera is the standard 3rd person camera.
    /// </summary>
    /// <param name="rawMoveDir">Raw movement input axis.</param>
    /// <returns>Angle (Degrees) to move the character at.</returns>
    public override float DetermineTargetMovementAngle(Vector3 rawMoveDir)
    {
        // Set the target angle to move at to be based on only the input.
        return Mathf.Atan2(rawMoveDir.x, rawMoveDir.z) * Mathf.Rad2Deg + transform.eulerAngles.y;
    }
}
