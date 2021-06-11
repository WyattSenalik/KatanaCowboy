using System;
using System.Collections.Generic;
using UnityEngine;

using GameEventSystem.Internal;

namespace GameEventSystem
{
    /// <summary>
    /// Fake event to help avoid race conditions when subscribing to and creating events.
    /// Temporarily holds the callbacks for the event.
    /// </summary>
    public class FakeGameEvent : IGameEvent
    {
        // Callbacks for the real event when its created
        private List<Action<GameEventData>> callbacks = new List<Action<GameEventData>>();

        
        /// <summary>
        /// Gets the last callback in the list of callbacks.
        /// </summary>
        /// <returns></returns>
        public Action<GameEventData> GetCallback()
        {
            if (callbacks.Count > 0)
            {
                return callbacks[callbacks.Count - 1];
            }
            return null;
        }


        public void Invoke(GameEventData data)
        {
            Debug.Log("What the fudge are you doing. You can't Invoke a FakeEvent");
        }
        public void Subscribe(Action<GameEventData> actionToAddToEvent)
        {
            callbacks.Add(actionToAddToEvent);
        }
        public void Unsubscribe(Action<GameEventData> actionToRemoveFromEvent)
        {
            callbacks.Remove(actionToRemoveFromEvent);
        }
        public bool IsRealEvent()
        {
            return false;
        }
    }
}