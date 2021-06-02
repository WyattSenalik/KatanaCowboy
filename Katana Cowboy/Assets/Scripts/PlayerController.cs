using UnityEngine;
using UnityEngine.InputSystem;
using GameEventSystem;

/// <summary>
/// Listens for player input and controls what happens to the player character based on those inputs.
/// Interfaces between the CharacterMovement, CameraController, SwordController, and GunController.
/// </summary>
[RequireComponent(typeof(ThirdPersonMoveAngleDeterminer))]
[RequireComponent(typeof(OverShoulderMoveAngleDeterminer))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // For what kind of controls the player should be using.
    public enum ControlType { Standard, Aim };

    
    // Speed to rotate camera while aiming.
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

    // Events
    // Input Events
    [Space]
    [Header("Input Events")]
    [SerializeField] private GameEventIdentifier movementEventID = null;
    [SerializeField] private GameEventIdentifier sprintEventID = null;
    [SerializeField] private GameEventIdentifier jumpEventID = null;
    [SerializeField] private GameEventIdentifier attackEventID = null;
    [SerializeField] private GameEventIdentifier aimEventID = null;
    [SerializeField] private GameEventIdentifier aimLookEventID = null;


    // Functions called by unity messages (ex: Start, Awake, Update, etc.)
    #region UnityEvents
    // Called 0th
    // Domestic Initialization
    private void Awake()
    {
        // Set references
        thirdPersonMoveRef = GetComponent<ThirdPersonMoveAngleDeterminer>();
        overShoulderMoveRef = GetComponent<OverShoulderMoveAngleDeterminer>();
        charMoveRef = GetComponent<CharacterMovement>();

        // Default control scheme is standard third person
        CurrentControlType = ControlType.Standard;
    }
    // Called 1st
    // Foreign Initialization
    private void Start()
    {
        // Default control sceme is third person
        charMoveRef.SetMoveAngleDeterminer(thirdPersonMoveRef);
    }
    // Called when the component is enabled
    private void OnEnable()
    {
        // Subscribe to events
        movementEventID.Subscribe(OnMovement);
        sprintEventID.Subscribe(OnSprint);
        jumpEventID.Subscribe(OnJump);
        attackEventID.Subscribe(OnAttack);
        aimEventID.Subscribe(OnAim);
        aimLookEventID.Subscribe(OnAimLook);
    }
    // Called when the component is disabled
    private void OnDisable()
    {
        // Unubscribe from events
        movementEventID.Unsubscribe(OnMovement);
        sprintEventID.Unsubscribe(OnSprint);
        jumpEventID.Unsubscribe(OnJump);
        attackEventID.Unsubscribe(OnAttack);
        aimEventID.Unsubscribe(OnAim);
        aimLookEventID.Unsubscribe(OnAimLook);
    }
    #endregion UnityEvents


    // Functions called by the event system
    #region EventCallbacks
    /// <summary>
    /// Gives the player a direction to move in and sets if the player is moving or not.
    /// Called by the unity input events.
    /// </summary>
    private void OnMovement(GameEventData eventData)
    {
        InputAction.CallbackContext context = eventData.ReadValue<InputAction.CallbackContext>();
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
    private void OnSprint(GameEventData eventData)
    {
        InputAction.CallbackContext context = eventData.ReadValue<InputAction.CallbackContext>();
        // Set if the player is sprinting
        charMoveRef.ToggleSprinting(context.performed);
    }
    /// <summary>
    /// Starts jumping if the player is on the ground.
    /// Called by the unity input events.
    /// </summary>
    private void OnJump(GameEventData eventData)
    {
        InputAction.CallbackContext context = eventData.ReadValue<InputAction.CallbackContext>();
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
    private void OnAttack(GameEventData eventData)
    {
        InputAction.CallbackContext context = eventData.ReadValue<InputAction.CallbackContext>();
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
    private void OnAim(GameEventData eventData)
    {
        InputAction.CallbackContext context = eventData.ReadValue<InputAction.CallbackContext>();
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
    private void OnAimLook(GameEventData eventData)
    {
        InputAction.CallbackContext context = eventData.ReadValue<InputAction.CallbackContext>();
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
    #endregion EventCallbacks


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
        // For each control type:
        // 1. Swap the angle determiner
        // 2. Toggle the weapon active
        // 3. Activate the corresponding camera
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