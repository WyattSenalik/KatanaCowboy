using System.Collections.Generic;
using System.IO;

namespace GameEventSystem.CustomEditor
{
    /// <summary>
    /// Class that holds much of the constant information about where to save the EventIDList.cs file.
    /// </summary>
    public static class EventListFileManager
    {
        private const string ROOT_SAVE_PATH = "Assets";
        private const string FOLDER_NAME = "EventWindow_DO_NOT_EDIT";
        private const string GENERATED_FOLDER_PATH = ROOT_SAVE_PATH + "/~_Generated/GameEventSystem";
        // Folder to save the EventIDList.cs file in
        public const string LIST_SAVE_PATH = GENERATED_FOLDER_PATH + "/" + FOLDER_NAME;
        // Folder to save the scriptable objects in
        public const string EVENT_SAVE_PATH = LIST_SAVE_PATH + "/Events";

        // File extensions for asset and meta data
        public const string ASSET_FILE_EXTENSION = ".asset";
        public const string META_FILE_EXTENSION = ".meta";

        // Name of the class/file name and the file's extension
        public const string EVENTID_LIST_CLASS_NAME = "EventIDList";
        private const string EVENTID_LIST_FILE_EXTENSION = ".cs";


        // Events that are currently in the EventIDList
        public static List<EventWithTypes> CurrentEvents => currentEventsInTheList;
        private static List<EventWithTypes> currentEventsInTheList = new List<EventWithTypes>();
        // If the EventIDList is currently being constructed
        private static bool isBeingCreated = false;


        /// <summary>
        /// Get a list of all the EventsWithTypes from the file system.
        /// </summary>
        public static List<EventWithTypes> GetListOfEventsWithTypes()
        {
            string[] soNames = GetListOfEventFileNames();
            List<EventWithTypes> events = new List<EventWithTypes>(soNames.Length);
            foreach (string name in soNames)
            {
                events.Add(GetEventWithTypesFromFileName(name));
            }
            return events;
        }
        /// <summary>
        /// Converts the given file name (for an event) into an event with types.
        /// </summary>
        /// <param name="file">Name of the scriptable object event id.</param>
        public static EventWithTypes GetEventWithTypesFromFileName(string file)
        {
            // Find the amount of breaks ($) there are
            int nameEndIndex = file.IndexOf('$');
            nameEndIndex = nameEndIndex != -1 ? nameEndIndex : file.Length;
            string name = file.Substring(0, nameEndIndex);

            // Find more breaks for each additional parameter
            List<string> paramStrings = new List<string>();
            if (nameEndIndex + 1 < file.Length)
            {
                file = file.Substring(nameEndIndex + 1);

                int curIndex = file.IndexOf('$');
                int infinityChecker = 20;
                int infinityCounter = 0;
                while (curIndex != -1 && infinityCounter < infinityChecker)
                {
                    // Pull off a paramter from the file name
                    string paramStr = file.Substring(0, curIndex);
                    paramStrings.Add(paramStr);

                    // Strink the file name to no longer include that pulled off parameter
                    file = file.Substring(curIndex + 1);

                    curIndex = file.IndexOf('$');

                    ++infinityCounter;
                }
                if (infinityCounter >= infinityChecker)
                {
                    UnityEngine.Debug.Log("Infinite loop detected");
                }

                // The file can be 0 here if there are no parameters
                if (file.Length > 0)
                {
                    // Add the last parameter
                    paramStrings.Add(file);
                }
            }

            return new EventWithTypes(name, paramStrings.ToArray());
        }
        /// <summary>
        /// Appends the data of EventWithTypes into a string to serve as the name of the scriptable objects.
        /// </summary>
        /// <param name="eventWithTypes">EventWithTypes to pull the name and paramters from.</param>
        public static string GetFileNameFromEventWithTypes(EventWithTypes eventWithTypes)
        {
            string str = eventWithTypes.Name;
            foreach (string paramName in eventWithTypes.ParamTypeNames)
            {
                str += '$' + paramName;
            }
            return str;
        }
        /// <summary>
        /// Gets a list of event names from the assets in the folder.
        /// </summary>
        /// <returns>List of event names from the file system.</returns>
        public static string[] GetListOfEventFileNames()
        {
            if (Directory.Exists(EVENT_SAVE_PATH))
            {
                string[] rawFileNames = Directory.GetFiles(EVENT_SAVE_PATH);
                string[] rawNonMetaFileNames = RemoveMetaFiles(rawFileNames);
                string[] fileNames = RemovePathsFromRawFileNames(rawNonMetaFileNames);
                return RemoveExtensionsFromFileNames(fileNames);
            }
            return new string[0];
        }
        /// <summary>
        /// Gets the path to the event asset with the given name.
        /// </summary>
        /// <param name="eventName">Name of the event to get the path of.</param>
        /// <returns>Path to the sriptable event.</returns>
        public static string GetEventAssetPath(string eventName)
        {
            return EVENT_SAVE_PATH + "/" + eventName + ASSET_FILE_EXTENSION;
        }
        /// <summary>
        /// Gets the full file path to the EventIDList2.cs.
        /// </summary>
        /// <returns>Full file path to EventIDList.cs with extension.</returns>
        public static string GetFullFilePath()
        {
            return LIST_SAVE_PATH + "/" + EVENTID_LIST_CLASS_NAME + EVENTID_LIST_FILE_EXTENSION;
        }

