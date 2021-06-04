using UnityEngine;
using UnityEngine.InputSystem;
using GameEventSystem;

public class AimCameraController : MonoBehaviour
{
    // Camera's look at target that will be moved up and down
    [SerializeField] private Transform lookAtTarget = null;
    // Speed the camera will look up.
    [SerializeField] private float rotateSpeed = 2f;
    [SerializeField] private float targetDefaultYPos = 1.25f;
    [SerializeField] private float targetMaxYPos = 2.3f;
    [SerializeField] private float targetMinYPos = 0.5f;

    // If the input axis should be inverted.
    [SerializeField] private bool invertAxis = true;

    // Events
    // Input Events
    [Space]
    [Header("Input Events")]
    [SerializeField] private GameEventIdentifier aimLookVerticalEventID = null;


    // Called when the component is enabled
    private void OnEnable()
    {
        // Subscribe to events (Can only do this here because this gameobject starts disabled)
        aimLookVerticalEventID.Subscribe(OnAimLookVertical);
    }
    // Called when the component is disabled
    private void OnDisable()
    {
        // Unsubscribe from events
        aimLookVerticalEventID.Unsubscribe(OnAimLookVertical);
    }

    /// <summary>
    /// Rotates the head (camera's pivot) based on the input axis value.
    /// Called by the unity input events.
    /// </summary>
    public void OnAimLookVertical(GameEventData data)
    {
        InputAction.CallbackContext context = data.ReadValue<InputAction.CallbackContext>();
        if (context.performed)
        {
            float axis = context.ReadValue<float>();
            // Invert the axis if needed
            axis = invertAxis ? -axis : axis;

            // Move the look at target within the bounds
            float yValue = lookAtTarget.localPosition.y + axis * rotateSpeed * Time.deltaTime;
            if (yValue > targetMaxYPos)
            {
                yValue = targetMaxYPos;
            }
            else if (yValue < targetMinYPos)
            {
                yValue = targetMinYPos;
            }
            Vector3 pos = lookAtTarget.localPosition;
            pos.y = yValue;
            lookAtTarget.localPosition = pos;
            /*
            // Update rotation
            Vector3 eulerRot = lookAtTarget.transform.eulerAngles;
            eulerRot.x += axis * rotateSpeed * Time.deltaTime;
            lookAtTarget.transform.rotation = Quaternion.Euler(eulerRot);
            */
        }
    }
}