﻿using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

using Cinemachine;
using GameEventSystem;

public class CameraZoom : MonoBehaviour
{
    // References
    // Reference to Cimemachine Free Look.
    [SerializeField] private CinemachineFreeLook camFreeLook = null;

    // Customization.
    // Speed to zoom in and out.
    [SerializeField] private float zoomSpeed = 2.0f;
    // Max scale for being zoomed out.
    [SerializeField] [Min(0.1f)] private float maxZoom = 2f;
    // Min scale for being zoomed in.
    [SerializeField] [Min(0.1f)] private float minZoom = 0.5f;
    // Speed of smoothing for zooming.
    [SerializeField] [Range(0.0f, 1.0f)] private float zoomSmoothing = 0.1f;

    // Starting orbit values.
    private CinemachineFreeLook.Orbit[] startingOrbits;
    // Minimum radii.
    private float[] minRadii = new float[3];
    // Maximum radii.
    private float[] maxRadii = new float[3];

    private float curZoom = 0.0f;
    private float targetZoom = 0.0f;

    private bool isZoomCoroutineActive = false;

    private bool isSubscribed = false;


    // Functions called by unity messages (ex: Start, Awake, Update, etc.)
    #region UnityEvents
    // Called 1st.
    // Foreign Initialization
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
    // Called when this component is enabled
    private void OnEnable()
    {
        // Subscribe to events
        ToggleSubscriptionToEvents(true);
    }
    // Called when this component is disabled
    private void OnDisable()
    {
        // Unsubscribe from events
        ToggleSubscriptionToEvents(false);
    }
    #endregion UnityEvents


    // Functions that subscribe and unsubscribe from the events this script listens to
    #region EventSubscriptions
    /// <summary>
    /// Subscribes or unsubscribes this script to listen to GameEventSystem events.
    /// </summary>
    /// <param name="condition">Subscribe or unsubscribe.</param>
    private void ToggleSubscriptionToEvents(bool condition)
    {
        if (isSubscribed != condition)
        {
            // Input events
            EventSystem.ToggleSubscriptionToEvent(condition, InputEventIDList.Zoom, OnZoom);

            isSubscribed = condition;
        }
    }
    #endregion EventSubscriptions


    // Functions called by the event system
    #region EventCallbacks
    /// <summary>
    /// Calls zoom to zoom in/out the free look camera.
    /// Called by the zoom input event.
    /// </summary>
    private void OnZoom(InputAction.CallbackContext context)
    {
        targetZoom = context.ReadValue<float>();

        StartZoomCoroutine(targetZoom);
        //Zoom(zoom);
    }
    #endregion EventCallbacks


    private void StartZoomCoroutine(float zoom)
    {
        targetZoom = zoom;
        if (!isZoomCoroutineActive)
        {
            StartCoroutine(ZoomCoroutine());
        }
    }

    private IEnumerator ZoomCoroutine()
    {
        isZoomCoroutineActive = true;

        while (curZoom - targetZoom > 0.1f)
        {
            curZoom = Mathf.Lerp(curZoom, targetZoom, zoomSmoothing);
            Zoom(curZoom);
            yield return null;
        }
        Zoom(targetZoom);

        isZoomCoroutineActive = false;
        yield return null;
    }

    /// <summary>
    /// Zooms the free look camera in or out by changing its orbits.
    /// </summary>
    /// <param name="zoom">Amount to zoom the camera in/out.</param>
    private void Zoom(float zoom)
    {
        for (int i = 0; i < camFreeLook.m_Orbits.Length; ++i)
        {
            float newZoom = camFreeLook.m_Orbits[i].m_Radius - startingOrbits[i].m_Radius * zoom * zoomSpeed * Time.deltaTime;

            float minRad = minZoom * startingOrbits[i].m_Radius;
            float maxRad = maxZoom * startingOrbits[i].m_Radius;
            camFreeLook.m_Orbits[i].m_Radius = Mathf.Clamp(newZoom, minRad, maxRad);
        }
    }
}