        /// <summary>
        /// Returns a copy of the given array, but with any meta files missing.
        /// </summary>
        /// <param name="rawFileNames">Array of file names to remove meta files from.</param>
        /// <returns>Array of file names with no meta files.</returns>
        public static string[] RemoveMetaFiles(string[] rawFileNames)
        {
            List<string> nonMetaFiles = new List<string>();
            for (int i = 0; i < rawFileNames.Length; ++i)
            {
                string curName = rawFileNames[i];
                if (curName[curName.Length - 1] != 'a')
                {
                    nonMetaFiles.Add(curName);
                }
            }
            return nonMetaFiles.ToArray();
        }

        /// <summary>
        /// Creates/updates the const static file with the event id's saved as const ints.
        /// </summary>
        public static void CreateFile()
        {
            if (isBeingCreated)
            {
                return;
            }
            isBeingCreated = true;

            // Delete any old versions of this file
            DeleteOldIDListFiles();

            // Beginning and end of the file
            const string FILE_BEGIN_TEXT = "// DO NOT EDIT DIRECTLY. GENERATED BY InitializeEventList.cs" + "\r\n" +
                "namespace GameEventSystem" + "\r\n" +
                "{" + "\r\n" +
                "public static class " + EVENTID_LIST_CLASS_NAME + "\r\n" +
                "{";
            const string FILE_END_TEXT = "}" + "\r\n" +
                "}";

            // Generate lines to write for each event
            currentEventsInTheList = GetListOfEventsWithTypes();
            string[] eventConstLines = GenerateEventConstantLines(currentEventsInTheList);
            string[] writeLines = new string[eventConstLines.Length + 2];
            // First line is the begin text
            writeLines[0] = FILE_BEGIN_TEXT;
            // All other lines are the event constants
            for (int i = 0; i < eventConstLines.Length; ++i)
            {
                writeLines[i + 1] = "    " + eventConstLines[i];
            }
            // Last line is the end text
            writeLines[writeLines.Length - 1] = FILE_END_TEXT;

            // Check if the path exists
            if (!Directory.Exists(LIST_SAVE_PATH))
            {
                Directory.CreateDirectory(LIST_SAVE_PATH);
            }

            // Write to the file
            File.WriteAllLines(GetFullFilePath(), writeLines);

            //AssetDatabase.SaveAssets();
            //AssetDatabase.Refresh();

            isBeingCreated = false;
        }


        /// <summary>
        /// Creates and returns an array of strings that will serve as lines in the
        /// generated script to be public const ints.
        /// </summary>
        /// <returns>Array of strings that can be used as lines in the generated script.</returns>
        private static string[] GenerateEventConstantLines(IReadOnlyList<EventWithTypes> eventsWithTypes)
        {
            string[] eventLines = new string[eventsWithTypes.Count];
            for (int i = 0; i < eventLines.Length; ++i)
            {
                EventWithTypes curEvent = eventsWithTypes[i];
                string eventID = curEvent.Name;
                List<string> eventParams = curEvent.ParamTypeNames;

                string eventParamStr = "";
                if (eventParams.Count > 0)
                {
                    eventParamStr += "<";
                    foreach (string eventP in eventParams)
                    {
                        eventParamStr += eventP + ", ";
                    }
                    eventParamStr = eventParamStr.Substring(0, eventParamStr.Length - 2);
                    eventParamStr += ">";
                }

                eventLines[i] = "public static readonly GameEventIdentifier" + eventParamStr + " " +
                    eventID + " = new GameEventIdentifier" + eventParamStr + "(\"" + eventID + "\");";
            }
            return eventLines;
        }

        /// <summary>
        /// Gets the file names (with extensions) from the full paths given.
        /// </summary>
        /// <param name="rawFileNames">Raw paths to the files.</param>
        /// <returns>Array of the file names.</returns>
        private static string[] RemovePathsFromRawFileNames(string[] rawFileNames)
        {
            string[] shorterNames = new string[rawFileNames.Length];
            for (int i = 0; i < rawFileNames.Length; ++i)
            {
                string curName = rawFileNames[i];
                int pathEndIndex = curName.LastIndexOf('\\');
                if (pathEndIndex == -1)
                {
                    pathEndIndex = curName.LastIndexOf('/');
                }
                shorterNames[i] = curName.Substring(pathEndIndex + 1);
            }
            return shorterNames;
        }
        /// <summary>
        /// Removes any extensions from the given file names. If a file has multiple extensions
        /// like .cs.meta it will only remove the .meta.
        /// </summary>
        /// <param name="fileNames">File names with extensions.</param>
        /// <returns>File names missing their extensions.</returns>
        private static string[] RemoveExtensionsFromFileNames(string[] fileNames)
        {
            string[] shorterNames = new string[fileNames.Length];
            for (int i = 0; i < fileNames.Length; ++i)
            {
                string curName = fileNames[i];
                int extStartIndex = curName.LastIndexOf('.');
                shorterNames[i] = curName.Substring(0, extStartIndex);
            }
            return shorterNames;
        }
        /// <summary>
        /// Deletes any old files for EventIDList.
        /// </summary>
        private static void DeleteOldIDListFiles()
        {
            // Search for the event id list in the file system and delete any files that are not
            // where they should be.
            string[] oldFiles = Directory.GetFiles(ROOT_SAVE_PATH, EVENTID_LIST_CLASS_NAME + EVENTID_LIST_FILE_EXTENSION);
            foreach (string file in oldFiles)
            {
                // Make sure it isn't the file in the correct place.
                if (file != GetFullFilePath())
                {
                    File.Delete(file);
                }
            }
        }
    }
}