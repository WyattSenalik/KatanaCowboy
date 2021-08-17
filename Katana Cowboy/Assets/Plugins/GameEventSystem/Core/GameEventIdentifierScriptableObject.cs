﻿using System;
using UnityEngine;

using GameEventSystem.Internal;

namespace GameEventSystem
{
    /// <summary>
    /// ID for a GameEvent thats data is the name of the created scriptable object.
    /// </summary>
    [CreateAssetMenu(fileName = "New GameEventID", menuName = "ScriptableObjects/EventSystem/GameEventIdentifier")]
    public class GameEventIdentifierScriptableObject : ScriptableObject, IGameEventIdentifier
    {
        public string GetID() => GetEventName();

        /// <summary>
        /// Subscribes the action to the event identified by this ID.
        /// </summary>
        /// <param name="action">Action to call when the event is invoked.</param>
        public void Subscribe(Action<GameEventData> action)
        {
            EventSystem.SubscribeToEvent(this, action);
        }
        /// <summary>
        /// Unsubscribes the action from the event identified by this ID.
        /// </summary>
        /// <param name="action">Action to no longer associate with the event.</param>
        public void Unsubscribe(Action<GameEventData> action)
        {
            EventSystem.UnsubscribeFromEvent(this, action);
        }


        /// <summary>
        /// Gets just the name portion of the scriptable object's name which contains the
        /// events name and parameters.
        private string GetEventName()
        {
            string fullName = this.name;
            int nameEndIndex = fullName.IndexOf('$');
            return fullName.Substring(0, nameEndIndex);
        }
    }
}