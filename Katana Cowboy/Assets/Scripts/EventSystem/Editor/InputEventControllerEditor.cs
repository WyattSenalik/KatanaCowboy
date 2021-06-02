using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

namespace GameEventSystem
{
    [CustomEditor(typeof(InputEventController))]
    public class InputEventControllerEditor : Editor
    {
        private PlayerInput playerInput = null;
        private SerializedProperty inputEventIDs = null;


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            inputEventIDs = serializedObject.FindProperty("inputEventIDs");
            InputEventController inputEventController = target as InputEventController;
            playerInput = inputEventController.GetComponent<PlayerInput>();

            int actionEventCount = playerInput.actionEvents.Count;
            if (actionEventCount != inputEventController.GetInputEventsAmount())
            {
                string[] eventLabels = new string[actionEventCount];
                for (int i = 0; i < actionEventCount; ++i)
                {
                    string label = GetNameFromArrayIndex(i);
                    eventLabels[i] = label;
                }
                inputEventController.SetInputEventsAmount(actionEventCount, eventLabels);
            }

            // List header
            Rect myRect = GUILayoutUtility.GetRect(0.0f, 18.0f);
            bool showChildren = EditorGUI.PropertyField(myRect, inputEventIDs, new GUIContent("Input Events"));
            EditorGUILayout.Space(2.0f);
            bool shouldContinue = inputEventIDs.NextVisible(showChildren);
            // Get rid of the size box
            if (shouldContinue)
            {
                myRect = GUILayoutUtility.GetRect(0.0f, 0.0f);
                showChildren = EditorGUI.PropertyField(myRect, inputEventIDs, GUIContent.none);
                shouldContinue = inputEventIDs.NextVisible(showChildren);
            }

            int counter = 0;
            while (shouldContinue)
            {
                EditorGUI.indentLevel = 1;
                myRect = GUILayoutUtility.GetRect(0.0f, 18.0f);
                string name = GetNameFromArrayIndex(counter);
                showChildren = EditorGUI.PropertyField(myRect, inputEventIDs, new GUIContent(name));
                EditorGUILayout.Space(2.0f);
                shouldContinue = inputEventIDs.NextVisible(showChildren);
                // The direct child of the thing
                if (showChildren && shouldContinue)
                {
                    EditorGUI.indentLevel = 2;
                    myRect = GUILayoutUtility.GetRect(0.0f, 18.0f);
                    showChildren = EditorGUI.PropertyField(myRect, inputEventIDs, new GUIContent("Event Identifier"));
                    EditorGUILayout.Space(2.0f);
                    shouldContinue = inputEventIDs.NextVisible(showChildren);
                }
                ++counter;
            }

            serializedObject.ApplyModifiedProperties();
        }


        private string GetNameFromArrayIndex(int index)
        {
            int actionEventCount = playerInput.actionEvents.Count;
            if (index < actionEventCount)
            {
                string eventName = playerInput.actionEvents[index].actionName;
                int shortLength = eventName.IndexOf('[');
                string shortenedName = eventName.Substring(0, shortLength);
                return shortenedName;
            }
            else
            {
                return "Null";
            }
        }
    }
}