using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Hextant;
using Hextant.Editor;

namespace GameEventSystem.Editor
{
    [Settings(SettingsUsage.EditorProject, "GameEventSystem Settings")]
    public sealed class GameEventSystemSettings : Settings<GameEventSystemSettings>
    {
        public GameEventEditorElement[] GameEventList
        {
            get => gameEventList;
            set => gameEventList = value;
        }
        [SerializeField] private GameEventEditorElement[] gameEventList = new GameEventEditorElement[0];

        [SettingsProvider]
        static SettingsProvider GetSettingsProvider() =>
            instance.GetSettingsProvider();
    }
}