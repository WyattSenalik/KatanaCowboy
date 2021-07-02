using UnityEngine;

/// <summary>
/// Controls rotating the character.
/// </summary>
[RequireComponent(typeof(CharacterMovement))]
public class CharacterRotator : MonoBehaviour
{
    // Smoothing the turning.
    [SerializeField] private RotateSmoother rotateSmoother = null;
    // Reference to the character movement to get the movement direction.
    private CharacterMovement characterMovementRef = null;


    // Called 0th
    // Domestic Initialization
    private void Awake()
    {
        characterMovementRef = GetComponent<CharacterMovement>();
    }
    // Repeatedly called on a set interval
    // Physics Calculations
    private void FixedUpdate()
    {
        // Figure out which way we should face
        Vector3 heading = DetermineHeading();

        // Apply rotation to the character so they are facing towards where they are walking and
        // apply a movement velocity towards the direction the camera is facing
        RotateCharacter(heading);
    }


    /// <summary>
    /// Smoothly rotates the character to be facing the given heading.
    /// </summary>
    /// <param name="heading">Direction for the character to face (assumed to be normalized).</param>
    public void RotateCharacter(Vector3 heading)
    {
        // Don't rotate the character if we have no heading
        if (heading == Vector3.zero)
        {
            return;
        }

        // Change the rotation of the character.
        transform.rotation = rotateSmoother.StepRotationFromHeading(heading, transform.rotation);
    }
    /// <summary>
    /// Rotates the character to have their body facing the given target.
    /// </summary>
    /// <param name="target">Target to face towards.</param>
    public void LookAtSmooth(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        RotateCharacter(direction);
    }
    /// <summary>
    /// Toggles if the character should be actively rotating themself in the direction
    /// they are moving. Default is that they are (true).
    /// </summary>
    /// <param name="value">True - enables the character to rotate in the direction they are moving.
    /// False - stops the character from rotating in the direction they are moving.</param>
    public void ToggleRotationFollowsMovement(bool value)
    {
        enabled = value;
    }


    /// <summary>
    /// Determines what the forward of the transform should be after the rotation.
    /// </summary>
    /// <returns>Forward/heading to rotate to match.</returns>
    private Vector3 DetermineHeading()
    {
        return characterMovementRef.GetMovementDirection();
    }
}
