using System;

namespace GameEventSystem
{
    /// <summary>
    /// Interface for a GameEvent to be used by the EventSystem.
    /// </summary>
    public interface IGameEvent
    {
        /// <summary>
        /// Invokes the GameEvent to call all its listeners.
        /// </summary>
        /// <param name="data">Parameter data for the GameEvent.</param>
        void Invoke(GameEventData data);
        /// <summary>
        /// Subscribes the given listener function to the event.
        /// </summary>
        /// <param name="actionToAddToEvent">Listener function to call when event is invoked.</param>
        void Subscribe(Action<GameEventData> actionToAddToEvent);
        /// <summary>
        /// Unsubscribes the given listener function from the event.
        /// </summary>
        /// <param name="actionToRemoveFromEvent">Listener function to no longer call when the event is invoked.</param>
        void Unsubscribe(Action<GameEventData> actionToRemoveFromEvent);
    }
}