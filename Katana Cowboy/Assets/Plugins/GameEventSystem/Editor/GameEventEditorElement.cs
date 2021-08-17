using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEventSystem.Editor
{
    [Serializable]
    public class GameEventEditorElement
    {
        public string EventName => eventName;
        [SerializeField] private string eventName = "";

        public List<string> ParamStringList => paramStringList;
        [SerializeField] private List<string> paramStringList = new List<string>();


        public GameEventEditorElement(string name, params string[] paramTypes)
        {
            eventName = name;
            paramStringList = new List<string>(paramTypes);
        }
    }
}