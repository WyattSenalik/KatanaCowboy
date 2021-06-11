using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;


namespace GameEventSystem
{
    /// <summary>
    /// Custom EditorWindow to create events and generate a script called EventList.
    /// </summary>
    public class EventWindow : EditorWindow
    {
        // Constants
        // Title of the window
        private const string WINDOW_TITLE = "Event Manager";
        // Max widths for style
        private const float BUTT_MAX_WIDTH = 100f;
        private const float EVENT_LABEL_MIN_WIDTH = 200f;

        // References to all the existing events
        private List<string> eventList = new List<string>();
        private List<EventToRename> renames = new List<EventToRename>();
        private int editableEventName = -1;
        private string originalEventName = "";
        private string renamedEventName = "";

        // Text field name for new event
        private string textFieldEventName = "";
        // Bool to hold if auto save is on
        private bool isAutoSave = true;
        // If we just hit the save button
        private bool justHitSave = false;


        /// <summary>
        /// Opens the editor window from the menu.
        /// </summary>
        [MenuItem("Window/" + WINDOW_TITLE)]
        public static void ShowWindow()
        {
            GetWindow<EventWindow>(WINDOW_TITLE);
        }


        // Called as the new window is opened
        private void Awake()
        {
            UpdateEventListFromFileSystem();
        }
        // Called when creating, renaming, or reparenting assets, as well as moving or renaming folders in the project
        private void OnProjectChange()
        {
            if (justHitSave || isAutoSave)
            {
                UpdateEventListFromFileSystem();
                InitializeEventList.CreateFile();
                justHitSave = false;
            }
        }
        // Called every GUI repaint call
        private void OnGUI()
        {
            GUILayout.Space(10f);
            // Add the auto save or manual save stuff
            DisplaySaveOptions();
            GUILayout.Space(25f);
            // Add the add event button
            DisplayAddEventButton();
            // Give a little space for the list to get away from the button
            GUILayout.Space(10f);
            // Two strings for the path and name of the class
            //EditorGUILayout.PropertyField(serializedFilePath);
            //EditorGUILayout.PropertyField(serializedClassName);
            // Event list
            DisplayEventList();
        }


