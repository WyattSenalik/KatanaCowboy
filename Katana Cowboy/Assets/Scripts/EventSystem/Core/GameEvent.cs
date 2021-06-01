using System;

namespace GameEventSystem
{
    /// <summary>
    /// Custom Event to be invoked and subscribed to.
    /// </summary>
    public class GameEvent : IGameEvent
    {
        // The actual event action. Takes GameEventData as its parameter.
        public event Action<GameEventData> OnEvent;


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
    }
}