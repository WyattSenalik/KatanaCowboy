using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{
    // For what kind of controls the player should be using.
    public enum ControlType { STANDARD, AIM };

    // Customization.
    // Speed the character will move at.
    [SerializeField]
    private float speed = 6f;
    // Speed the character will sprint at.
    [SerializeField]
    private float sprintSpeed = 12f;
    // Smoothing the turning.
    [SerializeField]
    private float turnSmoothTime = 0.1f;
    // Acceleration due to gravity.
    [SerializeField]
    private float gravity = -9.81f;
    // Radius of the sphere created for the ground check.
    [SerializeField]
    private float groundDistance = 0.4f;
    // Mask to check for ground.
    [SerializeField]
    private LayerMask groundMask = 1 << 0;
    // Height to reach on a jump.
    [SerializeField]
    private float jumpHeight = 3f;
    // Speed to rotate camera while aiming.
    [SerializeField]
    private float aimRotateSpeedX = 2f;
    [SerializeField]
    private float aimRotateSpeedY = 300f;

    // References.
    // Reference to the camera that will determine our forward direction.
    [SerializeField]
    private Transform camTrans = null;
    // Reference to the ground check.
    [SerializeField]
    private Transform groundCheck = null;
    // Reference to the character controller attached to this object.
    private CharacterController charContRef;

    // Holds the turn velocity to smooth the turn.
    // Should never be changed by this script.
    private float turnSmoothVelocity = 0;
    // Target direction to face
    private Vector3 rawMoveDirection = Vector3.zero;
    // Current velocity due to gravity
    private Vector3 gravityVelocity = Vector3.zero;
    // If the player is moving
    private bool isMoving = false;
    // If the player is sprinting
    private bool isSprinting = false;
    // If the player is grounded.
    private bool isGrounded = true;
    // If the player should follow its movement with rotation or follow the camera with its rotation
    private ControlType rotType = ControlType.AIM;

    // Sword attack control variables.
    // Reference to the sword controller for handling animation and such on the sword.
    [SerializeField]
    private SwordController swordContRef = null;
    // If the player is attacking with their sword right now, we don't want them to be able to move.
    private bool isAttacking = false;

    // Called 0th
    // Set references
    private void Awake()
    {
        charContRef = GetComponent<CharacterController>();
    }
    // Called at a fixed interval
    // Used for physics calculations
    private void FixedUpdate()
    {
        // Apply gravity velocity and handle if we are grounded
        ApplyGravityVelocity();
        // Apply rotation to the character so they are facing towards where they are walking and
        // apply a movement velocity towards teh direction the camera is facing
        Vector3 moveVelocity = DetermineMovementVelocity();

        // Apply the velocities to the actual character
        charContRef.Move((moveVelocity + gravityVelocity) * Time.deltaTime);
    }


    /// <summary>
    /// Gives the player a direction to move in and sets if the player is moving or not.
    /// Called by the unity input events.
    /// </summary>
    public void OnMovement(InputAction.CallbackContext context)
    {
        // If we pressed down and are not currently attacking
        if (context.performed && !isAttacking)
        {
            isMoving = true;
            // Get movement input
            Vector2 rawAxis = context.ReadValue<Vector2>();
            rawMoveDirection = new Vector3(rawAxis.x, 0, rawAxis.y).normalized;
        }
        else
        {
            isMoving = false;
        }
    }
    /// <summary>
    /// Sets is sprinting to true or false.
    /// Called by the unity input events.
    /// </summary>
    public void OnSprint(InputAction.CallbackContext context)
    {
        // Set if the player is sprinting
        isSprinting = context.performed;
    }
    /// <summary>
    /// Starts jumping if the player is on the ground.
    /// Called by the unity input events.
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            gravityVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    /// <summary>
    /// Starts the attacking animation and sets is attacking to true.
    /// Called by the unity input events.
    /// </summary>
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            switch (rotType)
            {
                // Start attacking with the sword
                case ControlType.STANDARD:
                    swordContRef.StartSwingAnimation();
                    isAttacking = true;
                    break;
                // Shoot with the gun will be handled in gun controller
                case ControlType.AIM:
                    break;
                default:
                    Debug.LogError("Unhandled ControlType " + rotType);
                    break;
            }
        }
    }
    /// <summary>
    /// Updates the player's rotation based on camera view input.
    /// Called by the unity input events.
    /// </summary>
    public void OnAimLook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Get the raw input.
            float rotVal = context.ReadValue<float>();

            // Change the rotation of the character.
            Vector3 eulerRot = transform.eulerAngles;
            eulerRot.y += rotVal * aimRotateSpeedY * Time.deltaTime;
            transform.rotation = Quaternion.Euler(eulerRot);
        }
    }


    /// <summary>
    /// Called by animator to let the controller know it has finished attacking.
    /// </summary>
    public void FinishAttack()
    {
        isAttacking = false;
    }
    /// <summary>
    /// Sets the player's rotation type to swap between different modes.
    /// FOLLOWMOVE - player will rotate towards where they are moving.
    /// FOLLOWCAM - player will rotate towards where they are looking.
    /// </summary>
    /// <param name="_rotType_">Type of rotation to set the player to.</param>
    public void SetRotationType(ControlType _rotType_)
    {
        rotType = _rotType_;
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
        gravityVelocity.y += gravity * Time.deltaTime;
        // Check if we should reset velocity.
        if (isGrounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = -2f;
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
            // Determine the target angle to move at
            float targetAngle = DetermineTargetAngle();
            // Determine speed based on if sprinting
            float curSpeed = DetermineSpeed();

            // Change where the character is facing
            // Get the angle we rotate the character so we don't simply snap and set the rotation of the character.
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            // Change the rotation of the character.
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

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

    /// <summary>
    /// Helper function to determine which movement angle we want based on the current rotation type.
    /// </summary>
    /// <returns>float (euler angle) targetAngle that the player should move towards.</returns>
    private float DetermineTargetAngle()
    {
        // Handle the different movement types.
        switch (rotType)
        {
            // Standard 3rd person rotation.
            case (ControlType.STANDARD):
                return DetermineStandardMoveAngle();
            // Over the shoulder aiming.
            case (ControlType.AIM):
                return DetermineAimMoveAngle();
            default:
                Debug.LogError("Unknown control type " + rotType);
                return 0;
        }
    }
    /// <summary>
    /// Helper function to determine which direction the player should move in
    /// when the camera is the standard 3rd person camera.
    /// </summary>
    /// <returns>float (euler angle) targetAngle that the player should move towards.</returns>
    private float DetermineStandardMoveAngle()
    {
        // Set the target angle to move at to be based on input and the camera's angle.
        return Mathf.Atan2(rawMoveDirection.x, rawMoveDirection.z) * Mathf.Rad2Deg + camTrans.eulerAngles.y;
    }
    /// <summary>
    /// Handles which direction to move for the aim control type.
    /// </summary>
    /// <param name="direction">Holds x and y input information.</param>
    /// <returns>float targetAngle that the player should move towards.</returns>
    private float DetermineAimMoveAngle()
    {
        // Set the target angle to move at to be based on only the input.
        float targetAngle = Mathf.Atan2(rawMoveDirection.x, rawMoveDirection.z) * Mathf.Rad2Deg + transform.eulerAngles.y;
        return targetAngle;
    }
}
