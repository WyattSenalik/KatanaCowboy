using UnityEngine;

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
    private LayerMask groundMask;
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
    // Current velocity due to gravity.
    private Vector3 velocity = Vector3.zero;
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

    // Called 0th before Start
    private void Awake()
    {
        // Try to set references
        try
        {
            charContRef = this.GetComponent<CharacterController>();
        }
        catch
        {
            Debug.LogError("Could not set references in ThirdPersonMovement attached to " + this.name);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // See if the player is attacking
        HandleAttack();
        // Handle moving along x and z axes.
        if (!isAttacking)
        {
            PlaneMovement();
        }
        // Handle moving along y axis.
        VerticalMovement();
    }

    /// <summary>
    /// Move the player based on horizontal and vertical input axes on
    /// the x and z axes in game.
    /// </summary>
    private void PlaneMovement()
    {
        // Get movement input.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // If there was input, move the character.
        if (direction.magnitude >= 0.1f)
        {
            // Set target angle based on input.
            float targetAngle;

            // Handle the different movement types.
            switch (rotType)
            {
                // Standard 3rd person rotation.
                case (ControlType.STANDARD):
                    targetAngle = StandardMoveAngle(direction);
                    break;
                // Over the shoulder aiming.
                case (ControlType.AIM):
                    targetAngle = AimMoveAngle(direction);
                    break;
                default:
                    Debug.LogError("Unknown control type " + rotType);
                    targetAngle = 0;
                    break;
            }

            // Get if the player is sprinting.
            bool isSprinting = Input.GetButton("Sprint");
            float curSpeed = isSprinting ? sprintSpeed : speed;

            // Move the character based on the target angle.
            Vector3 moveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
            charContRef.Move(moveDirection * curSpeed * Time.deltaTime);
        }

        // Handle non-movement dependent things.
        // Update the rotation when aiming.
        AimRotation();
    }

    /// <summary>
    /// Cause the player to fall due to gravity and jump based on jump button input.
    /// </summary>
    private void VerticalMovement()
    {
        // Check if the player is on the ground.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // Update velocity due to gravity.
        velocity.y += gravity * Time.deltaTime;
        charContRef.Move(velocity * Time.deltaTime);
        // Check if we should reset velocity.
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // If we are on the ground and try to jump, jump.
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    /// <summary>
    /// Starts the attacking animation in the animator.
    /// </summary>
    private void HandleAttack()
    {
        // If the user presses to attack.
        if (Input.GetButtonDown("SwordAttack"))
        {
            swordContRef.StartSwingAnimation();
            isAttacking = true;
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
    /// Helper function for PlaneMovement.
    /// Handles which direction to move for the standard control type.
    /// </summary>
    /// <param name="direction">Holds x and y input information.</param>
    /// <returns>float targetAngle that the player should move towards.</returns>
    private float StandardMoveAngle(Vector3 direction)
    {
        // Set the target angle to move at to be based on input and the camera's angle.
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camTrans.eulerAngles.y;

        // Get the angle we rotate the character so we don't simply snap and set the rotation of the character.
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        // Change the rotation of the character.
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        return targetAngle;
    }

    /// <summary>
    /// Helper function for PlaneMovement.
    /// Handles which direction to move for the aim control type.
    /// </summary>
    /// <param name="direction">Holds x and y input information.</param>
    /// <returns>float targetAngle that the player should move towards.</returns>
    private float AimMoveAngle(Vector3 direction)
    {
        // Set the target angle to move at to be based on only the input.
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + transform.eulerAngles.y;
        return targetAngle;
    }

    /// <summary>
    /// Helper function for PlaneMovement.
    /// Updates the player's rotation based on camera view input.
    /// </summary>
    private void AimRotation()
    {
        // Spin the player based on the (mouse)/(camera view) input.
        float xAxis = Input.GetAxisRaw("Mouse X");

        if (xAxis != 0f)
        {
            // Change the rotation of the character.
            Vector3 eulerRot = transform.eulerAngles;
            eulerRot.y += xAxis * aimRotateSpeedY * Time.deltaTime;
            transform.rotation = Quaternion.Euler(eulerRot);
        }
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
}
