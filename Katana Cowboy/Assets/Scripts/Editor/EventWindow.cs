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

        // Reference to the active window
        private static EditorWindow windowReference = null;
        private static EditorWindow WindowRef
        {
            get
            {
                if (windowReference == null)
                {
                    windowReference = GetWindow<EventWindow>(WINDOW_TITLE);
                }
                return windowReference;
            }
        }

        // References to all the existing events
        [SerializeField] private string[] eventList = new string[0];
        private int editableEventName = -1;
        private string originalEventName = "";
        private string renamedEventName = "";

        // Text field name for new event
        private string newEventName = "";


        /// <summary>
        /// Opens the editor window from the menu.
        /// </summary>
        [MenuItem("Window/" + WINDOW_TITLE)]
        public static void ShowWindow()
        {
            windowReference = GetWindow<EventWindow>(WINDOW_TITLE);
        }

        // Called as the new window is opened
        private void Awake()
        {
            UpdateEventListFromFileSystem();
        }
        // Called when creating, renaming, or reparenting assets, as well as moving or renaming folders in the project
        private void OnProjectChange()
        {
            UpdateEventListFromFileSystem();
            InitializeEventList.CreateFile();
        }
        // Called every GUI repaint call
        private void OnGUI()
        {
            // Get our target to be this and get a serialized property of the event list
            ScriptableObject target = this;
            SerializedObject serializedTarget = new SerializedObject(target);
            SerializedProperty serializedEventList = serializedTarget.FindProperty("eventList");
            //SerializedProperty serializedFilePath = serializedTarget.FindProperty("filePath");
            //SerializedProperty serializedClassName = serializedTarget.FindProperty("eventIDListClassName");

            GUILayout.Space(10f);
            // Add the add event button
            DisplayAddEventButton();
            // Give a little space for the list to get away from the button
            GUILayout.Space(10f);
            // Two strings for the path and name of the class
            //EditorGUILayout.PropertyField(serializedFilePath);
            //EditorGUILayout.PropertyField(serializedClassName);
            // Event list
            DisplayEventList();



            // Apply any modifications
            serializedTarget.ApplyModifiedProperties();
        }


        private void DisplayAddEventButton()
        {
            newEventName = EditorGUILayout.TextField("Event Name", newEventName);

            string addEventButtName = "Add Event";
            bool isEventNameValid = IsValidEventName(newEventName);
            GUI.enabled = isEventNameValid;
            if (!isEventNameValid)
            {
                addEventButtName += " [Invalid Event Name]";
            }

            if (GUILayout.Button(addEventButtName))
            {
                if (!Directory.Exists(EventListFileManager.EVENT_SAVE_PATH))
                {
                    Directory.CreateDirectory(EventListFileManager.EVENT_SAVE_PATH);
                }
                AssetDatabase.CreateAsset(new GameEventIdentifier(), EventListFileManager.GetEventAssetPath(newEventName));
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            GUI.enabled = true;
        }

        private void DisplayEventList()
        {
            GUIStyle eventLabelStyle = new GUIStyle(GUI.skin.box)
            {
                alignment = TextAnchor.MiddleLeft,
                padding = new RectOffset(5, 0, 3, 0)
            };
            GUILayout.BeginVertical();
            for (int i = 0; i < eventList.Length; ++i)
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
            for (int k = 0; k < eventList.Length; ++k)
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
            eventList = EventListFileManager.GetListOfEventNames();
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

        private void RenameEvent(int index)
        {
            // IMPORTANT NOTE:
            // It is possible that this replaces things that it does not mean to if this something is set up in the following way:
            // 
            // class IrrelevantClassName
            // {
            //     public object EventName;
            // }
            //
            // class ProblemClass
            // {
            //     public IrrelevantClassName EventIDList = new IrrelevantClassName();
            //     public void Function()
            //     {
            //         // BELOW LINE WILL BE CHANGED BY THIS
            //         EventIDList.EventName = null;
            //     }
            // }


            string[] scriptsToSearch = Directory.GetFiles("Assets", "*.cs", SearchOption.AllDirectories);
            Debug.Log("Amount of scripts to search " + scriptsToSearch.Length);
            // Get rid of meta files
            scriptsToSearch = EventListFileManager.RemoveMetaFiles(scriptsToSearch);
            for (int i = 0; i < scriptsToSearch.Length; ++i)
            {
                Debug.Log("Searching script " + scriptsToSearch);
                string text = File.ReadAllText(scriptsToSearch[i]);
                string searchForName = EventListFileManager.EVENTID_LIST_CLASS_NAME + "." + originalEventName;
                string replaceName = EventListFileManager.EVENTID_LIST_CLASS_NAME + "." + renamedEventName;
                bool foundMatches = false;
                foreach (Match match in Regex.Matches(text, searchForName))
                {
                    int priorCharIndex = match.Index - 1;
                    char priorChar = text[priorCharIndex];
                    char nextChar = text[match.Index + searchForName.Length];
                    if (!char.IsLetterOrDigit(priorChar) && priorChar != '.' &&
                        !char.IsLetterOrDigit(nextChar))
                    {
                        text = text.Substring(0, priorChar + 1) + replaceName + text.Substring(nextChar);
                        foundMatches = true;
                    }
                }
                if (foundMatches)
                {
                    File.WriteAllText(scriptsToSearch[i], text);
                }
            }


            // Rename the file
            string originalEventAssetPath = EventListFileManager.GetEventAssetPath(originalEventName);
            string renamedEventAssetPath = EventListFileManager.GetEventAssetPath(renamedEventName);
            try
            {
                File.Copy(originalEventAssetPath, renamedEventAssetPath);
                File.Copy(originalEventAssetPath + EventListFileManager.META_FILE_EXTENSION, renamedEventAssetPath + EventListFileManager.META_FILE_EXTENSION);

                DeleteEvent(index);
            }
            catch
            {
                Debug.LogError("Failed to rename the event (" + originalEventName + " -> " + renamedEventName + ")");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            CancelEventNameRename();
        }

        private void DeleteEvent(int index)
        {
            string eventToDelete = eventList[index];
            string eventAssetPath = EventListFileManager.GetEventAssetPath(eventToDelete);
            try
            {
                File.Delete(eventAssetPath);
                File.Delete(eventAssetPath + EventListFileManager.META_FILE_EXTENSION);
            }
            catch
            {
                Debug.LogError("Failed to delete the event (" + eventToDelete + ")");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        

        /*
        /// <summary>
        /// Places the apply changes button.
        /// </summary>
        private void DisplayApplyChangesButton()
        {
            // Update the apply changes button as well as the window title if there are changes
            string windowTitle;
            string applyChangesButtName;
            GUI.enabled = GetValidChangeStates(out windowTitle, out applyChangesButtName);
            WindowRef.titleContent.text = windowTitle;


            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            // Attempt to create the file from the event list
            if (GUILayout.Button(applyChangesButtName))
            {
                // Make sure the event list is valid
                if (IsEventListValid() == -1)
                {
                    // Update the previous list to reflect the new one
                    UpdatePrevious();

                    // Create the file and refresh the data base
                    CreateFile();
                    AssetDatabase.Refresh();
                }
                else
                {
                    Debug.LogError("Cannot create events that share a name or id");
                }
            }
            EditorGUILayout.EndHorizontal();
            // Stop any disabling from the button
            GUI.enabled = true;
        }

        /// <summary>
        /// Gets the states of the window title and the apply button name.
        /// Returns if there are changes and if they are valid.
        /// </summary>
        /// <param name="windowTitle">Window title to return.</param>
        /// <param name="applyChangesButtonName">Name of the Apply Changes button to return.</param>
        /// <returns>True if there are changes and they are valid, false otherwise.</returns>
        private bool GetValidChangeStates(out string windowTitle, out string applyChangesButtonName)
        {
            bool allChangesAreValid = false;

            // Enable the button and show a star in the title if there are changes
            bool areChanges = HasEventListChanged();
            applyChangesButtonName = "Apply Changes";
            windowTitle = WINDOW_TITLE;
            if (areChanges)
            {
                int invalidIndex = IsEventListValid();
                // If we have changes, make sure those changes are valid
                bool areEventsValid = invalidIndex == -1;
                if (areEventsValid)
                {
                    windowTitle = "(*) " + WINDOW_TITLE;
                    allChangesAreValid = true;
                }
                // Invalid changes means disable the button and let the user know there are invalid events
                else
                {
                    windowTitle = "(!!!) " + WINDOW_TITLE;
                    applyChangesButtonName += " [Invalid Events Exist] [" + eventList[invalidIndex] +
                        " at index=" + invalidIndex + "]";
                }
            }
            // Make sure that the class name and file path are also valid
            if (!IsClassNameValid())
            {
                Debug.LogError("EventWindow's event list class name is invalid. (Window->EventWindow)");
                windowTitle = "(!!!) " + WINDOW_TITLE;
                allChangesAreValid = false;
            }
            if (!IsFilePathValid())
            {
                Debug.LogError("EventWindow's filepath is invalid. (Window->EventWindow)");
                windowTitle = "(!!!) " + WINDOW_TITLE;
                allChangesAreValid = false;
            }

            return allChangesAreValid;
        }

        /// <summary>
        /// Checks if the event list has changed at all.
        /// </summary>
        /// <returns>True if the event list has changed, false otherwise.</returns>
        private bool HasEventListChanged()
        {
            // If the lists are different sizes, the list has changed
            if (eventList.Length != previousEventList.Length)
            {
                return true;
            }
            for (int i = 0; i < eventList.Length; ++i)
            {
                // If they aren't the same, the list has changed
                if (eventList[i] != previousEventList[i])
                {
                    return true;
                }
            }
            // If the file path or class name changed, there are changes
            if (eventIDListClassName != previousClassName)
            {
                return true;
            }
            if (filePath != previousFilePath)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Updates the previous event list to be the current event list, the previous class name, and
        /// the previous file path.
        /// </summary>
        private void UpdatePrevious()
        {
            // Before we update the previous delete it
            DeletePreviousFile();
            // Now update
            previousEventList = new string[eventList.Length];
            for (int i = 0; i < eventList.Length; ++i)
            {
                previousEventList[i] = eventList[i];
            }
            previousClassName = eventIDListClassName;
            previousFilePath = filePath;
        }

        /// <summary>
        /// Checks if the current event list is valid.
        /// The event list is invalid if:
        ///     1. An event name is ""
        ///     2. An event name starts with a number.
        ///     3. An event name contains non-alphanumeric characters.
        ///     4. Multiple events share names.
        /// </summary>
        /// <returns>Returns the index of the first invalid event in the eventList.
        /// Returns -1 if the list is valid.</returns>
        private int IsEventListValid()
        {
            Regex alphanumericRegex = new Regex("^[a-zA-Z][a-zA-Z0-9]*$");
            for (int i = 0; i < eventList.Length; ++i)
            {
                // Make sure the string is not empty
                if (eventList[i].Length < 1)
                {
                    return i;
                }
                // Check if the first character is a number
                if (char.IsDigit(eventList[i][0]))
                {
                    return i;
                }
                // Check if the event name contains non-alphanumeric characters
                if (!eventList[i].All(char.IsLetterOrDigit))
                {
                    return i;
                }
                // Check if any events share the same name
                for (int k = i + 1; k < eventList.Length; ++k)
                {
                    if (eventList[i] == eventList[k])
                    {
                        return k;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Checks if the current class name is valid.
        /// The class name is invalid if:
        ///     1. Its an empty string ("")
        ///     2. It starts with a number.
        ///     3. It contains non-alphanumeric characters.
        /// <returns>True if the class name is valid. False if found invalid.</returns>
        private bool IsClassNameValid()
        {
            Regex alphanumericRegex = new Regex("^[a-zA-Z][a-zA-Z0-9]*$");
            // Make sure it isn't an empty string
            if (eventIDListClassName.Length < 1)
            {
                return false;
            }
            // Check if the first character is a number
            if (char.IsDigit(eventIDListClassName[0]))
            {
                return false;
            }
            // Check if the event name contains non-alphanumeric characters
            if (!eventIDListClassName.All(char.IsLetterOrDigit))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if the file path is valid, false otherwise.
        /// </summary>
        /// <returns></returns>
        private bool IsFilePathValid()
        {
            // If the directory exists, its obviously valid
            if (Directory.Exists(filePath))
            {
                return true;
            }

            // Check for invalid characters in the file path
            char[] invalidChars = Path.GetInvalidPathChars();
            for (int i = 0; i < invalidChars.Length; ++i)
            {
                if (filePath.Contains(invalidChars[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Deletes the previous file and the file path to it.
        /// </summary>
        private void DeletePreviousFile()
        {
            if (File.Exists(GetFullPreviousFilePath()))
            {
                File.Delete(GetFullPreviousFilePath());
                File.Delete(GetFullPreviousFilePath() + ".meta");
                Directory.Delete(previousFilePath, true);
            }
            else
            {
                Debug.LogWarning("No path to the previous files were found");
            }
        }

        /// <summary>
        /// Creates/updates the const static file with the event id's saved as const ints.
        /// </summary>
        private void CreateFile()
        {
            string FILE_BEGIN_TEXT = "// DO NOT EDIT DIRECTLY. GENERATED BY EventWindow.cs" + "\r\r" +
            "public static class " + eventIDListClassName + "\r" +
            "{";
            const string FILE_END_TEXT = "}";

            // Generate lines to write for each event
            string[] eventConstLines = GenerateEventConstantLines();
            string[] writeLines = new string[eventConstLines.Length + 2];
            writeLines[0] = FILE_BEGIN_TEXT;
            for (int i = 0; i < eventConstLines.Length; ++i)
            {
                writeLines[i + 1] = "    " + eventConstLines[i];
            }
            writeLines[writeLines.Length - 1] = FILE_END_TEXT;

            // Check if the path exists
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            File.WriteAllLines(GetFullCurrentFilePath(), writeLines);
        }

        /// <summary>
        /// Creates and returns an array of strings that will serve as lines in the
        /// generated script to be public const ints.
        /// </summary>
        /// <returns>Array of strings that can be used as lines in the generated script.</returns>
        private string[] GenerateEventConstantLines()
        {
            string[] eventLines = new string[eventList.Length];
            for (int i = 0; i < eventLines.Length; ++i)
            {
                string eventName = eventList[i];
                string eventID = i.ToString();
                eventLines[i] = "public const int " + eventName + " = " + eventID + ";";
            }
            return eventLines;
        }

        /// <summary>
        /// Gets the full file path using the current file path and class name.
        /// </summary>
        /// <returns></returns>
        private string GetFullCurrentFilePath()
        {
            return GetFullPath(filePath, eventIDListClassName);
        }
        /// <summary>
        /// Gets the full file path using the previous file paht and class name.
        /// </summary>
        /// <returns></returns>
        private string GetFullPreviousFilePath()
        {
            return GetFullPath(previousFilePath, previousClassName);
        }
        /// <summary>
        /// Gets the full path to the file with the file name as className.
        /// The .cs extension is added in this method.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        /// <param name="className"></param>
        /// <returns></returns>
        private string GetFullPath(string path, string className)
        {
            return path + "/" + className + ".cs";
        }
        */
    }
}