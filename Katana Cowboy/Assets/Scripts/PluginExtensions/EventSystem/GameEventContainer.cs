using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEventSystem.Extension
{
    /// <summary>
    /// Container for multiple GameEventWrappers
    /// </summary>
    public class GameEventContainer
    {
        // Dictionary to hold the event wrappers by their event name/ID
        private Dictionary<string, GameEvent> _eventMap = null;


        /// <summary>
        /// Creates a GameEventContainer that holds events with the given names/IDs.
        /// </summary>
        /// <param name="eventNames">Names/IDs of the events to create.</param>
        public GameEventContainer(params string[] eventNames)
        {
            _eventMap = new Dictionary<string, GameEvent>(eventNames.Length);

            foreach (string name in eventNames)
            {
                GameEvent wrapper = new GameEvent(name);
                _eventMap.Add(name, wrapper);
            }
        }


        #region Invoke
        /// <summary>
        /// Helper function for the other Invoke functions.
        /// </summary>
        /// <param name="eventName">Name/ID of the event to invoke.</param>
        /// <param name="invokeAction">Action used to invoke the event.</param>
        /// <returns>True if an event with the given name/ID is in this container. False if no such event exists.</returns>
        private bool Invoke(string eventName, Action<GameEvent> invokeAction)
        {
            if (_eventMap.ContainsKey(eventName))
            {
                GameEvent wrapper = _eventMap[eventName];
                invokeAction?.Invoke(wrapper);

                return true;
            }

            Debug.LogError($"Could not find event with name {eventName}");
            return false;
        }
        /// <summary>
        /// Invokes the event with the given name/ID with no parameters.
        /// </summary>
        /// <param name="eventName">Name of the event to invoke.</param>
        /// <returns>True if an event with the given name/ID is in this container. False if no such event exists.</returns>
        public bool Invoke(string eventName)
        {
            return Invoke(eventName, (GameEvent wrapper) => wrapper.Invoke());
        }
        /// <summary>
        /// Invokes the event with the given name/ID with 1 parameter.
        /// </summary>
        /// <param name="eventName">Name of the event to invoke.</param>
        /// <returns>True if an event with the given name/ID is in this container. False if no such event exists.</returns>
        public bool Invoke<T>(string eventName, T param0)
        {
            return Invoke(eventName, (GameEvent wrapper) => wrapper.Invoke(param0));
        }
        /// <summary>
        /// Invokes the event with the given name/ID with 2 parameters.
        /// </summary>
        /// <param name="eventName">Name of the event to invoke.</param>
        /// <returns>True if an event with the given name/ID is in this container. False if no such event exists.</returns>
        public bool Invoke<T, G>(string eventName, T param0, G param1)
        {
            return Invoke(eventName, (GameEvent wrapper) => wrapper.Invoke(param0, param1));
        }
        /// <summary>
        /// Invokes the event with the given name/ID with 3 parameters.
        /// </summary>
        /// <param name="eventName">Name of the event to invoke.</param>
        /// <returns>True if an event with the given name/ID is in this container. False if no such event exists.</returns>
        public bool Invoke<T, G, H>(string eventName, T param0, G param1, H param2)
        {
            return Invoke(eventName, (GameEvent wrapper) => wrapper.Invoke(param0, param1, param2));
        }
        /// <summary>
        /// Invokes the event with the given name/ID with 4 parameters.
        /// </summary>
        /// <param name="eventName">Name of the event to invoke.</param>
        /// <returns>True if an event with the given name/ID is in this container. False if no such event exists.</returns>
        public bool Invoke<T, G, H, J>(string eventName, T param0, G param1, H param2, J param3)
        {
            return Invoke(eventName, (GameEvent wrapper) => wrapper.Invoke(param0, param1, param2, param3));
        }
        /// <summary>
        /// Invokes the event with the given name/ID with 5 parameters.
        /// </summary>
        /// <param name="eventName">Name of the event to invoke.</param>
        /// <returns>True if an event with the given name/ID is in this container. False if no such event exists.</returns>
        public bool Invoke<T, G, H, J, K>(string eventName, T param0, G param1, H param2,
            J param3, K param4)
        {
            return Invoke(eventName, (GameEvent wrapper) => wrapper.Invoke(param0, param1, param2, param3, param4));
        }
        /// <summary>
        /// Invokes the event with the given name/ID with 6 parameters.
        /// </summary>
        /// <param name="eventName">Name of the event to invoke.</param>
        /// <returns>True if an event with the given name/ID is in this container. False if no such event exists.</returns>
        public bool Invoke<T, G, H, J, K, L>(string eventName, T param0, G param1, H param2,
            J param3, K param4, L param5)
        {
            return Invoke(eventName, (GameEvent wrapper) => wrapper.Invoke(param0, param1, param2, param3, param4, param5));
        }
        /// <summary>
        /// Invokes the event with the given name/ID with 7 parameters.
        /// </summary>
        /// <param name="eventName">Name of the event to invoke.</param>
        /// <returns>True if an event with the given name/ID is in this container. False if no such event exists.</returns>
        public bool Invoke<T, G, H, J, K, L, I>(string eventName, T param0, G param1, H param2,
            J param3, K param4, L param5, I param6)
        {
            return Invoke(eventName, (GameEvent wrapper) => wrapper.Invoke(param0, param1, param2, param3, param4, param5, param6));
        }
        #endregion Invoke
    }
}