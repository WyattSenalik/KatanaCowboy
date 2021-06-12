using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;


namespace GameEventSystem.CustomEditor
{
    /// <summary>
    /// Custom EditorWindow to create events and generate a script called EventIDList.
    /// </summary>
    public class EventWindow : EditorWindow
    {
        // Constants
        // Title of the window
        private const string WINDOW_TITLE = "Event Manager";
        // Max widths for style
        private const float BUTT_MAX_WIDTH = 100f;
        private const float EVENT_LABEL_MIN_WIDTH = 200f;

        // References to all the window's current events
        private List<string> eventList = new List<string>();
        // List of renames that occured so that we can search the file system to replace later
        private List<EventToRename> renames = new List<EventToRename>();
        // Variables that hold temporary information about renaming
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
            // If we are auto saving or we have just hit the save button, recreate the file
            if (isAutoSave || justHitSave)
            {
                InitializeEventList.CreateFile();
                // Reset that we just hit the save button
                justHitSave = false;
            }
        }
        // Called every GUI repaint call
        private void OnGUI()
        {
            // Some space from the top
            GUILayout.Space(10f);

            // Add the auto save or manual save stuff
            DisplaySaveOptions();
            GUILayout.Space(25f);

            // Add the add event button
            DisplayAddEventButton();
            GUILayout.Space(10f);

            // Event list
            DisplayEventList();
        }


