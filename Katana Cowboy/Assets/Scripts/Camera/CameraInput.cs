using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

// NO LONGER IN USE.
public class CameraInput : MonoBehaviour, AxisState.IInputAxisProvider
{
    // If the player is allowed to 
    private bool acceptInput = true;

    // Called 1st before the 1st frame.
    private void Start()
    {
        acceptInput = true;
    }

    /// <summary>
    /// Override from AxisState.IInputAxisProvider.
    /// Used by Cinemachine cameras for input.
    /// </summary>
    /// <param name="axis">Which axis to query: 0, 1, or 2. These represent, respectively, the X, Y, and Z axes.</param>
    /// <returns>Get the value of the input axis</returns>
    public float GetAxisValue(int axis)
    {
        // If this isn't accepting input, don't move the camera.
        if (!acceptInput)
            return 0;

        switch (axis)
        {
            // X axis
            case (0):
                return Input.GetAxisRaw("Mouse X");
            // Y axis
            case (1):
                return Input.GetAxisRaw("Mouse Y");
            // Never rotate the camera on the z axis, are you a monster?
            case (2):
                return 0;
            // Should never be called, since there are only 3 axes.
            default:
                Debug.LogError("Unexpected axis " + axis + " in Camera_Test");
                return 0;
        }
    }

    /// <summary>
    /// Allow the player to influence camera by setting acceptInput to true.
    /// </summary>
    public void AcceptInput()
    {
        SetAcceptInput(true);
    }
    /// <summary>
    /// Deny the player from influencing camera by setting acceptInput to false.
    /// </summary>
    public void DenyInput()
    {
        SetAcceptInput(false);
    }
    /// <summary>
    /// Sets the accept input variable to the given value.
    /// </summary>
    /// <param name="_val_">Value to set acceptInput to.</param>
    private void SetAcceptInput(bool _val_)
    {
        acceptInput = _val_;
    }
}
