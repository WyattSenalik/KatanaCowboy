using UnityEngine;
using UnityEngine.InputSystem;

public class AimCameraController : MonoBehaviour
{
    // Head to rotate.
    [SerializeField] private GameObject head = null;

    // Speed the camera will look up.
    [SerializeField] private float rotateSpeed = 2f;

    // If the input axis should be inverted.
    [SerializeField] private bool invertAxis = true;


    /// <summary>
    /// Rotates the head (camera's pivot) based on the input axis value.
    /// Called by the unity input events.
    /// </summary>
    public void OnAimLookVertical(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float axis = context.ReadValue<float>();
            // Invert the axis if needed
            axis = invertAxis ? -axis : axis;
            // Update rotation
            Vector3 eulerRot = head.transform.eulerAngles;
            eulerRot.x += axis * rotateSpeed * Time.deltaTime;
            head.transform.rotation = Quaternion.Euler(eulerRot);
        }
    }
}