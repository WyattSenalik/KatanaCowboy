using UnityEngine;

/// <summary>
/// Extends the CharacterMoveAngleDeterminer to
/// have the character's movement direction be determined by
/// which way which way the character is facing.
/// </summary>
public class OverShoulderMoveAngleDeterminer : CharacterMoveAngleDeterminer
{
    // Smoothing the turning.
    [SerializeField] private float turnSmoothTime = 0.1f;

    // Holds the turn velocity to smooth the turn.
    // Should never be changed by this script.
    private float turnSmoothVelocity = 0.0f;


    /// <summary>
    /// Helper function to determine which direction the character should move in
    /// when the camera is the standard 3rd person camera.
    /// </summary>
    /// <param name="rawMoveDir">Raw movement input axis.</param>
    /// <returns>Angle (Degrees) to move the character at.</returns>
    public override float DetermineTargetMovementAngle(Vector3 rawMoveDir)
    {
        // Set the target angle to move at to be based on only the input.
        float targetAngle = Mathf.Atan2(rawMoveDir.x, rawMoveDir.z) * Mathf.Rad2Deg + transform.eulerAngles.y;
        // Slowly turn the character a bit.
        // Get the angle we rotate the character so we don't simply snap and set the rotation of the character.
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        
        return angle;
    }
}
