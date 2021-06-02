﻿using System;
using System.Collections.Generic;
using UnityEngine;

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
        /// <param name="eventID">Identifier for the event that will be used to subscribe to it.</param>
        /// <param name="eventToCall">Reference to the event.</param>
        public static void CreateEvent(GameEventIdentifier eventID, IGameEvent eventToCall)
        {
            if (!eventsHash.ContainsKey(eventID.GetID()))
            {
                eventsHash.Add(eventID.GetID(), eventToCall);
            }
            else
            {
                Debug.LogError("Event with ID " + eventID + " already exists");
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
            if (eventsHash.ContainsKey(eventID.GetID()))
            {
                eventsHash[eventID.GetID()].Subscribe(action);
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
            if (eventsHash.ContainsKey(eventID.GetID()))
            {
                eventsHash[eventID.GetID()].Unsubscribe(action);
                return true;
            }
            else
            {
                Debug.LogError("No event of name " + eventID + " exists");
                return false;
            }
        }
    }
}