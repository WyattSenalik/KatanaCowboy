﻿using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Camera types.
    public enum CameraTypes { Standard, Aim };


    // References to the different cameras.
    // Standard third person camera.
    [SerializeField] private GameObject thirdPersonCamera = null;
    // Camera for aiming in third person
    [SerializeField] private GameObject aimCamera = null;

    // References to ui that needs to be turned on per camera.
    // UI and objects that should be active when swapping to standard.
    [SerializeField] private GameObject[] standardObjects = new GameObject[0];
    // UI and objects that should be active when swapping to aim.
    [SerializeField] private GameObject[] aimObjects = new GameObject[0];

    // Current camera that is active.
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
                thirdPersonCamera.SetActive(true);
                foreach (GameObject obj in standardObjects)
                    obj.SetActive(true);
                break;
            case (CameraTypes.Aim):
                aimCamera.SetActive(true);
                foreach (GameObject obj in aimObjects)
                    obj.SetActive(true);
                break;
            default:
                Debug.LogError("Unkown CameraType " + activeCamType);
                break;
        }
        // Set which camera is active.
        activeCam = activeCamType;
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
