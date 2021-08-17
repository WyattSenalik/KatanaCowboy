using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameEventSystem.Editor
{
    // A custom inspector for Settings that does not draw the "Script" field.
    [UnityEditor.CustomEditor(typeof(GameEventSystemSettings), true)]
    public class GameEventSystemSettingsEditor : UnityEditor.Editor
    {
        // Constants
        // Fields not to draw in the editor
        private static readonly string[] EXCLUDED_FIELDS = { "m_Script" };
        // Max width for buttons
        private const float BUTTON_MAX_WIDTH = 100f;



        public override void OnInspectorGUI() => DrawDefaultInspector();

        // Draws the UI for exposed properties *without* the "Script" field.
        protected new bool DrawDefaultInspector()
        {
            if (serializedObject.targetObject == null) return false;

            EditorGUI.BeginChangeCheck();
            serializedObject.UpdateIfRequiredOrScript();

            DrawPropertiesExcluding(serializedObject, EXCLUDED_FIELDS);

            DrawButtons();

            serializedObject.ApplyModifiedProperties();
            return EditorGUI.EndChangeCheck();
        }

        private void DrawButtons()
        {
            GUILayout.BeginHorizontal();
            DrawSaveButton();
            GUILayout.FlexibleSpace();
            DrawSyncButton();
            GUILayout.EndHorizontal();
        }
        /// <summary>
        /// Draws the save button
        /// </summary>
        private void DrawSaveButton()
        {
            if (GUILayout.Button("Save", GUILayout.MaxWidth(BUTTON_MAX_WIDTH)))
            {
                GameEventSystemAssetManager.CreateAssets(GameEventSystemSettings.instance.GameEventList);
                EventListFileManager.CreateFile();

                Refresh();
            }
        }
        private void DrawSyncButton()
        {
            if (GUILayout.Button("Sync", GUILayout.MaxWidth(BUTTON_MAX_WIDTH)))
            {
                GameEventSystemSettings.instance.GameEventList = EventListFileManager.GetListOfEditorEvents().ToArray();

                Refresh();
            }
        }
        private void Refresh()
        {
            AssetDatabase.Refresh();
        }
    }
}