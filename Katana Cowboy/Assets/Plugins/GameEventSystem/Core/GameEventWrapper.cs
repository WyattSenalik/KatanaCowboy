using UnityEngine;

using GameEventSystem.Internal;

namespace GameEventSystem
{
    /// <summary>
    /// Wrapper class for the GameEvent that makes it more convenient to create a GameEvent.
    /// Any references to an instance of GameEventWrapper should only be in the class that created it.
    /// </summary>
    public class GameEventWrapper
    {
        // Identifier for the GameEvent
        private string eventID = "";
        // Actual GameEvent to be called
        private GameEvent gameEvent = new GameEvent();
        // Data for the GameEvent
        private GameEventDataInternal eventData = new GameEventDataInternal();
        // If the event has been created alrady
        private bool wasEventCreated = false;
        // If the event has already cleared its parameters for the current invocation
        private bool areParametersReset = false;


        /// <summary>
        /// Constructs a GameEventWrapper with the given string hash (id).
        /// </summary>
        /// <param name="gameEventID">string hash (id) for the GameEvent.</param>
        public GameEventWrapper(string gameEventID)
        {
            eventID = gameEventID;
            CreateEvent();
        }
        /// <summary>
        /// Constructs a GameEventWrapper with the given identifier.
        /// </summary>
        /// <param name="gameEventID">Identifier for the GameEvent.</param>
        public GameEventWrapper(GameEventIdentifier gameEventID) : this(gameEventID.GetID()) { }


        /// <summary>
        /// Invokes the event with no parameters.
        /// </summary>
        public void Invoke()
        {
            ResetParameters();
            gameEvent?.Invoke(eventData);
            // Allow parameters to be reset for next time
            areParametersReset = false;
        }
        /// <summary>
        /// Invokes the event with one parameter.
        /// </summary>
        public void Invoke<T>(T param)
        {
            ResetParameters();
            AddOrReplaceParameter(param);
            Invoke();
        }
        /// <summary>
        /// Invokes the event with two parameters.
        /// </summary>
        public void Invoke<T, G>(T param0, G param1)
        {
            ResetParameters();
            AddOrReplaceParameter(param0);
            Invoke(param1);
        }
        /// <summary>
        /// Invokes the event with three parameters.
        /// </summary>
        public void Invoke<T, G, H>(T param0, G param1, H param2)
        {
            ResetParameters();
            AddOrReplaceParameter(param0);
            Invoke(param1, param2);
        }
        /// <summary>
        /// Invokes the event with four parameters.
        /// </summary>
        public void Invoke<T, G, H, J>(T param0, G param1, H param2, J param3)
        {
            ResetParameters();
            AddOrReplaceParameter(param0);
            Invoke(param1, param2, param3);
        }
        /// <summary>
        /// Invokes the event with five parameters.
        /// </summary>
        public void Invoke<T, G, H, J, K>(T param0, G param1, H param2, J param3, K param4)
        {
            ResetParameters();
            AddOrReplaceParameter(param0);
            Invoke(param1, param2, param3, param4);
        }
        /// <summary>
        /// Invokes the event with six parameters.
        /// </summary>
        public void Invoke<T, G, H, J, K, L>(T param0, G param1, H param2, J param3, K param4, L param5)
        {
            ResetParameters();
            AddOrReplaceParameter(param0);
            Invoke(param1, param2, param3, param4, param5);
        }
        /// <summary>
        /// Invokes the event with seven parameters.
        /// Why would you ever need this many?
        /// </summary>
        public void Invoke<T, G, H, J, K, L, I>(T param0, G param1, H param2, J param3, K param4, L param5, I param6)
        {
            ResetParameters();
            AddOrReplaceParameter(param0);
            Invoke(param1, param2, param3, param4, param5, param6);
        }


        /// <summary>
        /// Initializes and creates the event.
        /// Will only create the event once even if called multiple times.
        /// </summary>
        private void CreateEvent()
        {
            if (!wasEventCreated)
            {
                Initialize();
                EventSystem.CreateEvent(eventID, gameEvent);
                wasEventCreated = true;
            }
            else
            {
                Debug.LogWarning("Attempted to create GameEvent " + eventID + " multiple times");
            }
        }
        /// <summary>
        /// Initializes the GameEvent and the event data.
        /// </summary>
        private void Initialize()
        {
            gameEvent = new GameEvent();
            eventData = new GameEventDataInternal();
        }
        /// <summary>
        /// Resets parameters for the current invocation if they are not already reset.
        /// </summary>
        private void ResetParameters()
        {
            if (!areParametersReset)
            {
                eventData.ResetParameters();
                areParametersReset = true;
            }
        }
        /// <summary>
        /// Adds the given paramter to the event data.
        /// </summary>
        /// <typeparam name="T">Type of the parameter.</typeparam>
        /// <param name="param">Paramter to add.</param>
        private void AddOrReplaceParameter<T>(T param)
        {
            eventData.AddOrReplaceValue(param);
        }
    }
}