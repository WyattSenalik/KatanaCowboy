using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameEventSystem
{
    [CustomEditor(typeof(InputEventController))]
    public class InputEventControllerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            Repaint();

            GUILayout.Space(20f);
            if (GUILayout.Button("Update"))
            {
                InputEventController inpEventCont = serializedObject.targetObject as InputEventController;
                inpEventCont.UpdateInputEvents();

                Repaint();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}