using UnityEngine;
using UnityEngine.InputSystem;
using GameEventSystem;

public class AimCameraController : MonoBehaviour
{
    // Camera to rotate.
    [SerializeField] private Transform headTrans = null;

    // Speed the camera will look up.
    [SerializeField] private float rotateSpeed = 2f;

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
            // Update rotation
            Vector3 eulerRot = headTrans.transform.eulerAngles;
            eulerRot.x += axis * rotateSpeed * Time.deltaTime;
            headTrans.transform.rotation = Quaternion.Euler(eulerRot);
        }
    }
}