using UnityEngine;

/// <summary>
/// Base class for character movement controllers.
/// </summary>
public class CharacterMovement : MonoBehaviour
{
    // Speed the character will move at.
    [SerializeField] [Min(0.0f)] private float speed = 6f;
    // Speed the character will sprint at.
    [SerializeField] [Min(0.0f)] private float sprintSpeed = 12f;
    // Acceleration due to gravity.
    [SerializeField] [Min(0.0f)] private float gravity = 9.81f;
    // Radius of the sphere created for the ground check.
    [SerializeField] [Min(0.0f)] private float groundDistance = 0.4f;
    // Mask to check for ground.
    [SerializeField] private LayerMask groundMask = 1 << 0;
    // Height to reach on a jump.
    [SerializeField] [Min(0.0f)] private float jumpHeight = 3f;

    // References
    // Reference to the ground check.
    [SerializeField] private Transform groundCheck = null;
    // Reference to the camera that will determine our forward direction.
    [SerializeField] private Transform camTrans = null;

    // Reference to the character controller attached to this object.
    private CharacterController charContRef = null;

    // Raw input direction for movement
    private Vector3 rawMoveDirection = Vector3.zero;
    // Current velocity due to gravity
    private Vector3 gravityVelocity = Vector3.zero;
    // Current movement direction
    private Vector3 movementDirection = Vector3.zero;
    // If the player is moving
    private bool isMoving = false;
    // If the player is sprinting
    private bool isSprinting = false;
    // If the player is grounded.
    private bool isGrounded = true;


    // Unity functions like Awake and Update
    #region UnityMessages
    // Called 0th
    // Set references
    protected virtual void Awake()
    {
        charContRef = GetComponent<CharacterController>();
    }
    // Called at a fixed interval
    // Used for physics calculations
    protected virtual void FixedUpdate()
    {
        // Apply gravity velocity and handle if we are grounded
        gravityVelocity.y = DetermineGravityVelocity(gravityVelocity.y);

        // Get the movement direction and velocity
        movementDirection = DetermineMovementDirection();
        Vector3 moveVelocity = DetermineMovementVelocity(movementDirection);

        // Apply the velocities to the actual character
        charContRef.Move((moveVelocity + gravityVelocity) * Time.deltaTime);
    }
    #endregion UnityMessages


    /// <summary>
    /// Sets the direction the character should move
    /// in relative to the camera's transform's forward.
    /// </summary>
    /// <param name="moveInput">Raw movement input to use.</param>
    public void SetRawMovementInputVector(Vector3 moveInput)
    {
        isMoving = true;
        rawMoveDirection = moveInput;
    }
    /// <summary>
    /// Stops the character from moving.
    /// </summary>
    public void StopMoving()
    {
        isMoving = false;
        rawMoveDirection = Vector3.zero;
    }
    /// <summary>
    /// Starts or stops sprinting.
    /// </summary>
    /// <param name="shouldSprint">If we should sprint or not.</param>
    public void ToggleSprinting(bool shouldSprint)
    {
        isSprinting = shouldSprint;
    }
    /// <summary>
    /// Jumps if the character is grounded. Otherwise does nothing.
    /// </summary>
    public void Jump()
    {
        if (isGrounded)
        {
            gravityVelocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }
    }

    /// <summary>
    /// Returns the character's current movement direction.
    /// Returns Vector3.zero when there is no current movement direction.
    /// </summary>
    /// <returns>Current movement direction.</returns>
    public Vector3 GetMovementDirection()
    {
        return movementDirection;
    }


    /// <summary>
    /// Check if the player is grounded and apply gravity to gravityVelocity accordingly.
    /// Causes the player to fall due to gravity.
    /// </summary>
    private float DetermineGravityVelocity(float currentGravityVel)
    {
        // Check if the player is on the ground.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // Update velocity due to gravity.
        float gravityVelocity = currentGravityVel - gravity * Time.deltaTime;
        // Check if we should reset velocity.
        if (isGrounded && gravityVelocity < 0)
        {
            gravityVelocity = 0.0f;
        }

        return gravityVelocity;
    }
    /// <summary>
    /// Calculates the direction at which the player should move.
    /// </summary>
    /// <returns>Direction the player should walk/run using.</returns>
    private Vector3 DetermineMovementDirection()
    {
        if (isMoving)
        {
            // Determine the target angle from the move angle determiner
            float targetAngle = DetermineTargetMovementAngle(rawMoveDirection);

            // Have the move direciton ot match the target angle.
            return (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
        }
        else
        {
            return Vector3.zero;
        }
    }
    /// <summary>
    /// Applies the current speed to the given movement direction whether sprinting or walking.
    /// </summary>
    /// <param name="movementDirection">Direction to move the character in.</param>
    /// <returns>Velocity to move the character in the given direction.</returns>
    private Vector3 DetermineMovementVelocity(Vector3 movementDirection)
    {
        // Determine speed based on if sprinting
        float curSpeed = DetermineSpeed();
        // Apply the speed
        return movementDirection * curSpeed;
    }
    /// <summary>
    /// Determines which direction the character should move.
    /// </summary>
    /// <param name="rawMoveDir">Raw movement input axis.</param>
    /// <returns>Angle (Degrees) to move the character at.</returns>
    private float DetermineTargetMovementAngle(Vector3 rawMoveDir)
    {
        // Set the target angle to move at to be based on input and the camera's angle.
        float targetAngle = Mathf.Atan2(rawMoveDir.x, rawMoveDir.z) * Mathf.Rad2Deg + camTrans.eulerAngles.y;
        // Return the previously calculated target angle.
        return targetAngle;
    }
    /// <summary>
    /// Helper function to determine if the player should move at sprint speed or walking speed.
    /// </summary>
    /// <returns></returns>
    private float DetermineSpeed()
    {
        return isSprinting ? sprintSpeed : speed;
    }
}
