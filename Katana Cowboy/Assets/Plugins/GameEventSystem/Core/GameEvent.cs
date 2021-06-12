using System;

using GameEventSystem.Internal;

namespace GameEventSystem
{
    /// <summary>
    /// Custom Event to be invoked and subscribed to.
    /// </summary>
    public class GameEvent : IGameEvent
    {
        // The actual event action. Takes GameEventData as its parameter.
        public event Action<GameEventData> OnEvent;


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
            OnEvent?.Invoke(data);
        }
        public void Subscribe(Action<GameEventData> actionToAddToEvent)
        {
            OnEvent += actionToAddToEvent;
        }
        public void Unsubscribe(Action<GameEventData> actionToRemoveFromEvent)
        {
            OnEvent -= actionToRemoveFromEvent;
        }
        public bool IsRealEvent()
        {
            return true;
        }
    }
}