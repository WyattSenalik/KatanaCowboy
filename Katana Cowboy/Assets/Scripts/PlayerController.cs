using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ThirdPersonMoveAngleDeterminer))]
[RequireComponent(typeof(OverShoulderMoveAngleDeterminer))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // For what kind of controls the player should be using.
    public enum ControlType { Standard, Aim };

    
    // Speed to rotate camera while aiming.
    [SerializeField] private float aimRotateSpeedX = 2f;
    [SerializeField] private float aimRotateSpeedY = 300f;

    // References
    // Reference to the camera controller for handling swapping between cameras.
    [SerializeField] private CameraController camContRef = null;
    // Reference to the sword controller for handling animation and such on the sword.
    [SerializeField] private SwordController swordContRef = null;
    // Reference to the gun controller for handling shooting when aimed.
    [SerializeField] private GunController gunContRef = null;

    // Reference to the player's character movement script
    private CharacterMovement charMoveRef = null;
    // Third person movement reference for standard controls
    private ThirdPersonMoveAngleDeterminer thirdPersonMoveRef = null;
    // Over the shoulder movement reference for aim controls
    private OverShoulderMoveAngleDeterminer overShoulderMoveRef = null;
    // If the player should follow its movement with rotation or follow the camera with its rotation
    private ControlType contType = ControlType.Standard;
    private ControlType CurrentControlType
    {
        get { return contType; }
        set
        {
            contType = value;
            UpdateControlType();
        }
    }

    // Sword attack control variables.
    // If the player is attacking with their sword right now, we don't want them to be able to move.
    private bool isAttacking = false;


    // Called 0th
    // Set references
    private void Awake()
    {
        thirdPersonMoveRef = GetComponent<ThirdPersonMoveAngleDeterminer>();
        overShoulderMoveRef = GetComponent<OverShoulderMoveAngleDeterminer>();
        charMoveRef = GetComponent<CharacterMovement>();
    }
    // Called 1st
    // Initialization
    private void Start()
    {
        // Default control sceme is third person
        charMoveRef.SetMoveAngleDeterminer(thirdPersonMoveRef);
        CurrentControlType = ControlType.Standard;
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
            // Get movement input
            Vector2 rawAxis = context.ReadValue<Vector2>();
            Vector3 rawMoveDir = new Vector3(rawAxis.x, 0, rawAxis.y).normalized;
            // Move the character
            charMoveRef.SetRawMovementInputVector(rawMoveDir);
        }
        else
        {
            // Stop moving the character if we are no longer giving input
            charMoveRef.StopMoving();
        }
    }
    /// <summary>
    /// Sets is sprinting to true or false.
    /// Called by the unity input events.
    /// </summary>
    public void OnSprint(InputAction.CallbackContext context)
    {
        // Set if the player is sprinting
        charMoveRef.ToggleSprinting(context.performed);
    }
    /// <summary>
    /// Starts jumping if the player is on the ground.
    /// Called by the unity input events.
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Have the player try to jump
            charMoveRef.Jump();
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
            switch (CurrentControlType)
            {
                // Start attacking with the sword
                case ControlType.Standard:
                    swordContRef.StartSwingAnimation();
                    isAttacking = true;
                    break;
                // Shoot with the gun will be handled in gun controller
                case ControlType.Aim:
                    gunContRef.Shoot();
                    break;
                default:
                    Debug.LogError("Unhandled ControlType " + CurrentControlType);
                    break;
            }
        }
    }
    /// <summary>
    /// Swaps the player to and from aiming.
    /// Called by the unity input events.
    /// </summary>
    public void OnAim(InputAction.CallbackContext context)
    {
        // Aim pressed
        if (context.performed)
        {
            CurrentControlType = ControlType.Aim;
        }
        // Aim released
        else if (context.canceled)
        {
            CurrentControlType = ControlType.Standard;
        }
    }
    /// <summary>
    /// Updates the player's rotation based on camera view input.
    /// Called by the unity input events.
    /// </summary>
    public void OnAimLook(InputAction.CallbackContext context)
    {
        if (context.performed && CurrentControlType == ControlType.Aim)
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
    /// Sets the player's control type to swap between different control schemes.
    /// Standard - player will rotate towards where they are moving.
    /// Aim - player will rotate towards where they are looking.
    /// </summary>
    private void UpdateControlType()
    {
        switch (CurrentControlType)
        {
            case ControlType.Standard:
                charMoveRef.SetMoveAngleDeterminer(thirdPersonMoveRef);
                gunContRef.ToggleActive(false);
                camContRef.ActivateDefaultCamera();
                break;
            case ControlType.Aim:
                charMoveRef.SetMoveAngleDeterminer(overShoulderMoveRef);
                gunContRef.ToggleActive(true);
                camContRef.ActivateAimCamera();
                break;
            default:
                break;
        }
    }
}