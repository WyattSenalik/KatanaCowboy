using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    // Customization.
    // Speed to zoom in and out.
    [SerializeField]
    private float zoomSpeed = 2f;
    // Max scale for being zoomed out.
    [SerializeField] [Min(0.1f)]
    private float maxZoom = 2f;
    // Min scale for being zoomed in.
    [SerializeField] [Min(0.1f)]
    private float minZoom = 0.5f;
    // Speed of smoothing for zooming.
    [SerializeField]
    private float zoomSmoothTime = 0.1f;

    // References.
    // Reference to Cimemachine Free Look.
    [SerializeField]
    private CinemachineFreeLook camFreeLook = null;

    // Starting orbit values.
    private CinemachineFreeLook.Orbit[] startingOrbits;
    // Minimum radii.
    private float[] minRadii = new float[3];
    // Maximum radii.
    private float[] maxRadii = new float[3];

    // Called 1st before the first frame.
    private void Start()
    {
        startingOrbits = new CinemachineFreeLook.Orbit[3];
        for (int i = 0; i < 3; ++i)
        {
            startingOrbits[i] = new CinemachineFreeLook.Orbit(camFreeLook.m_Orbits[i].m_Height, camFreeLook.m_Orbits[i].m_Radius);
            minRadii[i] = minZoom * startingOrbits[i].m_Radius;
            maxRadii[i] = maxZoom * startingOrbits[i].m_Radius;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Get zoom input.
        float zoom = Input.GetAxisRaw("Zoom") * 50f;

        if (zoom != 0f)
        {
            for (int i = 0; i < camFreeLook.m_Orbits.Length; ++i)
            {
                float newZoom = camFreeLook.m_Orbits[i].m_Radius - startingOrbits[i].m_Radius * zoom * zoomSpeed * Time.deltaTime;
                float smoothZoom = Mathf.Lerp(camFreeLook.m_Orbits[i].m_Radius, newZoom, zoomSmoothTime);

                float minRad = minZoom * startingOrbits[i].m_Radius;
                float maxRad = maxZoom * startingOrbits[i].m_Radius;
                camFreeLook.m_Orbits[i].m_Radius = Mathf.Clamp(smoothZoom, minRad, maxRad);
            }
        }
    }
}
