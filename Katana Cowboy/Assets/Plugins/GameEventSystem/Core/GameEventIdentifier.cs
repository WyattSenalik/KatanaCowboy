using GameEventSystem.Internal;

namespace GameEventSystem
{
    public class GameEventIdentifierBase : IGameEventIdentifier
    {
        public string GetID() => id;
        private string id = "";

        public GameEventIdentifierBase(string identifier)
        {
            id = identifier;
        }
    }
    public class GameEventIdentifier : GameEventIdentifierBase
    {
        public GameEventIdentifier(string identifier) : base(identifier) { }
    }
    public class GameEventIdentifier<T> : GameEventIdentifierBase
    {
        public GameEventIdentifier(string identifier) : base(identifier) { }
    }
    public class GameEventIdentifier<T, G> : GameEventIdentifierBase
    {
        public GameEventIdentifier(string identifier) : base(identifier) { }
    }
    public class GameEventIdentifier<T, G, H> : GameEventIdentifierBase
    {
        public GameEventIdentifier(string identifier) : base(identifier) { }
    }
    public class GameEventIdentifier<T, G, H, J> : GameEventIdentifierBase
    {
        public GameEventIdentifier(string identifier) : base(identifier) { }
    }
    public class GameEventIdentifier<T, G, H, J, K> : GameEventIdentifierBase
    {
        public GameEventIdentifier(string identifier) : base(identifier) { }
    }
    public class GameEventIdentifier<T, G, H, J, K, L> : GameEventIdentifierBase
    {
        public GameEventIdentifier(string identifier) : base(identifier) { }
    }
    public class GameEventIdentifier<T, G, H, J, K, L, I> : GameEventIdentifierBase
    {
        public GameEventIdentifier(string identifier) : base(identifier) { }
    }
}