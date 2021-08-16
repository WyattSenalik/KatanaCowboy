using System;
using System.Collections.Generic;
using UnityEngine;

using GameEventSystem.Internal;

namespace GameEventSystem
{
    /// <summary>
    /// Static class to create and hold all the custom events.
    /// </summary>
    public static class EventSystem
    {
        // References to all the events by their ID.
        private static Dictionary<string, IGameEvent> eventsHash = new Dictionary<string, IGameEvent>();


        /// <summary>
        /// Puts a new event into the EventSystem.
        /// Cannot add multiple events with the same eventID.
        /// </summary>
        /// <param name="eventID">String hash for the event that will be used to subscribe to it.</param>
        /// <param name="eventToCall">Reference to the event.</param>
        public static void CreateEvent(string eventID, GameEventInternal eventToCall)
        {
            // If the event does not exist yet or if it does exist, but is now null, create it
            if (!eventsHash.ContainsKey(eventID) || eventsHash[eventID] == null)
            {
                eventsHash.Add(eventID, eventToCall);
            }
            else
            {
                if (eventsHash[eventID].IsRealEvent())
                {
                    Debug.LogError("Event with ID " + eventID + " already exists");
                }
                // If the event is a fake event, then create a real event from the fake event.
                else
                {
                    //Debug.LogWarning("Creating event " + eventID + " from fake event");
                    FakeGameEvent fakeEvent = eventsHash[eventID] as FakeGameEvent;
                    eventToCall.CreateFromFakeEvent(fakeEvent);
                    eventsHash.Remove(eventID);
                    eventsHash.Add(eventID, eventToCall);
                }
            }

        }
        /// <summary>
        /// Puts a new event into the EventSystem.
        /// Cannot add multiple events with the same eventID.
        /// </summary>
        /// <param name="eventID">Identifier for the event that will be used to subscribe to it.</param>
        /// <param name="eventToCall">Reference to the event.</param>
        public static void CreateEvent(GameEventIdentifier eventID, GameEventInternal eventToCall)
        {
            CreateEvent(eventID.GetID(), eventToCall);
        }

        
        /// <summary>
        /// Subscribes the action to the event with the given identifier.
        /// </summary>
        /// <param name="eventID">String hash for the event that the action will be subscribed to.</param>
        /// <param name="action">Action to be called by the event when invoked.</param>
        /// <returns>True if event is found. False otherwise.</returns>
        public static bool SubscribeToEvent(string eventID, Action<GameEventData> action)
        {
            if (eventsHash.ContainsKey(eventID))
            {
                eventsHash[eventID].Subscribe(action);
                return true;
            }
            // If the event does not exist yet, create a fake event temporarily
            else
            {
                FakeGameEvent fakeEvent = new FakeGameEvent();
                fakeEvent.Subscribe(action);
                eventsHash.Add(eventID, fakeEvent);

                //Debug.LogWarning("No event of name " + eventID + " exists. FakeEvent was created.");
                return false;
            }
        }
        /// <summary>
        /// Subscribes the action to the event with the given identifier.
        /// </summary>
        /// <param name="eventID">Identifier for the event that the action will be subscribed to.</param>
        /// <param name="action">Action to be called by the event when invoked.</param>
        /// <returns>True if event is found. False otherwise.</returns>
        public static bool SubscribeToEvent(GameEventIdentifier eventID, Action<GameEventData> action)
        {
            return SubscribeToEvent(eventID.GetID(), action);
        }

        /// <summary>
        /// Unsubscribes the action from the event with the given identifier.
        /// </summary>
        /// <param name="eventID">String hash for the event that the action will be unsubscribed from.</param>
        /// <param name="action">Action to not longer be associated with the event.</param>
        /// <returns>True if event is found. False otherwise.</returns>
        public static bool UnsubscribeFromEvent(string eventID, Action<GameEventData> action)
        {
            if (eventsHash.ContainsKey(eventID))
            {
                eventsHash[eventID].Unsubscribe(action);
                return true;
            }
            else
            {
                Debug.LogError("No event of name " + eventID + " exists");
                return false;
            }
        }
        /// <summary>
        /// Unsubscribes the action from the event with the given identifier.
        /// </summary>
        /// <param name="eventID">Identifier for the event that the action will be unsubscribed from.</param>
        /// <param name="action">Action to not longer be associated with the event.</param>
        /// <returns>True if event is found. False otherwise.</returns>
        public static bool UnsubscribeFromEvent(GameEventIdentifier eventID, Action<GameEventData> action)
        {
            return UnsubscribeFromEvent(eventID.GetID(), action);
        }

        /// <summary>
        /// Toggles if the given action is subscribed or unsubscribed to/from the event with the given identifier.
        /// </summary>
        /// <param name="condition">Whether to subscribe (true) or unsubscribe (false) from the given event.</param>
        /// <param name="eventID">String hash for the event that the action will be subscribed/unsubscribed from.</param>
        /// <param name="action">Action to associate with the event.</param>
        /// <returns>True if event is found. False otherwise.</returns>
        public static bool ToggleSubscriptionToEvent(bool condition, string eventID, Action<GameEventData> action)
        {
            if (condition)
            {
                return SubscribeToEvent(eventID, action);
            }
            else
            {
                return UnsubscribeFromEvent(eventID, action);
            }
        }
        /// <summary>
        /// Toggles if the given action is subscribed or unsubscribed to/from the event with the given identifier.
        /// </summary>
        /// <param name="condition">Whether to subscribe (true) or unsubscribe (false) from the given event.</param>
        /// <param name="eventID">Identifier for the event that the action will be subscribed/unsubscribed from.</param>
        /// <param name="action">Action to associate with the event.</param>
        /// <returns>True if event is found. False otherwise.</returns>
        public static bool ToggleSubscriptionToEvent(bool condition, GameEventIdentifier eventID, Action<GameEventData> action)
        {
            return ToggleSubscriptionToEvent(condition, eventID.GetID(), action);
        }
    }
}