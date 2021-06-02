using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEventSystem
{
    public class FakeGameEvent : IGameEvent
    {
        private List<Action<GameEventData>> callbacks = new List<Action<GameEventData>>();

        
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