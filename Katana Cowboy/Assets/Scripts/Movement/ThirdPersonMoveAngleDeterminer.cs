using UnityEngine;

/// <summary>
/// Extends CharacterMoveAngleDeterminer to determine the angle
/// to move in to be determined by the specified camera's transform.
/// </summary>
public class ThirdPersonMoveAngleDeterminer : CharacterMoveAngleDeterminer
{
    // Smoothing the turning.
    [SerializeField] private float turnSmoothTime = 0.1f;
    // Reference to the camera that will determine our forward direction.
    [SerializeField] private Transform camTrans = null;

    // Holds the turn velocity to smooth the turn.
    // Should never be changed by this script.
    private float turnSmoothVelocity = 0.0f;


    /// <summary>
    /// Determines which direction the character should move in
    /// when the camera is the standard 3rd person camera.
    /// </summary>
    /// <param name="rawMoveDir">Raw movement input axis.</param>
    /// <returns>Angle (Degrees) to move the character at.</returns>
    public override float DetermineTargetMovementAngle(Vector3 rawMoveDir)
    {
        // Set the target angle to move at to be based on input and the camera's angle.
        float targetAngle = Mathf.Atan2(rawMoveDir.x, rawMoveDir.z) * Mathf.Rad2Deg + camTrans.eulerAngles.y;
        // Slowly turn the character a bit.
        // Get the angle we rotate the character so we don't simply snap and set the rotation of the character.
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        // Change the rotation of the character.
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        // Return the previously calculated target angle.
        return targetAngle;
    }
}