        /// <summary>
        /// Displays the save button and the auto save checkbox.
        /// </summary>
        private void DisplaySaveOptions()
        {
            GUILayout.BeginHorizontal();
            {
                // Only have the save button enabled if auto save is off and there are changes
                GUI.enabled = !isAutoSave && !DoesEventListMatchFileSystem();
                {
                    // When the button is pressed
                    if (GUILayout.Button("Save", GUILayout.MaxWidth(BUTT_MAX_WIDTH)))
                    {
                        // Set that we just hit the save button to true
                        justHitSave = true;
                        // Apply any current changes that we have
                        ApplySavedChanges();
                    }
                }
                GUI.enabled = true;

                // Auto save checkbox
                GUILayout.Label("Auto-Save", GUILayout.MinWidth(60), GUILayout.ExpandWidth(false));
                bool prevSaveState = isAutoSave;
                isAutoSave = EditorGUILayout.Toggle(isAutoSave);
                // If we have just toggled auto save on start by applying any saved changes
                if (isAutoSave && !prevSaveState)
                {
                    ApplySavedChanges();
                }
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Displays the add event text box and button.
        /// </summary>
        private void DisplayAddEventButton()
        {
            GUILayout.BeginHorizontal();
            {
                // Text field to type the name of the event to add into
                textFieldEventName = EditorGUILayout.TextField("Event Name", textFieldEventName);

                // If the event name is invalid, append some info onto the button name
                string addEventButtName = "Add Event";
                bool isEventNameValid = IsValidEventName(textFieldEventName);
                if (!isEventNameValid)
                {
                    addEventButtName += " [Invalid Event Name]";
                }

                // If the event name is invalid, disable the add event button
                GUI.enabled = isEventNameValid;
                {
                    // If the button is pressed
                    if (GUILayout.Button(addEventButtName))
                    {
                        // Add the entered event
                        AddEvent(textFieldEventName);
                    }
                }
                GUI.enabled = true;
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Displays the current list of events.
        /// </summary>
        private void DisplayEventList()
        {
            // Create the style for the event textbox.
            GUIStyle eventTextFieldStyle = new GUIStyle(GUI.skin.box)
            {
                alignment = TextAnchor.MiddleLeft,
                padding = new RectOffset(5, 0, 3, 0)
            };

            GUILayout.BeginVertical();
            // Display for an area for each event
            for (int i = 0; i < eventList.Count; ++i)
            {
                GUILayout.BeginHorizontal();
                {
                    // If the current event is not the one being renamed
                    if (editableEventName != i)
                    {
                        // Display the text field as uninteractable
                        GUI.enabled = false;
                        GUILayout.TextField(eventList[i], eventTextFieldStyle, GUILayout.MinWidth(EVENT_LABEL_MIN_WIDTH), GUILayout.ExpandWidth(false));
                        GUI.enabled = true;
                        // If the rename button is pressed, allow the current event to be renamed
                        if (GUILayout.Button("Rename", GUILayout.MaxWidth(BUTT_MAX_WIDTH)))
                        {
                            AllowEventNameRename(i);
                        }
                        // If the delete button is pressed, delete the current event
                        if (GUILayout.Button("Delete", GUILayout.MaxWidth(BUTT_MAX_WIDTH)))
                        {
                            DeleteEvent(i);
                        }
                    }
                    // If the current event is currently being renamed
                    else
                    {
                        // Display the text field as interactable
                        renamedEventName = GUILayout.TextField(renamedEventName, eventTextFieldStyle, GUILayout.MinWidth(EVENT_LABEL_MIN_WIDTH), GUILayout.ExpandWidth(false));
                        // If the event name is invalid, disable the confirm button
                        GUI.enabled = IsValidEventName(renamedEventName);
                        {
                            // If the confirm button is pressed, rename the current event to the renamedEventName
                            if (GUILayout.Button("Confirm", GUILayout.MaxWidth(BUTT_MAX_WIDTH)))
                            {
                                RenameEvent(i);
                            }
                        }
                        GUI.enabled = true;
                        // IF the cancael button is pressed, don't rename the event and get its old name back
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

        /// <summary>
        /// Updates the window's event list to reflect the events that are saved in the file system.
        /// </summary>
        private void UpdateEventListFromFileSystem()
        {
            eventList = new List<string>(EventListFileManager.GetListOfEventNames());
        }
        /// <summary>
        /// Checks if the current event list is in sync with the events in the file system.
        /// </summary>
        /// <returns>True if they are in sync. False if they don't match in some way.</returns>
        private bool DoesEventListMatchFileSystem()
        {
            string[] fileEventNames = EventListFileManager.GetListOfEventNames();

            // If they don't have the same amount of events, they are out of sync
            if (eventList.Count != fileEventNames.Length)
            {
                return false;
            }

            // Check against each name. If the file system contains a name the event list doesn't have,
            // then they are out of sync.
            for (int i = 0; i < fileEventNames.Length; ++i)
            {
                string curFileEventName = fileEventNames[i];
                if (!eventList.Contains(curFileEventName))
                {
                    return false;
                }
            }

            // If they are the same size and the event list contains all the file event names, they match
            return true;
        }

        /// <summary>
        /// Starts allowing the user to rename the event name with the given index.
        /// </summary>
        /// <param name="index">Index of the event to allow rename for.</param>
        private void AllowEventNameRename(int index)
        {
            editableEventName = index;
            // Get a reference to the original name for later use
            originalEventName = eventList[index];
            // Start the rename of the event off by just being the original
            renamedEventName = originalEventName;
        }
        /// <summary>
        /// Stops allowing the user to rename an event name.
        /// </summary>
        private void CancelEventNameRename()
        {
            editableEventName = -1;
        }

        /// <summary>
        /// Adds the event to the event list and auto-saves if enabled.
        /// </summary>
        /// <param name="eventName">Event name to add.</param>
        private void AddEvent(string eventName)
        {
            // Add the event into the array
            eventList.Add(eventName);
            // Check for auto save
            CheckAutoSave();
        }
        /// <summary>
        /// Deletes the event with the given index from the event list and auto-saves if enabled.
        /// </summary>
        /// <param name="index">Index of the event name to delete. Assumed to be in range.</param>
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
        /// <summary>
        /// Renames the event with the given index using the class variables for its original and renamed names.
        /// </summary>
        private void RenameEvent(int index)
        {
            // Change the name of the event
            eventList[index] = renamedEventName;

            // Check if this rename is a rename of a previous rename
            string actualOriginalName = originalEventName;
            int renamedIndex = -1;
            for (int i = 0; i < renames.Count; ++i)
            {
                // If it is a rename of a previous rename, update the original name to a new rename.
                if (originalEventName == renames[i].RenamedEventName)
                {
                    actualOriginalName = renames[i].OriginalEventName;
                    renamedIndex = i;
                }
            }
            // If we found that it is a rename of the previous rename, then get rid of the old one.
            if (renamedIndex != -1)
            {
                renames.RemoveAt(renamedIndex);
                /* So I was going to do this, but there are so many edge cases, that I
                 * think it is just going to be easier to just force the user to save when they do a rename
                // Make sure that the rename did not cause any circular problems
                // This can occur if:
                //     1. Rename occurs: A -> C
                //     2. Rename occurs: B -> A
                //     3. Rename occurs: C -> B
                // In this situation nothing has actually changed, but we have two renames in the list:
                //     1. Rename in files: B -> A
                //     2. Rename in files: A -> A
                // 
                // A more complicated example of this problem:
                //     1. Rename occurs: A -> D
                //     2. Rename occurs: B -> A
                //     3. Rename occurs: C -> B
                //     4. Rename occurs: D -> C
                // In this situation again, nothing has changed, but we will rename everything that was B to C
                // due to the list looking like this:
                //     1. Rename in files: B -> A   This will change all B's to A's
                //     2. Rename in files: C -> B
                //     3. Rename in files: A -> C   This will then change all A's to C's including the previous B's
                //
                // To catch this problem, we will look if any renames whose replace name is
                // also the original name for any future renames. If that occurs, we will move the 
                // rename with the replace name to after the future rename with the original name.
                // We will also attempt to find if any renames try to rename them selves and remove them.
                //
                // First lets find those self renames
                int counter = 0;
                while (counter < renames.Count)
                {
                    EventToRename curRename = renames[counter];
                    if (curRename.OriginalEventName == curRename.RenamedEventName)
                    {
                        // Remove the self rename
                        renames.RemoveAt(counter);
                    }
                    // Only increment if we didn't remove
                    else
                    {
                        ++counter;
                    }
                }
                // Next lets find any renames whose replace name is the original name of a future event
                counter = 0;
                while (counter < renames.Count)
                {
                    EventToRename curRename = renames[counter];
                    for (int i = counter + 1; i < renames.Count; ++i)
                    {
                        EventToRename futureRename = renames[i];
                        if (curRename.RenamedEventName == futureRename.OriginalEventName)
                        {
                            // First make sure there is no siutation like this
                            //     1. A -> B
                            //     2. B -> A
                            if (curRename.OriginalEventName == futureRename.RenamedEventName)
                            {
                                // In this case, get rid of both renames (starting with the future one)
                                renames.RemoveAt(i);
                                renames.RemoveAt(counter);
                                break;
                            }
                            // If the situation is not that bad, just swap them TODO

                        }
                    }
                }
                */
            }
            // Add the new rename
            renames.Add(new EventToRename(actualOriginalName, renamedEventName));
            // Set the temporary variables to allow name editing
            CancelEventNameRename();
            // For renames we save regardless of auto saving or not and force the user to save
            justHitSave = true;
            ApplySavedChanges();
        }
        /// <summary>
        /// Makes a check if we are auto saving and applies saved changes if we are.
        /// </summary>
        /// <returns>True if we are autosaving, false otherwise.</returns>
        private bool CheckAutoSave()
        {
            if (isAutoSave)
            {
                ApplySavedChanges();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Creates and deletes events in the file structure to have them match the event list.
        /// Also searches the files in the file system for renames if needed.
        /// </summary>
        private void ApplySavedChanges()
        {
            // Confirm the directory exists
            if (!Directory.Exists(EventListFileManager.EVENT_SAVE_PATH))
            {
                Directory.CreateDirectory(EventListFileManager.EVENT_SAVE_PATH);
            }

            // Get rid of all the old files if they aren't in the new files
            string[] eventsInDirectory = EventListFileManager.GetListOfEventNames();
            foreach (string eventName in eventsInDirectory)
            {
                // Delete the asset unless it is in the event list
                if (!eventList.Contains(eventName))
                {
                    //Debug.Log("Deleting " + eventName);
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
                    //Debug.Log("Creating " + eventName);
                    AssetDatabase.CreateAsset(CreateInstance<GameEventIdentifier>(), EventListFileManager.GetEventAssetPath(eventName));
                }
            }
            // Iterate over the renames
            foreach (EventToRename eventToRename in renames)
            {
                //Debug.Log("Renaming event " + eventToRename.OriginalEventName + " to " + eventToRename.RenamedEventName);
                RenameEventOccurancesInCSharpFiles(eventToRename);
            }
            // Get rid of all the renames to reset for next time
            renames.Clear();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        /// <summary>
        /// Searches all of the files for the original name of the rename and changes all
        /// references to it to the renamed name of the rename.
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
        /// <param name="renameEvent">Contains the original name and replace name for the renamed event.</param>
        private void RenameEventOccurancesInCSharpFiles(EventToRename renameEvent)
        {
            // Get the names of all the scripts to search
            string[] scriptsToSearch = Directory.GetFiles("Assets", "*.cs", SearchOption.AllDirectories);
            // Get rid of meta files
            scriptsToSearch = EventListFileManager.RemoveMetaFiles(scriptsToSearch);
            // Iterate over all the scripts to search for the original name and replace it with the replace name
            for (int i = 0; i < scriptsToSearch.Length; ++i)
            {
                // Read the whole file
                string text = File.ReadAllText(scriptsToSearch[i]);
                string searchForName = EventListFileManager.EVENTID_LIST_CLASS_NAME + "." + renameEvent.OriginalEventName;
                string replaceName = EventListFileManager.EVENTID_LIST_CLASS_NAME + "." + renameEvent.RenamedEventName;
                bool foundMatches = false;
                bool keepEvaluating = true;
                // Find all matches to the search name (evaluating one at a time)
                while (keepEvaluating)
                {
                    // Assume this is the last evaluation
                    keepEvaluating = false;
                    // Evaluate the text for new matches
                    foreach (Match match in Regex.Matches(text, searchForName))
                    {
                        // Find the previous character and next character to the found match
                        int priorCharIndex = match.Index - 1;
                        int nextCharIndex = match.Index + searchForName.Length;
                        char priorChar = text[priorCharIndex];
                        char nextChar = text[nextCharIndex];
                        // If it is indeed a reference and not simply a random match (within the limits
                        // specified in the summary)
                        if (!char.IsLetterOrDigit(priorChar) && priorChar != '.' &&
                            !char.IsLetterOrDigit(nextChar))
                        {
                            // Replace the original name with its replacement
                            text = text.Substring(0, priorCharIndex + 1) + replaceName + text.Substring(nextCharIndex);
                            // Mark that we found a match and that we
                            // should keep evaluating and then reevaluate by breaking
                            foundMatches = true;
                            keepEvaluating = true;
                            break;
                        }
                    }
                }
                // If we found any matches, then write the changes we made to the file
                if (foundMatches)
                {
                    File.WriteAllText(scriptsToSearch[i], text);
                }
            }
        }
    }

    /// <summary>
    /// Holds the original name and replace/rename name of the event.
    /// </summary>
    struct EventToRename
    {
        // Original name of the event
        public string OriginalEventName => originalEventName;
        private string originalEventName;
        // Replace name of the event
        public string RenamedEventName => renamedEventName;
        private string renamedEventName;


        /// <summary>
        /// Creates an event to rename.
        /// </summary>
        /// <param name="eventOriginalName">Original name of the event.</param>
        /// <param name="eventRenamedName">Replace name of the event.</param>
        public EventToRename(string eventOriginalName, string eventRenamedName)
        {
            originalEventName = eventOriginalName;
            renamedEventName = eventRenamedName;
        }
    }
}