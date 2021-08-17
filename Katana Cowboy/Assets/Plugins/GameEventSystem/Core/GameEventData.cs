using System;
using System.Collections.Generic;
using UnityEngine;

using GameEventSystem.Internal;

namespace GameEventSystem
{
    /// <summary>
    /// Data class to be used by GameEvent as the parameter.
    /// Holds other parameters that can be accessed by their type.
    /// Cannot have multiple parameters of the same type.
    /// </summary>
    public class GameEventData
    {
        // Parameters referenced by the type. This does not allow for multiple parameters of the same type.
        protected readonly Dictionary<Type, object> eventParams = new Dictionary<Type, object>();


        /// <summary>
        /// Constructs GameEventData from the internal version of this class.
        /// </summary>
        /// <param name="internalEventData">Internal GameEventData to build this GameEventData from.</param>
        public GameEventData(GameEventDataInternal internalEventData)
        {
            foreach (KeyValuePair<Type, object> pair in internalEventData.GetParameters())
            {
                eventParams.Add(pair.Key, pair.Value);
            }
        }
        /// <summary>
        /// Default GameEventData Constructor.
        /// Does nothing.
        /// </summary>
        protected GameEventData() { }


        /// <summary>
        /// Gets a parameter of the given data type from the data.
        /// </summary>
        /// <typeparam name="T">Type of parameter ot get.</typeparam>
        /// <returns>Returns the paramter of the given type if found. Returns default otherwise.</returns>
        public T ReadValue<T>()
        {
            if (eventParams.ContainsKey(typeof(T)))
            {
                return (T)eventParams[typeof(T)];
            }
            string paramNames = "";
            foreach (Type type in eventParams.Keys)
            {
                paramNames += type + "; ";
            }
            Debug.LogError("No parameter of type " + typeof(T) + " has been specified. " + "\n" +
                "The specified paramater types are: " + paramNames);
            return default;
        }
    }
}