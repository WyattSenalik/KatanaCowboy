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


        public override void OnInspectorGUI()
        {
            SerializedProperty inputEvents = serializedObject.FindProperty("inputEvents");
            InputEventController inputEventController = target as InputEventController;
            playerInput = inputEventController.GetComponent<PlayerInput>();

            int actionEventCount = playerInput.actionEvents.Count;
            inputEventController.SetInputEventLength(actionEventCount);

            bool shouldContinue = true;
            int counter = 0;
            while (shouldContinue)
            {
                bool showChildren = true;
                if (counter != 1)
                {
                    Rect myRect = GUILayoutUtility.GetRect(0.0f, 18.0f);
                    string name = GetNameFromIndex(counter);
                    showChildren = EditorGUI.PropertyField(myRect, inputEvents, new GUIContent(name));
                }
                else
                {
                    EditorGUI.PropertyField(GUILayoutUtility.GetRect(0.0f, 0.0f), inputEvents, GUIContent.none);
                }
                shouldContinue = inputEvents.NextVisible(showChildren);
                if (counter != 1)
                {
                    EditorGUILayout.Space(2.0f);
                }
                ++counter;
            }
        }



        private string GetNameFromIndex(int index)
        {
            if (index == 0)
            {
                return "Input Events";
            }
            else if (index == 1)
            {
                return "Size";
            }
            else
            {
                int actionEventCount = playerInput.actionEvents.Count;
                int curIndex = index - 2;
                if (curIndex - 2 < actionEventCount)
                {
                    string eventName = playerInput.actionEvents[curIndex].actionName;
                    int shortLength = eventName.IndexOf('[');
                    string shortenedName = eventName.Substring(0, shortLength);
                    return shortenedName;
                }
                else
                {
                    return "Null";//
                }
            }
        }
    }
}