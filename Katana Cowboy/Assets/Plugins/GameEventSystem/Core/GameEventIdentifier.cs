using System;
using UnityEngine;

namespace GameEventSystem
{
    /// <summary>
    /// ID for a GameEvent thats data is the name of the created scriptable object.
    /// </summary>
    [CreateAssetMenu(fileName = "New GameEventID", menuName = "ScriptableObjects/EventSystem/GameEventIdentifier")]
    public class GameEventIdentifier : ScriptableObject
    {
        /// <summary>
        /// Gets the ID (asset name) for the GameEventID.
        /// </summary>
        /// <returns>string ID for the GameEvent.</returns>
        public string GetID()
        {
            return this.name;
        }

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

        public override string ToString()
        {
            return GetID();
        }
    }
}