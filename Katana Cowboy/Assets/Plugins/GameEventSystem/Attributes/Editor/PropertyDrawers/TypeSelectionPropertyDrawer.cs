using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameEventSystemAttributes
{
    [CustomPropertyDrawer(typeof(TypeSelectionAttribute))]
    public class TypeSelectionPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            if (property.propertyType == SerializedPropertyType.String)
            {
                // generate the typelist
                List<Type> typeList = AssemblyManager.GetTypes();
                typeList.Sort((Type t1, Type t2) => { return t1.Name.CompareTo(t2.Name); });
                List<string> nameList = AssemblyManager.GetTypeNames(typeList);
                List<string> fullNameList = AssemblyManager.GetTypeFullNames(typeList);

                string propertyString = property.stringValue;
                int index = 0;
                // check if there is an entry that matches the entry and get the index
                // we skip index 0 as that is a special custom case
                for (int i = 1; i < nameList.Count; i++)
                {
                    if (nameList[i].Equals(propertyString, System.StringComparison.Ordinal))
                    {
                        index = i;
                        break;
                    }
                }
                
                // Draw the popup box with the current selected index
                int newIndex = EditorGUI.Popup(rect, label.text, index, nameList.ToArray());

                // Adjust the actual string value of the property based on the selection
                // But make it the full name of the type
                string newValue = newIndex > 0 ? fullNameList[newIndex] : string.Empty;

                if (!property.stringValue.Equals(newValue, System.StringComparison.Ordinal))
                {
                    property.stringValue = newValue;
                }
            }
            else
            {
                string message = string.Format("{0} supports only string fields", typeof(TypeSelectionPropertyDrawer).Name);
                Debug.LogWarning(message);
            }

            EditorGUI.EndProperty();
        }
    }
}