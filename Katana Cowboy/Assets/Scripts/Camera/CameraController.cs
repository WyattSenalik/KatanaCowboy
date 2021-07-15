using UnityEngine;

using Cinemachine;

public class CameraController : MonoBehaviour
{
    // Camera types.
    public enum CameraTypes { Standard, Aim };

    // Reference to the main camera's transform.
    [SerializeField] private Transform mainCamTrans = null;
    // References to the different cameras.
    // Standard third person camera.
    [SerializeField] private CinemachineFreeLook thirdPersonCamera = null;
    // Camera for aiming in third person
    [SerializeField] private CinemachineFreeLook aimCamera = null;

    // References to ui that needs to be turned on per camera.
    // UI and objects that should be active when swapping to standard.
    [SerializeField] private GameObject[] standardObjects = new GameObject[0];
    // UI and objects that should be active when swapping to aim.
    [SerializeField] private GameObject[] aimObjects = new GameObject[0];

    // Current camera that is active.
    private CinemachineFreeLook activeCamera = null;
    private CameraTypes activeCam = CameraTypes.Standard;


    // Called before the first frame.
    private void Start()
    {
        // Initialially, be at a normal camera.
        ActivateDefaultCamera();
    }


    /// <summary>
    /// Activates the Default Camera and sets the rotation type for the player controller.
    /// </summary>
    public void ActivateDefaultCamera()
    {
        ChangeCamera(PlayerController.ControlType.Standard, CameraTypes.Standard);
    }
    /// <summary>
    /// Activates the Aim Camera and sets the rotation type for the player controller.
    /// </summary>
    public void ActivateAimCamera()
    {
        ChangeCamera(PlayerController.ControlType.Aim, CameraTypes.Aim);
    }


    /// <summary>
    /// Helper function for the activate camera functions.
    /// Turns on things specific for each camera.
    /// </summary>
    /// <param name="controlType">Type of control type to give the player.</param>
    /// <param name="activeCamType">Type of comera type to use.</param>
    private void ChangeCamera(PlayerController.ControlType controlType, CameraTypes activeCamType)
    {
        // First disable all the cameras.
        DisableAllCameras();
        // Hide all ui.
        HideAllUI();
        // Enable the one active cam and its UI.
        switch (activeCamType)
        {
            case (CameraTypes.Standard):
                ActivateCamera(ref thirdPersonCamera, ref standardObjects);
                break;
            case (CameraTypes.Aim):
                ActivateCamera(ref aimCamera, ref aimObjects);
                break;
            default:
                Debug.LogError("Unkown CameraType " + activeCamType);
                break;
        }
        // Set which camera is active.
        activeCam = activeCamType;
    }
    /// <summary>
    /// Activates the given camera and its associated objects.
    /// </summary>
    /// <param name="camera">Camera to activate.</param>
    /// <param name="cameraObjs">Object to activate along with the camera.</param>
    private void ActivateCamera(ref CinemachineFreeLook camera, ref GameObject[] cameraObjs)
    {
        if (activeCamera != null)
        {
            camera.m_XAxis.Value = activeCamera.m_XAxis.Value;
            camera.m_YAxis.Value = activeCamera.m_YAxis.Value;
        }
        camera.gameObject.SetActive(true);
        foreach (GameObject obj in cameraObjs)
        {
            obj.SetActive(true);
        }

        activeCamera = camera;
    }
    /// <summary>
    /// Helper function for ChangeCamera.
    /// Disables all the cameras.
    /// </summary>
    private void DisableAllCameras()
    {
        thirdPersonCamera.gameObject.SetActive(false);
        aimCamera.gameObject.SetActive(false);
    }
    /// <summary>
    /// Helper function for ChangeCamera.
    /// Turns off all UI tied to all cameras.
    /// </summary>
    private void HideAllUI()
    {
        // Turn off all standard ui.
        foreach (GameObject obj in standardObjects)
            obj.SetActive(false);
        // Turn off all aim ui.
        foreach (GameObject obj in aimObjects)
            obj.SetActive(false);
    }
}
