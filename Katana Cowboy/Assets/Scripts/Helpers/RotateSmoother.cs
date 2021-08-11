using System;
using UnityEngine;

/// <summary>
/// Handles controlling some standard rotation smoothing behaviour.
/// </summary>
[Serializable]
public class RotateSmoother
{
    // Smoothing the turning.
    [SerializeField] [Min(0.0f)] private float turnSmoothTime = 0.1f;

    // Holds the turn velocity to smooth the turn.
    // Should never be changed by this script.
    private float turnSmoothVelocity = 0.0f;


    // Constructors
    public RotateSmoother() { }
    public RotateSmoother(float smoothTime)
    {
        turnSmoothTime = smoothTime;
    }


    /// <summary>
    /// Gets the angle from the given direction.
    /// </summary>
    /// <param name="heading">Direction to determine the angle from (xz plane).</param>
    /// <returns>Angle on the xz plane created by the direction.</returns>
    public float GetAngleFromHeading(Vector3 heading)
    {
        // Find the target angle from the given heading.
        return Mathf.Atan2(heading.x, heading.z) * Mathf.Rad2Deg;
    }
    /// <summary>
    /// Gets the angle from the given heading and then smooths it over time.
    /// </summary>
    /// <param name="heading">Direction to determine the angle from (xz plane).</param>
    /// <param name="currentAngle">Current angle to smooth starting from.</param>
    /// <returns>Angle between the angle created by the headhing and the current angle.</returns>
    public float StepAngleFromHeading(Vector3 heading, float currentAngle)
    {
        // Find the target angle from the given heading.
        float targetAngle = GetAngleFromHeading(heading);
        // Slowly turn the character a bit.
        // Get the angle we rotate the character so we don't simply snap and set the rotation of the character.
        return Mathf.SmoothDampAngle(currentAngle, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
    }
    /// <summary>
    /// Gets a rotation that is a step between the given current rotation and the
    /// rotation where the heading is the forward.
    /// </summary>
    /// <param name="heading">Desired direction to face towards (xz plane).</param>
    /// <param name="currentRotation">Current rotation to smooth starting from.</param>
    /// <returns>Rotation a step between the current rotation and the rotation where
    /// the heading is the forward vector.</returns>
    public Quaternion StepRotationFromHeading(Vector3 heading, Quaternion currentRotation)
    {
        float angle = StepAngleFromHeading(heading, currentRotation.eulerAngles.y);
        return Quaternion.Euler(new Vector3(0.0f, angle, 0.0f));
    }
    /// <summary>
    /// Gets a rotation that is a step between the given current rotation and the
    /// rotation where the the look at target is being looked at.
    /// </summary>
    /// <param name="lookAtTarget">Position to look at.</param>
    /// <param name="currentPosition">Position of the looker.</param>
    /// <param name="currentRotation">Current rotation to smooth starting from.</param>
    /// <returns>Rotation a step between the current rotation and the rotation where
    /// the look at target is being looked at from the current position.</returns>
    public Quaternion StepRotationFromLookAt(Vector3 lookAtTarget, Vector3 currentPosition, Quaternion currentRotation)
    {
        Vector3 heading = (lookAtTarget - currentPosition).normalized;
        return StepRotationFromHeading(heading, currentRotation);
    }
}
