using UnityEngine;

namespace GameEventSystem
{
    /// <summary>
    /// ID for a GameEvent thats data is the name of the created scriptable object.
    /// </summary>
    [CreateAssetMenu(fileName = "New GameEventID", menuName = "ScriptableObjects/EventSystem/GameEventIdentifier")]
    public class GameEventIdentifier : ScriptableObject
    {
        /// <summary>
        /// Gets the ID (asset name) for the GameEventID.
        /// </summary>
        /// <returns>string ID for the GameEvent.</returns>
        public string GetID()
        {
            return this.name;
        }

        public override string ToString()
        {
            return GetID();
        }
    }
}