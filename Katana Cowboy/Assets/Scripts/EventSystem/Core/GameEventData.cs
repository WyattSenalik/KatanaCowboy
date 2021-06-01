using UnityEngine;
using System.Collections.Generic;

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
        protected readonly Dictionary<System.Type, object> eventParams = new Dictionary<System.Type, object>();


        /// <summary>
        /// Constructs GameEventData from the internal version of this class.
        /// </summary>
        /// <param name="internalEventData">Internal GameEventData to build this GameEventData from.</param>
        public GameEventData(GameEventDataInternal internalEventData)
        {
            Dictionary<System.Type, object>.Enumerator dictEnum = internalEventData.ParamEnumerator;
            while (dictEnum.MoveNext())
            {
                KeyValuePair<System.Type, object> pair = dictEnum.Current;
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
            Debug.LogError("No parameter of type " + typeof(T) + " has been specified.");
            return default;
        }
    }
}