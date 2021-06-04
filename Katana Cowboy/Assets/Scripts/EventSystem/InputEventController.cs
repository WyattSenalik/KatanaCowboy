using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameEventSystem
{
    /// <summary>
    /// Creates events for each of the player input events.
    /// Takes control of the player input to help the player attach event IDs to the
    /// input events.
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class InputEventController : MonoBehaviour
    {
        // Reference to the player input
        private PlayerInput playerInput = null;

        // List of IDs visible in the editor named with the player input event names
        // The ID given will be the ID for the game event that will be created from the input event
        [SerializeField] private List<EventIDWithLabel> labeledEvents = new List<EventIDWithLabel>();
        // List to hold the input game events in memory
        private List<InputGameEvent> inputGameEvents = new List<InputGameEvent>();


        // Called 1st
        // Foreign Initialization
        private void Start()
        {
            UpdateInputEvents();
            CreateGameEvents();
        }


        /// <summary>
        /// Update the input events editor list.
        /// </summary>
        public void UpdateInputEvents()
        {
            playerInput = GetComponent<PlayerInput>();
            playerInput.notificationBehavior = PlayerNotifications.InvokeUnityEvents;
            if (HasEventListChanged())
            {
                UpdateEventList();
            }
        }


        /// <summary>
        /// Checks if the input event list has changed and if we need to update the editor list.
        /// </summary>
        /// <returns>True if the input event list has changed since last update.</returns>
        private bool HasEventListChanged()
        {
            // Obviously has changed if the size is different
            if (playerInput.actionEvents.Count != labeledEvents.Count)
            {
                return true;
            }
            // Compare each of the names. If they are not the same, its changed
            for (int i = 0; i < labeledEvents.Count; ++i)
            {
                if (playerInput.actionEvents[i].actionName != labeledEvents[i].Label)
                {
                    return true;
                }
            }

            // No change if we reached this point
            return false;
        }
        /// <summary>
        /// Removes any old input events and adds any new input events.
        /// </summary>
        private void UpdateEventList()
        {
            List<string> alreadyFoundLabels = new List<string>();
            // Remove old events that don't exist
            for (int i = 0; i < labeledEvents.Count; ++i)
            {
                bool exists = false;
                for (int k = 0; k < playerInput.actionEvents.Count; ++k)
                {
                    string actionName = ShortenInputActionName(playerInput.actionEvents[k].actionName);
                    if (labeledEvents[i].Label == actionName)
                    {
                        exists = true;
                    }
                }
                if (!exists || alreadyFoundLabels.Contains(labeledEvents[i].Label))
                {
                    labeledEvents.RemoveAt(i);
                }
                else
                {
                    alreadyFoundLabels.Add(labeledEvents[i].Label);
                }
            }
            // Add new events that don't exist yet
            for (int i = 0; i < playerInput.actionEvents.Count; ++i)
            {
                bool exists = false;
                string actionName = ShortenInputActionName(playerInput.actionEvents[i].actionName);
                for (int k = 0; k < labeledEvents.Count; ++k)
                {
                    if (actionName == labeledEvents[k].Label)
                    {
                        exists = true;
                    }
                }
                if (!exists)
                {
                    labeledEvents.Add(new EventIDWithLabel(actionName));
                }
            }
        }
        /// <summary>
        /// Shortens the name of the input action ot not have the key binding info.
        /// </summary>
        /// <param name="rawActionName">Full name of the input action.</param>
        /// <returns>Shorten name of the input action.</returns>
        private string ShortenInputActionName(string rawActionName)
        {
            int endIndex = rawActionName.IndexOf('[');
            if (endIndex == -1)
            {
                return rawActionName;
            }
            else
            {
                return rawActionName.Substring(0, endIndex);
            }
        }
        /// <summary>
        /// Creates game events for all the input events.
        /// </summary>
        private void CreateGameEvents()
        {
            inputGameEvents = new List<InputGameEvent>();
            foreach (EventIDWithLabel eventWithLabel in labeledEvents)
            {
                GameEventIdentifier id = eventWithLabel.ID;
                PlayerInput.ActionEvent actionEvent = GetInputEventWithLabel(eventWithLabel.Label);
                InputGameEvent inputEvent = new InputGameEvent(id, actionEvent);
                inputGameEvents.Add(inputEvent);
            }
        }
        /// <summary>
        /// Finds and returns the input action with the given label.
        /// </summary>
        /// <param name="label">Label of the desired input action event.</param>
        /// <returns>Input Action Event that can be subscribed to.</returns>
        private PlayerInput.ActionEvent GetInputEventWithLabel(string label)
        {
            for (int i = 0; i < playerInput.actionEvents.Count; ++i)
            {
                string actionLabel = ShortenInputActionName(playerInput.actionEvents[i].actionName);
                if (actionLabel == label)
                {
                    return playerInput.actionEvents[i];
                }
            }
            Debug.LogError("Could not find event with label " + label);
            return null;
        }
    }

    /// <summary>
    /// Class that ties a label of an input action event to a game event id.
    /// Used in the serialized editor list.
    /// </summary>
    [System.Serializable]
    internal class EventIDWithLabel
    {
        // Label of an input action event
        public string Label => label;
        [HideInInspector] [SerializeField] private string label = "";
        // Identifier to link ot the input action event
        public GameEventIdentifier ID => identifier;
        [SerializeField] private GameEventIdentifier identifier = null;


        public EventIDWithLabel(string eventLabel)
        {
            label = eventLabel;
        }
    }

    /// <summary>
    /// InputGameEvent to attach a GameEvent to an InputActionEvent.
    /// </summary>
    internal class InputGameEvent
    {
        // Game event that will be called by the InputActionEvent
        private GameEventWrapper eventWrapper = null;


        /// <summary>
        /// Creates an InputGameEvent, adds the game event as a listener, and then creates the
        /// game event in the event system.
        /// </summary>
        /// <param name="id">ID to use to create the game event.</param>
        /// <param name="actionEvent">InputActionEvent to listen to.</param>
        public InputGameEvent(GameEventIdentifier id, PlayerInput.ActionEvent actionEvent)
        {
            if (id == null)
            {
                return;
            }
            eventWrapper = new GameEventWrapper(id);
            actionEvent.AddListener(Invoke);
            eventWrapper.CreateEvent();
        }


        /// <summary>
        /// Invokes the game event.
        /// Added as a listener to the input action event.
        /// </summary>
        /// <param name="context"></param>
        private void Invoke(InputAction.CallbackContext context)
        {
            eventWrapper.Invoke(context);
        }
    }
}