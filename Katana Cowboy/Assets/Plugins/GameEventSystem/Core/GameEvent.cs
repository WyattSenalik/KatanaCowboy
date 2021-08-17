
using GameEventSystem.Internal;

namespace GameEventSystem
{
    /// <summary>
    /// Base class for GameEvents to extend.
    /// </summary>
    public abstract class GameEventBase
    {
        // Identifier for the GameEvent
        private IGameEventIdentifier eventID = null;
        // Actual GameEvent to be called
        private GameEventInternal gameEvent = new GameEventInternal();
        // Data for the GameEvent
        private GameEventDataInternal eventData = new GameEventDataInternal();

        /// <summary>
        /// Constructs a GameEvent with the given string hash (id).
        /// </summary>
        /// <param name="gameEventID">Hash (id) for the GameEvent.</param>
        public GameEventBase(IGameEventIdentifier gameEventID)
        {
            eventID = gameEventID;

            gameEvent = new GameEventInternal();
            eventData = new GameEventDataInternal();
            EventSystem.CreateEvent(eventID, gameEvent);
        }
        /// <summary>
        /// Constructs a GameEvent with the given identifier.
        /// </summary>
        /// <param name="gameEventID">Identifier for the GameEvent.</param>
        public GameEventBase(GameEventIdentifierScriptableObject gameEventID) : this(gameEventID as IGameEventIdentifier) { }


        /// <summary>
        /// Resets parameters for the current invocation if they are not already reset.
        /// </summary>
        protected void ResetParameters()
        {
            eventData.ResetParameters();
        }
        /// <summary>
        /// Adds the given paramter to the event data.
        /// </summary>
        /// <typeparam name="T">Type of the parameter.</typeparam>
        /// <param name="param">Paramter to add.</param>
        protected void AddOrReplaceParameter<T>(T param)
        {
            eventData.AddOrReplaceValue(param);
        }
        /// <summary>
        /// Invokes the event with the game event data.
        /// </summary>
        protected void Invoke()
        {
            gameEvent?.Invoke(eventData);
        }
    }
    /// <summary>
    /// Wrapper class for the GameEventInternal.
    /// Any references to an instance of GameEvent should only be in the class that created it.
    /// </summary>
    public class GameEvent : GameEventBase
    {
        /// <summary>
        /// Constructs a GameEvent with the given string hash (id).
        /// </summary>
        /// <param name="gameEventID">Hash (id) for the GameEvent.</param>
        public GameEvent(GameEventIdentifier gameEventID) : base(gameEventID) { }
        /// <summary>
        /// Constructs a GameEvent for a scriptable object.
        /// </summary>
        /// <param name="gameEventID">Hash (id) for the GameEvent.</param>
        public GameEvent(GameEventIdentifierScriptableObject gameEventID) : base(gameEventID) { }


        /// <summary>
        /// Invokes the event with no parameters.
        /// </summary>
        new public void Invoke()
        {
            ResetParameters();
            base.Invoke();
        }
    }
    public class GameEvent<T> : GameEventBase
    {
        public GameEvent(GameEventIdentifier<T> gameEventID) : base(gameEventID) { }
        public GameEvent(GameEventIdentifierScriptableObject gameEventID) : base(gameEventID) { }

        /// <summary>
        /// Invokes the event with one parameter.
        /// </summary>
        public void Invoke(T param)
        {
            ResetParameters();
            AddOrReplaceParameter(param);
            base.Invoke();
        }
    }
    public class GameEvent<T, G> : GameEventBase
    {
        public GameEvent(GameEventIdentifier<T, G> gameEventID) : base(gameEventID) { }
        public GameEvent(GameEventIdentifierScriptableObject gameEventID) : base(gameEventID) { }

        /// <summary>
        /// Invokes the event with one parameter.
        /// </summary>
        public void Invoke(T param1, G param2)
        {
            ResetParameters();
            AddOrReplaceParameter(param1);
            AddOrReplaceParameter(param2);
            base.Invoke();
        }
    }
    public class GameEvent<T, G, H> : GameEventBase
    {
        public GameEvent(GameEventIdentifier<T, G, H> gameEventID) : base(gameEventID) { }
        public GameEvent(GameEventIdentifierScriptableObject gameEventID) : base(gameEventID) { }

        /// <summary>
        /// Invokes the event with one parameter.
        /// </summary>
        public void Invoke(T param1, G param2, H param3)
        {
            ResetParameters();
            AddOrReplaceParameter(param1);
            AddOrReplaceParameter(param2);
            AddOrReplaceParameter(param3);
            base.Invoke();
        }
    }
    public class GameEvent<T, G, H, J> : GameEventBase
    {
        public GameEvent(GameEventIdentifier<T, G, H, J> gameEventID) : base(gameEventID) { }
        public GameEvent(GameEventIdentifierScriptableObject gameEventID) : base(gameEventID) { }

        /// <summary>
        /// Invokes the event with one parameter.
        /// </summary>
        public void Invoke(T param1, G param2, H param3, J param4)
        {
            ResetParameters();
            AddOrReplaceParameter(param1);
            AddOrReplaceParameter(param2);
            AddOrReplaceParameter(param3);
            AddOrReplaceParameter(param4);
            base.Invoke();
        }
    }
    public class GameEvent<T, G, H, J, K> : GameEventBase
    {
        public GameEvent(GameEventIdentifier<T, G, H, J, K> gameEventID) : base(gameEventID) { }
        public GameEvent(GameEventIdentifierScriptableObject gameEventID) : base(gameEventID) { }

        /// <summary>
        /// Invokes the event with one parameter.
        /// </summary>
        public void Invoke(T param1, G param2, H param3, J param4, K param5)
        {
            ResetParameters();
            AddOrReplaceParameter(param1);
            AddOrReplaceParameter(param2);
            AddOrReplaceParameter(param3);
            AddOrReplaceParameter(param4);
            AddOrReplaceParameter(param5);
            base.Invoke();
        }
    }
    public class GameEvent<T, G, H, J, K, L> : GameEventBase
    {
        public GameEvent(GameEventIdentifier<T, G, H, J, K, L> gameEventID) : base(gameEventID) { }
        public GameEvent(GameEventIdentifierScriptableObject gameEventID) : base(gameEventID) { }

        /// <summary>
        /// Invokes the event with one parameter.
        /// </summary>
        public void Invoke(T param1, G param2, H param3, J param4, K param5, L param6)
        {
            ResetParameters();
            AddOrReplaceParameter(param1);
            AddOrReplaceParameter(param2);
            AddOrReplaceParameter(param3);
            AddOrReplaceParameter(param4);
            AddOrReplaceParameter(param5);
            AddOrReplaceParameter(param6);
            base.Invoke();
        }
    }
    public class GameEvent<T, G, H, J, K, L, I> : GameEventBase
    {
        public GameEvent(GameEventIdentifier<T, G, H, J, K, L, I> gameEventID) : base(gameEventID) { }
        public GameEvent(GameEventIdentifierScriptableObject gameEventID) : base(gameEventID) { }

        /// <summary>
        /// Invokes the event with one parameter.
        /// </summary>
        public void Invoke(T param1, G param2, H param3, J param4, K param5, L param6, I param7)
        {
            ResetParameters();
            AddOrReplaceParameter(param1);
            AddOrReplaceParameter(param2);
            AddOrReplaceParameter(param3);
            AddOrReplaceParameter(param4);
            AddOrReplaceParameter(param5);
            AddOrReplaceParameter(param6);
            AddOrReplaceParameter(param7);
            base.Invoke();
        }
    }
}