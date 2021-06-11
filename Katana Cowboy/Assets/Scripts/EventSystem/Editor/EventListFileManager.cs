using System.Collections.Generic;
using System.IO;

namespace GameEventSystem.CustomEditor
{
    /// <summary>
    /// Class that holds much of the constant information about where to save the EventIDList.cs file.
    /// </summary>
    public static class EventListFileManager
    {
        // Folder to save the EventIDList.cs file in
        public const string LIST_SAVE_PATH = "Assets/EventWindow_DO_NOT_EDIT";
        // Folder to save the scriptable objects in
        public const string EVENT_SAVE_PATH = LIST_SAVE_PATH + "/Events";

        // File extensions for asset and meta data
        public const string ASSET_FILE_EXTENSION = ".asset";
        public const string META_FILE_EXTENSION = ".meta";

        // Name of the class/file name and the file's extension
        public const string EVENTID_LIST_CLASS_NAME = "EventIDList";
        private const string EVENTID_LIST_FILE_EXTENSION = ".cs";


        /// <summary>
        /// Gets a list of event names from the assets in the folder.
        /// </summary>
        /// <returns>List of event names from the file system.</returns>
        public static string[] GetListOfEventNames()
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
    }
}