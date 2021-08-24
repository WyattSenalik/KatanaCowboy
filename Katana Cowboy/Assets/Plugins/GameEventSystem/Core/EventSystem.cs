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
        /// <param name="eventID">ID hash for the event that will be used to subscribe to it.</param>
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
        public static void CreateEvent(IGameEventIdentifier eventID, GameEventInternal eventToCall)
        {
            CreateEvent(eventID.GetID(), eventToCall);
        }
        /// <summary>
        /// Puts a new event into the EventSystem.
        /// Cannot add multiple events with the same eventID.
        /// </summary>
        /// <param name="eventID">Identifier for the event that will be used to subscribe to it.</param>
        /// <param name="eventToCall">Reference to the event.</param>
        public static void CreateEvent(GameEventIdentifierScriptableObject eventID, GameEventInternal eventToCall)
        {
            CreateEvent(eventID.GetID(), eventToCall);
        }


        #region SubscribeToEvent
        /// <summary>
        /// Subscribes the action to the event with the given identifier.
        /// </summary>
        /// <param name="eventID">ID hash for the event that the action will be subscribed to.</param>
        /// <param name="action">Action to be called by the event when invoked.</param>
        /// <returns>True if event is found. False otherwise.</returns>
        public static bool SubscribeToEvent(GameEventIdentifier eventID, Action action)
        {
            return SubscribeToEvent(eventID, CreateGameEventDataAction(action));
        }
        public static bool SubscribeToEvent<T>(GameEventIdentifier<T> eventID, Action<T> action)
        {
            return SubscribeToEvent(eventID, CreateGameEventDataAction(action));
        }
        public static bool SubscribeToEvent<T, G>(GameEventIdentifier<T, G> eventID, Action<T, G> action)
        {
            return SubscribeToEvent(eventID, CreateGameEventDataAction(action));
        }
        public static bool SubscribeToEvent<T, G, H>(GameEventIdentifier<T, G, H> eventID, Action<T, G, H> action)
        {
            return SubscribeToEvent(eventID, CreateGameEventDataAction(action));
        }
        public static bool SubscribeToEvent<T, G, H, J>(GameEventIdentifier<T, G, H, J> eventID, Action<T, G, H, J> action)
        {
            return SubscribeToEvent(eventID, CreateGameEventDataAction(action));
        }
        public static bool SubscribeToEvent<T, G, H, J, K>(GameEventIdentifier<T, G, H, J, K> eventID, Action<T, G, H, J, K> action)
        {
            return SubscribeToEvent(eventID, CreateGameEventDataAction(action));
        }
        public static bool SubscribeToEvent<T, G, H, J, K, L>(GameEventIdentifier<T, G, H, J, K, L> eventID,
            Action<T, G, H, J, K, L> action)
        {
            return SubscribeToEvent(eventID, CreateGameEventDataAction(action));
        }
        public static bool SubscribeToEvent<T, G, H, J, K, L, I>(GameEventIdentifier<T, G, H, J, K, L, I> eventID,
            Action<T, G, H, J, K, L, I> action)
        {
            return SubscribeToEvent(eventID, CreateGameEventDataAction(action));
        }
        /// <summary>
        /// Subscribes the action to the event with the given identifier.
        /// </summary>
        /// <param name="eventID">Identifier for the event that the action will be subscribed to.</param>
        /// <param name="action">Action to be called by the event when invoked.</param>
        /// <returns>True if event is found. False otherwise.</returns>
        public static bool SubscribeToEvent(GameEventIdentifierScriptableObject eventID, Action<GameEventData> action)
        {
            return SubscribeToEvent(eventID as IGameEventIdentifier, action);
        }

        /// <summary>
        /// Helper function to subscribe the given action to the event linked with the given ID.
        /// </summary>
        private static bool SubscribeToEvent(IGameEventIdentifier eventID, Action<GameEventData> action)
        {
            if (eventsHash.ContainsKey(eventID.GetID()))
            {
                eventsHash[eventID.GetID()].Subscribe(action);
                return true;
            }
            // If the event does not exist yet, create a fake event temporarily
            else
            {
                FakeGameEvent fakeEvent = new FakeGameEvent();
                fakeEvent.Subscribe(action);
                eventsHash.Add(eventID.GetID(), fakeEvent);

                //Debug.LogWarning("No event of id " + eventID + " exists. FakeEvent was created.");
                return false;
            }
        }
        #endregion SubscribeToEvent

        #region UnsubscribeFromEvent
        /// <summary>
        /// Unsubscribes the action from the event with the given identifier.
        /// </summary>
        /// <param name="eventID">ID hash for the event that the action will be unsubscribed from.</param>
        /// <param name="action">Action to not longer be associated with the event.</param>
        /// <returns>True if event is found. False otherwise.</returns>
        public static bool UnsubscribeFromEvent(GameEventIdentifier eventID, Action action)
        {
            return UnsubscribeFromEvent(eventID, CreateGameEventDataAction(action));
        }
        public static bool UnsubscribeFromEvent<T>(GameEventIdentifier<T> eventID, Action<T> action)
        {
            return UnsubscribeFromEvent(eventID, CreateGameEventDataAction(action));
        }
        public static bool UnsubscribeFromEvent<T, G>(GameEventIdentifier<T, G> eventID, Action<T, G> action)
        {
            return UnsubscribeFromEvent(eventID, CreateGameEventDataAction(action));
        }
        public static bool UnsubscribeFromEvent<T, G, H>(GameEventIdentifier<T, G, H> eventID, Action<T, G, H> action)
        {
            return UnsubscribeFromEvent(eventID, CreateGameEventDataAction(action));
        }
        public static bool UnsubscribeFromEvent<T, G, H, J>(GameEventIdentifier<T, G, H, J> eventID, Action<T, G, H, J> action)
        {
            return UnsubscribeFromEvent(eventID, CreateGameEventDataAction(action));
        }
        public static bool UnsubscribeFromEvent<T, G, H, J, K>(GameEventIdentifier<T, G, H, J, K> eventID, Action<T, G, H, J, K> action)
        {
            return UnsubscribeFromEvent(eventID, CreateGameEventDataAction(action));
        }
        public static bool UnsubscribeFromEvent<T, G, H, J, K, L>(GameEventIdentifier<T, G, H, J, K, L> eventID,
            Action<T, G, H, J, K, L> action)
        {
            return UnsubscribeFromEvent(eventID, CreateGameEventDataAction(action));
        }
        public static bool UnsubscribeFromEvent<T, G, H, J, K, L, I>(GameEventIdentifier<T, G, H, J, K, L, I> eventID,
            Action<T, G, H, J, K, L, I> action)
        {
            return UnsubscribeFromEvent(eventID, CreateGameEventDataAction(action));
        }
        /// <summary>
        /// Unsubscribes the action from the event with the given identifier.
        /// </summary>
        /// <param name="eventID">Identifier for the event that the action will be unsubscribed from.</param>
        /// <param name="action">Action to not longer be associated with the event.</param>
        /// <returns>True if event is found. False otherwise.</returns>
        public static bool UnsubscribeFromEvent(GameEventIdentifierScriptableObject eventID, Action<GameEventData> action)
        {
            return UnsubscribeFromEvent(eventID as IGameEventIdentifier, action);
        }

        /// <summary>
        /// Helper functions to unsubscribe the given action from the event linked with the given ID.
        /// </summary>
        private static bool UnsubscribeFromEvent(IGameEventIdentifier eventID, Action<GameEventData> action)
        {
            if (eventsHash.ContainsKey(eventID.GetID()))
            {
                eventsHash[eventID.GetID()].Unsubscribe(action);
                return true;
            }
            else
            {
                Debug.LogError("No event with id " + eventID.GetID() + " exists");
                return false;
            }
        }
        #endregion UnsubscribeFromEvent

        #region ToggleSubscriptionToEvent
        /// <summary>
        /// Toggles if the given action is subscribed or unsubscribed to/from the event with the given identifier.
        /// </summary>
        /// <param name="condition">Whether to subscribe (true) or unsubscribe (false) from the given event.</param>
        /// <param name="eventID">ID hash for the event that the action will be subscribed/unsubscribed from.</param>
        /// <param name="action">Action to associate with the event.</param>
        /// <returns>True if event is found. False otherwise.</returns>
        public static bool ToggleSubscriptionToEvent(bool condition, GameEventIdentifier eventID, Action action)
        {
            return ToggleSubscriptionToEvent(condition, eventID, CreateGameEventDataAction(action));
        }
        public static bool ToggleSubscriptionToEvent<T>(bool condition, GameEventIdentifier<T> eventID, Action<T> action)
        {
            return ToggleSubscriptionToEvent(condition, eventID, CreateGameEventDataAction(action));
        }
        public static bool ToggleSubscriptionToEvent<T, G>(bool condition, GameEventIdentifier<T, G> eventID, Action<T, G> action)
        {
            return ToggleSubscriptionToEvent(condition, eventID, CreateGameEventDataAction(action));
        }
        public static bool ToggleSubscriptionToEvent<T, G, H>(bool condition, GameEventIdentifier<T, G, H> eventID,
            Action<T, G, H> action)
        {
            return ToggleSubscriptionToEvent(condition, eventID, CreateGameEventDataAction(action));
        }
        public static bool ToggleSubscriptionToEvent<T, G, H, J>(bool condition, GameEventIdentifier<T, G, H, J> eventID,
            Action<T, G, H, J> action)
        {
            return ToggleSubscriptionToEvent(condition, eventID, CreateGameEventDataAction(action));
        }
        public static bool ToggleSubscriptionToEvent<T, G, H, J, K>(bool condition, GameEventIdentifier<T, G, H, J, K> eventID,
            Action<T, G, H, J, K> action)
        {
            return ToggleSubscriptionToEvent(condition, eventID, CreateGameEventDataAction(action));
        }
        public static bool ToggleSubscriptionToEvent<T, G, H, J, K, L>(bool condition,
            GameEventIdentifier<T, G, H, J, K, L> eventID, Action<T, G, H, J, K, L> action)
        {
            return ToggleSubscriptionToEvent(condition, eventID, CreateGameEventDataAction(action));
        }
        public static bool ToggleSubscriptionToEvent<T, G, H, J, K, L, I>(bool condition,
            GameEventIdentifier<T, G, H, J, K, L, I> eventID, Action<T, G, H, J, K, L, I> action)
        {
            return ToggleSubscriptionToEvent(condition, eventID, CreateGameEventDataAction(action));
        }
        /// <summary>
        /// Toggles if the given action is subscribed or unsubscribed to/from the event with the given identifier.
        /// </summary>
        /// <param name="condition">Whether to subscribe (true) or unsubscribe (false) from the given event.</param>
        /// <param name="eventID">Identifier for the event that the action will be subscribed/unsubscribed from.</param>
        /// <param name="action">Action to associate with the event.</param>
        /// <returns>True if event is found. False otherwise.</returns>
        public static bool ToggleSubscriptionToEvent(bool condition, GameEventIdentifierScriptableObject eventID, Action<GameEventData> action)
        {
            return ToggleSubscriptionToEvent(condition, eventID as IGameEventIdentifier, action);
        }

        /// <summary>
        /// Helper to toggle if the given action is subscribed or unsubscribed to/from the event with the given identifier.
        /// </summary>
        private static bool ToggleSubscriptionToEvent(bool condition, IGameEventIdentifier eventID, Action<GameEventData> action)
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
        #endregion ToggleSubscriptionToEvent


        #region CreateGameEventDataAction
        /// <summary>
        /// Creates a Action<GameEventData> from the given action.
        /// </summary>
        private static Action<GameEventData> CreateGameEventDataAction(Action action)
        {
            return (GameEventData data) => action.Invoke();
        }
        private static Action<GameEventData> CreateGameEventDataAction<T>(Action<T> action)
        {
            return (GameEventData data) =>
            {
                action.Invoke(data.ReadValue<T>());
            };
        }
        private static Action<GameEventData> CreateGameEventDataAction<T, G>(Action<T, G> action)
        {
            return (GameEventData data) =>
            {
                action.Invoke(data.ReadValue<T>(), data.ReadValue<G>());
            };
        }
        private static Action<GameEventData> CreateGameEventDataAction<T, G, H>(Action<T, G, H> action)
        {
            return (GameEventData data) =>
            {
                action.Invoke(data.ReadValue<T>(), data.ReadValue<G>(), data.ReadValue<H>());
            };
        }
        private static Action<GameEventData> CreateGameEventDataAction<T, G, H, J>(Action<T, G, H, J> action)
        {
            return (GameEventData data) =>
            {
                action.Invoke(data.ReadValue<T>(), data.ReadValue<G>(), data.ReadValue<H>(),
                    data.ReadValue<J>());
            };
        }
        private static Action<GameEventData> CreateGameEventDataAction<T, G, H, J, K>(Action<T, G, H, J, K> action)
        {
            return (GameEventData data) =>
            {
                action.Invoke(data.ReadValue<T>(), data.ReadValue<G>(), data.ReadValue<H>(),
                    data.ReadValue<J>(), data.ReadValue<K>());
            };
        }
        private static Action<GameEventData> CreateGameEventDataAction<T, G, H, J, K, L>(Action<T, G, H, J, K, L> action)
        {
            return (GameEventData data) =>
            {
                action.Invoke(data.ReadValue<T>(), data.ReadValue<G>(), data.ReadValue<H>(),
                    data.ReadValue<J>(), data.ReadValue<K>(), data.ReadValue<L>());
            };
        }
        private static Action<GameEventData> CreateGameEventDataAction<T, G, H, J, K, L, I>(Action<T, G, H, J, K, L, I> action)
        {
            return (GameEventData data) =>
            {
                action.Invoke(data.ReadValue<T>(), data.ReadValue<G>(), data.ReadValue<H>(),
                    data.ReadValue<J>(), data.ReadValue<K>(), data.ReadValue<L>(), data.ReadValue<I>());
            };
        }
        #endregion CreateGameEventDataAction
    }
}