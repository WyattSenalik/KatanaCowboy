using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace GameEventSystem.Editor
{
    public static class GameEventSystemAssetManager
    {
        public static void CreateAssets(IReadOnlyList<GameEventEditorElement> eventList)
        {
            EventListFileManager.VerifyFolderPaths();
            DeleteOldAssets();
            CreateNewAssets(eventList);
        }

        private static void DeleteOldAssets()
        {
            string[] rawFileNames = Directory.GetFiles(EventListFileManager.EVENT_FOLDER_PATH);
            foreach (string fileName in rawFileNames)
            {
                AssetDatabase.DeleteAsset(fileName);
            }
            AssetDatabase.SaveAssets();
        }
        private static void CreateNewAssets(IReadOnlyList<GameEventEditorElement> eventList)
        {
            foreach (GameEventEditorElement editorEvent in eventList)
            {
                GameEventIdentifierScriptableObject soID = new GameEventIdentifierScriptableObject();
                string fileName = EventListFileManager.GetFileNameFromEditorEvent(editorEvent);
                string path = EventListFileManager.GetEventAssetPath(fileName);
                AssetDatabase.CreateAsset(soID, path);
            }
            AssetDatabase.SaveAssets();
        }
    }
}
