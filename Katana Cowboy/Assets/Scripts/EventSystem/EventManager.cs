using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEventSystem
{
    [CreateAssetMenu(fileName = "New EventManager", menuName = "ScriptableObjects/EventSystem/EventManager")]
    public class EventManager : ScriptableObject
    {
        public string FilePath => filePath;
        /// <summary>Folder structure to save the generated file in.</summary>
        [SerializeField] private string filePath = "Assets/EventWindow";
        public string PreviousFilePath => previousFilePath;
        [HideInInspector] [SerializeField] private string previousFilePath = "";

        public string EventIDListClassName => eventIDListClassName;
        /// <summary>Name of the class to generate in the file.</summary>
        [SerializeField] private string eventIDListClassName = "EventIDList";
        public string PreviousEventIDListClassName => previousClassName;
        [HideInInspector] [SerializeField] private string previousClassName = "";

        // List displayed in the editor to be edited by the user
        public string[] EventList => eventList;
        [SerializeField] private string[] eventList = new string[0];
        public string[] PreviousEventList => previousEventList;
        [HideInInspector] [SerializeField] private string[] previousEventList = new string[0];
    }
}