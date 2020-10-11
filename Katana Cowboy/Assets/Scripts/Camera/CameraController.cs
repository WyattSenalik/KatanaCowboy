using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject thirdPersonCamera = null;
    [SerializeField]
    private GameObject aimCamera = null;

    public void ActivateAimCamera()
    {
        thirdPersonCamera.SetActive(false);
        aimCamera.SetActive(true);
    }

    public void ActivateDefaultCamera()
    {
        thirdPersonCamera.SetActive(true);
        aimCamera.SetActive(false);
    }
}
