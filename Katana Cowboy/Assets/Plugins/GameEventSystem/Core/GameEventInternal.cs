using System;
using UnityEngine;

namespace GameEventSystem.Internal
{
    /// <summary>
    /// Custom Event to be invoked and subscribed to.
    /// </summary>
    public class GameEventInternal : IGameEvent
    {
        // The actual event action. Takes GameEventData as its parameter.
        private event Action<GameEventData> onEvent;


        /// <summary>
        /// Creates an event from a fake event which is holding the listeners.
        /// </summary>
        /// <param name="fakeEvent">Fake event holding listeners for the event.</param>
        public void CreateFromFakeEvent(FakeGameEvent fakeEvent)
        {
            Action<GameEventData> action = fakeEvent.GetCallback();
            while (action != null)
            {
                this.Subscribe(action);
                fakeEvent.Unsubscribe(action);

                action = fakeEvent.GetCallback();
            }
        }


        public void Invoke(GameEventData data)
        {
            onEvent?.Invoke(data);
        }
        public void Subscribe(Action<GameEventData> actionToAddToEvent)
        {
            onEvent += actionToAddToEvent;
        }
        public void Unsubscribe(Action<GameEventData> actionToRemoveFromEvent)
        {
            onEvent -= actionToRemoveFromEvent;
        }
        public bool IsRealEvent()
        {
            return true;
        }
    }
}