using UnityEngine;
using UnityEngine.InputSystem;

using GameEventSystem;

/// <summary>
/// Listens for player input and controls what happens to the player character based on those inputs.
/// Interfaces between the CharacterMovement, CharacterRotator, CameraController, SwordController, and GunController.
/// </summary>
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CharacterRotator))]
public class PlayerController : MonoBehaviour
{
    // For what kind of controls the player should be using.
    public enum ControlType { Standard, Aim };

    // References
    // Camera controller for handling swapping between cameras.
    [SerializeField] private CameraController camContRef = null;
    // Sword controller for handling animation and such on the sword.
    [SerializeField] private SwordController swordContRef = null;
    // Gun controller for handling shooting when aimed.
    [SerializeField] private GunController gunContRef = null;

    // Aim controller
    // Speed to rotate camera while aiming.
    [SerializeField] private float aimRotateSpeedY = 50.0f;
    // Smoothing the turning.
    [SerializeField] private float turnSmoothTime = 0.1f;

    // Reference to the player character movement script
    private CharacterMovement charMoveRef = null;
    // Reference to the player character's rotator script
    private CharacterRotator charRotatorRef = null;
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

    private Vector3 moveVector = Vector3.zero;

    // If we are subscribed to enable/disable events
    private bool isSubscribed = false;


    // Functions called by unity messages (ex: Start, Awake, Update, etc.)
    #region UnityEvents
    // Called 0th
    // Domestic Initialization
    private void Awake()
    {
        // Set references
        charMoveRef = GetComponent<CharacterMovement>();
        charRotatorRef = GetComponent<CharacterRotator>();

        // Default control scheme is standard third person
        CurrentControlType = ControlType.Standard;
    }
    // Called when the component is enabled
    private void OnEnable()
    {
        // Subscribe to events
        SubscribeToEvents();
    }
    // Called when the component is disabled
    private void OnDisable()
    {
        // Unubscribe from events
        UnsubscribeFromEvents();
    }
    // Called every frame
    private void Update()
    {
        // Handle aiming rotation behavior
        if (CurrentControlType == ControlType.Aim)
        {
            Vector3 aimDirection = gunContRef.GetAimDirection();
            charRotatorRef.RotateCharacter(aimDirection);
        }

        if (!swordContRef.IsAttacking)
        {
            if (moveVector.sqrMagnitude > 0)
            {
                charMoveRef.SetRawMovementInputVector(moveVector);
            }
            else
            {
                charMoveRef.StopMoving();
            }
        }
    }
    #endregion UnityEvents


    // Functions that subscribe and unsubscribe from the events this script listens to
    #region EventSubscriptions
    /// <summary>
    /// Subscribes to events this script listens to.
    /// </summary>
    private void SubscribeToEvents()
    {
        if (!isSubscribed)
        {
            // Input events
            //EventSystem.SubscribeToEvent(EventIDList.Movement, OnMovement);
            //EventSystem.SubscribeToEvent(EventIDList.Sprint, OnSprint);
            //EventSystem.SubscribeToEvent(EventIDList.Jump, OnJump);
            //EventSystem.SubscribeToEvent(EventIDList.Attack, OnAttack);
            //EventSystem.SubscribeToEvent(EventIDList.Aim, OnAim);
            //EventSystem.SubscribeToEvent(EventIDList.AimLook, OnAimLook);

            isSubscribed = true;
        }
    }
    /// <summary>
    /// Unsubscribes from events this listens to.
    /// </summary>
    private void UnsubscribeFromEvents()
    {
        if (isSubscribed)
        {
            // Input events
            //EventSystem.UnsubscribeFromEvent(EventIDList.Movement, OnMovement);
            //EventSystem.UnsubscribeFromEvent(EventIDList.Sprint, OnSprint);
            //EventSystem.UnsubscribeFromEvent(EventIDList.Jump, OnJump);
            //EventSystem.UnsubscribeFromEvent(EventIDList.Attack, OnAttack);
            //EventSystem.UnsubscribeFromEvent(EventIDList.Aim, OnAim);
            //EventSystem.UnsubscribeFromEvent(EventIDList.AimLook, OnAimLook);

            isSubscribed = false;
        }
    }
    #endregion EventSubscriptions


    // Functions called by the event system
    #region EventCallbacks
    /// <summary>
    /// Gives the player a direction to move in and sets if the player is moving or not.
    /// Called by the unity input events.
    /// </summary>
    private void OnMovement(InputAction.CallbackContext context)
    {
        // If we pressed down and are not currently attacking
        if (context.performed)
        {
            // Get movement input
            Vector2 rawAxis = context.ReadValue<Vector2>();
            Vector3 rawMoveDir = new Vector3(rawAxis.x, 0, rawAxis.y).normalized;
            // Move the character
            moveVector = rawMoveDir;
        }
        else
        {
            // Stop moving the character if we are no longer giving input
            moveVector = Vector3.zero;
        }
    }
    /// <summary>
    /// Sets is sprinting to true or false.
    /// Called by the unity input events.
    /// </summary>
    private void OnSprint(InputAction.CallbackContext context)
    {
        // Set if the player is sprinting
        charMoveRef.ToggleSprinting(context.performed);
    }
    /// <summary>
    /// Starts jumping if the player is on the ground.
    /// Called by the unity input events.
    /// </summary>
    private void OnJump(InputAction.CallbackContext context)
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
    private void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            switch (CurrentControlType)
            {
                // Start attacking with the sword
                case ControlType.Standard:
                    if (!swordContRef.IsAttacking)
                    {
                        swordContRef.StartSwingAnimation();
                        charMoveRef.StopMoving();
                    }
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
    private void OnAim(InputAction.CallbackContext context)
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
    private void OnAimLook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
        }
    }
    #endregion EventCallbacks


    /// <summary>
    /// Sets the player's control type to swap between different control schemes.
    /// Standard - player will rotate towards where they are moving.
    /// Aim - player will rotate towards where they are looking.
    /// </summary>
    private void UpdateControlType()
    {
        // For each control type:
        // 1. Change the rotator
        // 2. Toggle the weapon active
        // 3. Activate the corresponding camera
        switch (CurrentControlType)
        {
            case ControlType.Standard:
                charRotatorRef.ToggleRotationFollowsMovement(true);
                gunContRef.ToggleActive(false);
                camContRef.ActivateDefaultCamera();
                break;
            case ControlType.Aim:
                charRotatorRef.ToggleRotationFollowsMovement(false);
                gunContRef.ToggleActive(true);
                camContRef.ActivateAimCamera();
                break;
            default:
                break;
        }
    }
}