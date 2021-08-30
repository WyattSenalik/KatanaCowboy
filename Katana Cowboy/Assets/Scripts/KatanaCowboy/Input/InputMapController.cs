using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles swapping of the input map to handle when some scripts swap and then want to swap back to what it previously was.
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class InputMapController : SingletonMonoBehav<InputMapController>
{
    // Name of the default map
    [SerializeField] private string defaultMapName = "ThirdPersonMovement";

    // Reference to the player input
    private PlayerInput playerInputRef = null;

    // Stack of active input maps.
    private List<string> activeMapNames = new List<string>();


    // Called 0th
    // Domestic Initialization
    protected override void Awake()
    {
        base.Awake();

        playerInputRef = GetComponent<PlayerInput>();
    }
    // Called 1st
    // Foreign Initialization
    private void Start()
    {
        ResetInputMap();
    }


    /// <summary>Adds the given input map to the stack and swaps to it.</summary>
    /// <param name="inputMapName">Name of the input map to add to the stack.</param>
    public void SwitchInputMap(string inputMapName)
    {
        activeMapNames.Add(inputMapName);
        UpdateActiveInputMap();
    }

    /// <summary>Pops the input map with the given name off the stack and updates the input map to the one before it.</summary>
    /// <param name="inputMapName">Name of the input map to remove from the stack.</param>
    public void PopInputMap(string inputMapName)
    {
        activeMapNames.Remove(inputMapName);
        UpdateActiveInputMap();
    }

    /// <summary>Resets the input map to just be the starting input map.</summary>
    public void ResetInputMap()
    {
        // Initialize active map names to have the default map in it.
        activeMapNames = new List<string>();
        activeMapNames.Add(defaultMapName);
        UpdateActiveInputMap();
    }

    /// <summary>Updates the action map to be the map at the top of the stack.</summary>
    private void UpdateActiveInputMap()
    {
        if (activeMapNames.Count > 0)
        {
            playerInputRef.SwitchCurrentActionMap(activeMapNames[activeMapNames.Count - 1]);
        }
        else
        {
            Debug.LogError("Last input map was removed from the stack");
        }
    }
}
