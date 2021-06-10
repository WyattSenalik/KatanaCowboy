using System.Collections.Generic;
using System.IO;

namespace GameEventSystem
{
    public static class EventListFileManager
    {
        public const string LIST_SAVE_PATH = "Assets/EventWindow";
        public const string EVENT_SAVE_PATH = LIST_SAVE_PATH + "/Events";

        public const string ASSET_FILE_EXTENSION = ".asset";
        public const string META_FILE_EXTENSION = ".meta";

        public const string EVENTID_LIST_CLASS_NAME = "EventIDList";
        private const string EVENTID_LIST_FILE_EXTENSION = ".cs";


        public static string[] GetListOfEventNames()
        {
            string[] rawFileNames = Directory.GetFiles(EVENT_SAVE_PATH);
            string[] rawNonMetaFileNames = RemoveMetaFiles(rawFileNames);
            string[] fileNames = RemovePathsFromRawFileNames(rawNonMetaFileNames);
            return RemoveExtensionsFromFileNames(fileNames);
        }
        public static string GetEventAssetPath(string eventName)
        {
            return EVENT_SAVE_PATH + "/" + eventName + ASSET_FILE_EXTENSION;
        }
        public static string GetFullFilePath()
        {
            return LIST_SAVE_PATH + "/" + EVENTID_LIST_CLASS_NAME + EVENTID_LIST_FILE_EXTENSION;
        }


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