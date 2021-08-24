using UnityEngine;

using GameEventSystem.Internal;

namespace GameEventSystem
{
    /// <summary>
    /// ID for a GameEvent thats data is the name of the created scriptable object.
    /// </summary>
    [CreateAssetMenu(fileName = "New GameEventID", menuName = "ScriptableObjects/EventSystem/GameEventIdentifier")]
    public class GameEventIdentifierScriptableObject : ScriptableObject, IGameEventIdentifier
    {
        public string GetID() => this.name;
    }
}