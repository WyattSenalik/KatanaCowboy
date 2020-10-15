using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Camera types.
    public enum CameraTypes { STANDARD, AIM };

    // References to the different cameras.
    // Standard third person camera.
    [SerializeField]
    private GameObject thirdPersonCamera = null;
    // Camera for aiming in third person
    [SerializeField]
    private GameObject aimCamera = null;

    // References to ui that needs to be turned on per camera.
    // UI and objects that should be active when swapping to standard.
    [SerializeField]
    private GameObject[] standardObjects = new GameObject[0];
    // UI and objects that should be active when swapping to aim.
    [SerializeField]
    private GameObject[] aimObjects = new GameObject[0];

    // Refence to the player's movement controller script.
    [SerializeField]
    private ThirdPersonMovement thirdPersonMove = null;

    // Current camera that is active.
    private CameraTypes activeCam;


    // Called before the first frame.
    private void Start()
    {
        // Initialially, be at a normal camera.
        ChangeCamera(ThirdPersonMovement.ControlType.STANDARD, CameraTypes.STANDARD);
    }

    // Called once every frame.
    private void Update()
    {
        // If the player is holding down aim, then aim.
        float aim = Input.GetAxisRaw("Aim");
        if (aim >= 0.01f)
        {
            // If we aren't already aiming.
            if (activeCam != CameraTypes.AIM)
            {
                ChangeCamera(ThirdPersonMovement.ControlType.AIM, CameraTypes.AIM);
            }
        }
        // If aim is not being held down and we aren't standard, change to standard.
        else if (activeCam != CameraTypes.STANDARD)
        {
            ChangeCamera(ThirdPersonMovement.ControlType.STANDARD, CameraTypes.STANDARD);
        }
    }

    /// <summary>
    /// Activates the Default Camera and sets the rotation type for the player controller.
    /// </summary>
    public void ActivateDefaultCamera()
    {
        ChangeCamera(ThirdPersonMovement.ControlType.STANDARD, CameraTypes.STANDARD);
    }

    /// <summary>
    /// Activates the Aim Camera and sets the rotation type for the player controller.
    /// </summary>
    public void ActivateAimCamera()
    {
        ChangeCamera(ThirdPersonMovement.ControlType.AIM, CameraTypes.AIM);
    }

    /// <summary>
    /// Helper function for the activate camera functions.
    /// Turns off
    /// </summary>
    /// <param name="_controlType_"></param>
    /// <param name="_thirdPerson_"></param>
    /// <param name="_aim_"></param>
    private void ChangeCamera(ThirdPersonMovement.ControlType _controlType_, CameraTypes _activeCam_)
    {
        // First disable all the cameras.
        DisableAllCameras();
        // Hide all ui.
        HideAllUI();
        // Enable the one active cam and its UI.
        switch (_activeCam_)
        {
            case (CameraTypes.STANDARD):
                thirdPersonCamera.SetActive(true);
                foreach (GameObject obj in standardObjects)
                    obj.SetActive(true);
                break;
            case (CameraTypes.AIM):
                aimCamera.SetActive(true);
                foreach (GameObject obj in aimObjects)
                    obj.SetActive(true  );
                break;
            default:
                Debug.LogError("Unkown CameraType " + _activeCam_);
                break;
        }
        // Set which camera is active.
        activeCam = _activeCam_;

        // Set the player's rotation type.
        thirdPersonMove.SetRotationType(_controlType_);
    }

    /// <summary>
    /// Helper function for ChangeCamera.
    /// Disables all the cameras.
    /// </summary>
    private void DisableAllCameras()
    {
        thirdPersonCamera.SetActive(false);
        aimCamera.SetActive(false);
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
