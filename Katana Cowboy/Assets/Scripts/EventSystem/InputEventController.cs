using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameEventSystem
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputEventController : MonoBehaviour
    {
        private PlayerInput playerInput = null;

        [SerializeField] private List<EventIDWithLabel> labeledEvents = new List<EventIDWithLabel>();
        private List<InputGameEvent> inputGameEvents = new List<InputGameEvent>();


        private void Start()
        {
            UpdateInputEvents();
            CreateGameEvents();
        }


        public void UpdateInputEvents()
        {
            playerInput = GetComponent<PlayerInput>();
            playerInput.notificationBehavior = PlayerNotifications.InvokeUnityEvents;
            if (HasEventListChanged())
            {
                UpdateEventList();
            }
        }


        private bool HasEventListChanged()
        {
            if (playerInput.actionEvents.Count != labeledEvents.Count)
            {
                return true;
            }

            for (int i = 0; i < labeledEvents.Count; ++i)
            {
                if (playerInput.actionEvents[i].actionName != labeledEvents[i].Label)
                {
                    return true;
                }
            }

            return false;
        }
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

    [System.Serializable]
    internal class EventIDWithLabel
    {
        public string Label => label;
        [HideInInspector] [SerializeField] private string label = "";
        public GameEventIdentifier ID => identifier;
        [SerializeField] private GameEventIdentifier identifier = null;


        public EventIDWithLabel(string eventLabel)
        {
            label = eventLabel;
        }
    }

    internal class InputGameEvent
    {
        private GameEventWrapper eventWrapper = null;


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


        private void Invoke(InputAction.CallbackContext context)
        {
            eventWrapper.Invoke(context);
        }
    }
}