using UnityEngine;
using UnityEngine.InputSystem;

namespace GameEventSystem
{
    /// <summary>
    /// Represents an input event.
    /// Used by the unity input system to invoke an event that is hooked up to the custom input system.
    /// </summary>
    [CreateAssetMenu(fileName = "New InputGameEvent", menuName = "ScriptableObjects/EventSystem/InputGameEvent")]
    public class InputGameEvent : ScriptableObject
    {
        // The event wrapper that serves as the input event
        [SerializeField] private GameEventWrapper inputEvent = null;
        public GameEventWrapper InputEvent { get { return inputEvent; } set { inputEvent = value; } }

        /// <summary>
        /// Invokes the input event.
        /// Called by the unity input system.
        /// </summary>
        /// <param name="context"></param>
        public void InvokeEvent(InputAction.CallbackContext context)
        {
            inputEvent.Invoke(context);
        }
    }
}