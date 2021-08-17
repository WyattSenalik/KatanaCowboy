using UnityEngine;
using UnityEngine.InputSystem;

namespace GameEventSystem.Extension
{
    /// <summary>
    /// Holds an input event created from the given ID.
    /// MonoBehvaiour to give to the PlayerInput unity events to call invoke.
    /// </summary>
    public class InputEvent : MonoBehaviour
    {
        // ID of the input event
        [SerializeField] private GameEventIdentifierScriptableObject inputEventID = null;
        // Wrapper to create the event
        private GameEvent<InputAction.CallbackContext> inputEvent = null;


        // Called 0th
        // Domestic initialization
        private void Awake()
        {
            // Create the event
            inputEvent = new GameEvent<InputAction.CallbackContext>(inputEventID);
        }


        /// <summary>
        /// Invokes the GameEvent.
        /// Called by PlayerInput's unity events.
        /// </summary>
        /// <param name="context"></param>
        public void Invoke(InputAction.CallbackContext context)
        {
            inputEvent.Invoke(context);
        }
    }
}