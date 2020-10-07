using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{
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
    private float turnSmoothVelocity;
    // Current velocity due to gravity.
    private Vector3 velocity;
    // If the player is grounded.
    private bool isGrounded;

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
        // Handle moving along x and z axes.
        PlaneMovement();
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
            // The the target angle to move at.
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camTrans.eulerAngles.y;
            // Get the angle we rotate the character so we don't simply snap and set the rotation of the character.
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Get if the player is sprinting.
            bool isSprinting = Input.GetButton("Sprint");
            float curSpeed = isSprinting ? sprintSpeed : speed;

            // Move the character based on the target angle.
            Vector3 moveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
            charContRef.Move(moveDirection * curSpeed * Time.deltaTime);
        }
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
}
