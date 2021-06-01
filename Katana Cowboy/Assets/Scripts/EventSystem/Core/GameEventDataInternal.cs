using System.Collections.Generic;
using UnityEngine;

namespace GameEventSystem
{
    /// <summary>
    /// Internal version of the GameEventData class.
    /// Used by the GameEventWrapper to help create GameEvents easier.
    /// </summary>
    public class GameEventDataInternal : GameEventData
    {
        /// <summary>
        /// Iterator of the parameter dictionary.
        /// </summary>
        public Dictionary<System.Type, object>.Enumerator ParamEnumerator => eventParams.GetEnumerator();


        /// <summary>
        /// Default GameEventDataInternal Constructor.
        /// Does nothing.
        /// </summary>
        public GameEventDataInternal() : base() { }
        /// <summary>
        /// Constructs GameEventDataInternal with the given parameters.
        /// Multiple parameters cannot be of the same type.
        /// </summary>
        /// <param name="parameters">Parameters to use for this GameEventData.</param>
        public GameEventDataInternal(object[] parameters)
        {
            foreach (object param in parameters)
            {
                AddValue(param);
            }
        }


        /// <summary>
        /// Adds the given value to the parameters.
        /// Cannot be of the a type that is already in the parameters.
        /// </summary>
        /// <typeparam name="T">Type of the parameter.</typeparam>
        /// <param name="addVal">Parameter to add.</param>
        public void AddValue<T>(T addVal)
        {
            if (eventParams.ContainsKey(addVal.GetType()))
            {
                Debug.LogError("Tried to add parameters of type " + addVal.GetType() + ". " +
                        "GameEventData cannot have multiple parameters of the same type. Consider " +
                        "using a wrapper class to house the parameters instead.");
            }
            else
            {
                eventParams.Add(addVal.GetType(), addVal);
            }
        }
        /// <summary>
        /// Adds the given value to the parameters.
        /// If the type of the value already exists among the parameters, replaces the parameter with the new value.
        /// </summary>
        /// <typeparam name="T">Type of the parameter.</typeparam>
        /// <param name="newValue">Parameter to add or replace if already existing.</param>
        public void AddOrReplaceValue<T>(T newValue)
        {
            if (eventParams.ContainsKey(newValue.GetType()))
            {
                eventParams[newValue.GetType()] = newValue;
            }
            else
            {
                eventParams.Add(newValue.GetType(), newValue);
            }
        }
    }
}