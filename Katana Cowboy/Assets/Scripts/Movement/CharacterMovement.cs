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
    // Reference to the character controller attached to this object.
    private CharacterController charContRef = null;
    // Reference to the move angle determiner specified by a player controller
    private CharacterMoveAngleDeterminer moveAngleDeterminer = null;

    // Raw input direction for movement
    private Vector3 rawMoveDirection = Vector3.zero;
    // Current velocity due to gravity
    private Vector3 gravityVelocity = Vector3.zero;
    // If the player is moving
    private bool isMoving = false;
    // If the player is sprinting
    private bool isSprinting = false;
    // If the player is grounded.
    private bool isGrounded = true;


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
        // Quick check to make sure
        if (moveAngleDeterminer == null)
        {
            Debug.LogError("A CharacterMoveAngleDeterminer must be set for "
                + this.name + "'s CharacterMovement script");
        }

        // Apply gravity velocity and handle if we are grounded
        ApplyGravityVelocity();
        // Apply rotation to the character so they are facing towards where they are walking and
        // apply a movement velocity towards the direction the camera is facing
        Vector3 moveVelocity = DetermineMovementVelocity();

        // Apply the velocities to the actual character
        charContRef.Move((moveVelocity + gravityVelocity) * Time.deltaTime);
    }


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
    /// Sets the move angle determiner that determines what angle to move at.
    /// </summary>
    /// <param name="angleDet">Angle determiner to use to decide which direction to move in.</param>
    public void SetMoveAngleDeterminer(CharacterMoveAngleDeterminer angleDet)
    {
        moveAngleDeterminer = angleDet;
    }


    /// <summary>
    /// Check if the player is grounded and apply gravity to gravityVelocity accordingly.
    /// Causes the player to fall due to gravity.
    /// </summary>
    private void ApplyGravityVelocity()
    {
        // Check if the player is on the ground.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // Update velocity due to gravity.
        gravityVelocity.y -= gravity * Time.deltaTime;
        // Check if we should reset velocity.
        if (isGrounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = 0.0f;
        }
    }
    /// <summary>
    /// Calculates the direction and speed at which the player should move
    /// by walking/running. Returns the result.
    /// </summary>
    /// <returns>Movement vector the player should walk/run using.</returns>
    private Vector3 DetermineMovementVelocity()
    {
        if (isMoving)
        {
            // Determine the target angle from the move angle determiner
            float targetAngle = moveAngleDeterminer.DetermineTargetMovementAngle(rawMoveDirection);
            // Determine speed based on if sprinting
            float curSpeed = DetermineSpeed();

            // Move the character based on the target angle.
            Vector3 moveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
            return moveDirection * curSpeed;
        }
        else
        {
            return Vector3.zero;
        }
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