        private void DisplaySaveOptions()
        {
            GUILayout.BeginHorizontal();
            {
                GUI.enabled = !isAutoSave;
                {
                    if (GUILayout.Button("Save", GUILayout.MaxWidth(BUTT_MAX_WIDTH)))
                    {
                        justHitSave = true;
                        ApplySavedChanges();
                    }
                }
                GUI.enabled = true;

                GUILayout.Label("Auto-Save", GUILayout.MinWidth(60), GUILayout.ExpandWidth(false));
                bool prevSaveState = isAutoSave;
                isAutoSave = EditorGUILayout.Toggle(isAutoSave);
                if (isAutoSave && !prevSaveState)
                {
                    ApplySavedChanges();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void DisplayAddEventButton()
        {
            GUILayout.BeginHorizontal();
            {
                textFieldEventName = EditorGUILayout.TextField("Event Name", textFieldEventName);

                string addEventButtName = "Add Event";
                bool isEventNameValid = IsValidEventName(textFieldEventName);

                GUI.enabled = isEventNameValid;
                {
                    if (!isEventNameValid)
                    {
                        addEventButtName += " [Invalid Event Name]";
                    }

                    if (GUILayout.Button(addEventButtName))
                    {
                        AddEvent(textFieldEventName);
                    }
                }
                GUI.enabled = true;
            }
            GUILayout.EndHorizontal();
        }

        private void DisplayEventList()
        {
            GUIStyle eventLabelStyle = new GUIStyle(GUI.skin.box)
            {
                alignment = TextAnchor.MiddleLeft,
                padding = new RectOffset(5, 0, 3, 0)
            };
            GUILayout.BeginVertical();
            for (int i = 0; i < eventList.Count; ++i)
            {
                GUILayout.BeginHorizontal();
                {
                    if (editableEventName != i)
                    {
                        GUI.enabled = false;
                        GUILayout.TextField(eventList[i], eventLabelStyle, GUILayout.MinWidth(EVENT_LABEL_MIN_WIDTH), GUILayout.ExpandWidth(false));
                        GUI.enabled = true;
                        if (GUILayout.Button("Rename", GUILayout.MaxWidth(BUTT_MAX_WIDTH)))
                        {
                            AllowEventNameRename(i);
                        }
                        if (GUILayout.Button("Delete", GUILayout.MaxWidth(BUTT_MAX_WIDTH)))
                        {
                            DeleteEvent(i);
                        }
                    }
                    else
                    {
                        renamedEventName = GUILayout.TextField(renamedEventName, eventLabelStyle, GUILayout.MinWidth(EVENT_LABEL_MIN_WIDTH), GUILayout.ExpandWidth(false));
                        if (GUILayout.Button("Confirm", GUILayout.MaxWidth(BUTT_MAX_WIDTH)))
                        {
                            RenameEvent(i);
                        }
                        if (GUILayout.Button("Cancel", GUILayout.MaxWidth(BUTT_MAX_WIDTH)))
                        {
                            CancelEventNameRename();
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Checks if the given event name is valid.
        /// An event name is invalid if:
        ///     1. It is "".
        ///     2. It starts with a number.
        ///     3. It contains non-alphanumeric characters.
        ///     4. Another event already has that name.
        /// </summary>
        /// <param name="eventName">Event name to check validity.</param>
        /// <returns>True if the event is valid. False if found invalid.</returns>
        private bool IsValidEventName(string eventName)
        {
            Regex alphanumericRegex = new Regex("^[a-zA-Z][a-zA-Z0-9]*$");
            // Make sure the string is not empty
            if (eventName.Length < 1)
            {
                return false;
            }
            // Check if the first character is a number
            if (char.IsDigit(eventName[0]))
            {
                return false;
            }
            // Check if the event name contains non-alphanumeric characters
            if (!eventName.All(char.IsLetterOrDigit))
            {
                return false;
            }
            // Check if any events share the same name
            for (int k = 0; k < eventList.Count; ++k)
            {
                if (eventName == eventList[k])
                {
                    return false;
                }
            }

            return true;
        }

        private void UpdateEventListFromFileSystem()
        {
            eventList = new List<string>(EventListFileManager.GetListOfEventNames());
        }

        private void AllowEventNameRename(int index)
        {
            editableEventName = index;
            originalEventName = eventList[index];
            renamedEventName = originalEventName;
        }
        private void CancelEventNameRename()
        {
            editableEventName = -1;
        }


        private void AddEvent(string eventName)
        {
            // Add the event into the array
            eventList.Add(eventName);
            // Check for auto save
            CheckAutoSave();
        }
        private void DeleteEvent(int index)
        {
            // Remove the event from the array
            string eventToDelete = eventList[index];
            eventList.Remove(eventToDelete);
            // Check if the deleted event was a rename, get rid of that rename
            int renamedIndex = -1;
            for (int i = 0; i < renames.Count; ++i)
            {
                if (eventToDelete == renames[i].RenamedEventName)
                {
                    renamedIndex = i;
                }
            }
            if (renamedIndex != -1)
            {
                renames.RemoveAt(renamedIndex);
            }
            // Check for auto save
            CheckAutoSave();
        }
        private void RenameEvent(int index)
        {
            // Remove the old event and add the new one
            eventList.Remove(originalEventName);
            eventList.Add(renamedEventName);
            string printOut = "Rename: ";
            foreach (string eventName in eventList)
            {
                printOut += eventName + "; ";
            }
            Debug.Log(printOut);
            // Check if this rename is a rename of a previous rename
            string actualOriginalName = originalEventName;
            int renamedIndex = -1;
            for (int i = 0; i < renames.Count; ++i)
            {
                if (originalEventName == renames[i].RenamedEventName)
                {
                    actualOriginalName = renames[i].OriginalEventName;
                    renamedIndex = i;
                }
            }
            if (renamedIndex != -1)
            {
                renames.RemoveAt(renamedIndex);
            }
            renames.Add(new EventToRename(actualOriginalName, renamedEventName));
            // Set the temporary variables to allow name editing
            CancelEventNameRename();
            // Check for auto save
            CheckAutoSave();
        }
        private bool CheckAutoSave()
        {
            if (isAutoSave)
            {
                ApplySavedChanges();
                return true;
            }
            return false;
        }
        private void ApplySavedChanges()
        {
            // Confirm the directory exists
            if (!Directory.Exists(EventListFileManager.EVENT_SAVE_PATH))
            {
                Directory.CreateDirectory(EventListFileManager.EVENT_SAVE_PATH);
            }
            string printOut = "";
            foreach (string eventName in eventList)
            {
                printOut += eventName + "; ";
            }
            Debug.Log("EventList: " + printOut);

            // Get rid of all the old files if they aren't in the new files
            string[] eventsInDirectory = EventListFileManager.GetListOfEventNames();
            foreach (string eventName in eventsInDirectory)
            {
                // Delete the asset unless it is in the event list
                if (!eventList.Contains(eventName))
                {
                    Debug.Log("Deleting " + eventName);
                    File.Delete(EventListFileManager.GetEventAssetPath(eventName));
                    File.Delete(EventListFileManager.GetEventAssetPath(eventName) + EventListFileManager.META_FILE_EXTENSION);
                }
            }
            // Create the new files if they don't already exist
            foreach (string eventName in eventList)
            {
                // Create the asset if it doesn't already exist
                if (!eventsInDirectory.Contains(eventName))
                {
                    Debug.Log("Creating " + eventName);
                    AssetDatabase.CreateAsset(CreateInstance<GameEventIdentifier>(), EventListFileManager.GetEventAssetPath(eventName));
                }
            }
            // Iterate over the renames
            foreach (EventToRename eventToRename in renames)
            {
                Debug.Log("Renaming event " + eventToRename.OriginalEventName + " to " + eventToRename.RenamedEventName);
                RenameEventOccurancesInCSharpFiles(eventToRename);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        /// <summary>
        /// 
        /// 
        /// IMPORTANT NOTE:
        /// It is possible that this replaces things that it does not mean to if this something is set up in the following way:
        /// 
        /// class IrrelevantClassName
        /// {
        ///     public object EventName;
        /// }
        ///
        /// class ProblemClass
        /// {
        ///     public IrrelevantClassName EventIDList = new IrrelevantClassName();
        ///     public void Function()
        ///     {
        ///         // BELOW LINE WILL BE CHANGED BY THIS
        ///         EventIDList.EventName = null;
        ///     }
        /// }
        /// </summary>
        /// <param name="renameEvent"></param>
        private void RenameEventOccurancesInCSharpFiles(EventToRename renameEvent)
        {
            string[] scriptsToSearch = Directory.GetFiles("Assets", "*.cs", SearchOption.AllDirectories);
            // Get rid of meta files
            scriptsToSearch = EventListFileManager.RemoveMetaFiles(scriptsToSearch);
            for (int i = 0; i < scriptsToSearch.Length; ++i)
            {
                string text = File.ReadAllText(scriptsToSearch[i]);
                string searchForName = EventListFileManager.EVENTID_LIST_CLASS_NAME + "." + renameEvent.OriginalEventName;
                string replaceName = EventListFileManager.EVENTID_LIST_CLASS_NAME + "." + renameEvent.RenamedEventName;
                bool foundMatches = false;
                foreach (Match match in Regex.Matches(text, searchForName))
                {
                    int priorCharIndex = match.Index - 1;
                    int nextCharIndex = match.Index + searchForName.Length;
                    char priorChar = text[priorCharIndex];
                    char nextChar = text[nextCharIndex];
                    if (!char.IsLetterOrDigit(priorChar) && priorChar != '.' &&
                        !char.IsLetterOrDigit(nextChar))
                    {
                        text = text.Substring(0, priorCharIndex + 1) + replaceName + text.Substring(nextCharIndex);
                        foundMatches = true;
                    }
                }
                if (foundMatches)
                {
                    File.WriteAllText(scriptsToSearch[i], text);
                }
            }
        }
    }

    struct EventToRename
    {
        public string OriginalEventName => originalEventName;
        private string originalEventName;
        public string RenamedEventName => renamedEventName;
        private string renamedEventName;


        public EventToRename(string eventOriginalName, string eventRenamedName)
        {
            originalEventName = eventOriginalName;
            renamedEventName = eventRenamedName;
        }
    }
}