using UnityEngine;

public class AimCameraController : MonoBehaviour
{
    // Head to rotate.
    [SerializeField]
    private GameObject head = null;

    // Speed the camera will look up.
    [SerializeField]
    private float rotateSpeed = 2f;

    // If the input axis should be inverted.
    [SerializeField]
    private bool invertAxis = true;

    // Called once every frame.
    private void Update()
    {
        // Get vertical look input.
        float yAxis = Input.GetAxisRaw("Mouse Y");

        if (yAxis != 0f)
        {
            // Invert the axis if needed.
            yAxis = invertAxis ? -yAxis : yAxis;
            // Update rotation.
            Vector3 eulerRot = head.transform.eulerAngles;
            eulerRot.x += yAxis * rotateSpeed * Time.deltaTime;
            head.transform.rotation = Quaternion.Euler(eulerRot);
        }
    }
}