using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameEventSystem
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputEventController : MonoBehaviour
    {
        [SerializeField] private InputEventWithLabel[] inputEventIDs = new InputEventWithLabel[0];

        private GameEventWrapper[] eventWrappers = new GameEventWrapper[0];


        private void Awake()
        {
            PlayerInput playerInput = GetComponent<PlayerInput>();
            eventWrappers = new GameEventWrapper[inputEventIDs.Length];
            for (int i = 0; i < eventWrappers.Length; ++i)
            {
                InputEventWithLabel inputEvent = inputEventIDs[i];
                GameEventWrapper wrapper = new GameEventWrapper(inputEvent.EventID);
                eventWrappers[i] = wrapper;
                eventWrappers[i].CreateEvent();

                playerInput.actionEvents[i].AddListener((InputAction.CallbackContext context) => { wrapper.Invoke(context); });
            }
        }


        public int GetInputEventsAmount()
        {
            return inputEventIDs.Length;
        }
        public void SetInputEventsAmount(int newLength, string[] labels)
        {
            int oldLength = inputEventIDs.Length;
            InputEventWithLabel[] oldInputEvents = new InputEventWithLabel[oldLength];
            for (int i = 0; i < oldLength; ++i)
            {
                oldInputEvents[i] = new InputEventWithLabel(inputEventIDs[i]);
            }

            inputEventIDs = new InputEventWithLabel[newLength];
            for (int i = 0; i < newLength; ++i)
            {
                inputEventIDs[i] = new InputEventWithLabel(labels[i]);
            }

            for (int newIndex = 0; newIndex < newLength; ++newIndex)
            {
                string newLabel = inputEventIDs[newIndex].Label;
                for (int oldIndex = 0; oldIndex < oldLength; ++oldIndex)
                {
                    string oldLabel = oldInputEvents[oldIndex].Label;
                    if (oldLabel.Equals(newLabel))
                    {
                        inputEventIDs[newIndex].EventID = oldInputEvents[oldIndex].EventID;
                        break;
                    }
                }
            }
        }
    }

    [System.Serializable]
    internal class InputEventWithLabel
    {
        [SerializeField] private GameEventIdentifier eventID = null;
        public GameEventIdentifier EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }

        private string eventLabel = "";
        public string Label => eventLabel;


        public InputEventWithLabel(string label)
        {
            eventID = null;
            eventLabel = label;
        }
        public InputEventWithLabel(InputEventWithLabel objToCopy)
        {
            eventID = objToCopy.eventID;
            eventLabel = objToCopy.eventLabel;
        }
    }
}