using System.Collections.Generic;
using System.IO;
using UnityEditor;


namespace GameEventSystem.CustomEditor
{
    /// <summary>
    /// Initializes the event list when the unity editor loads up.
    /// For this reason, it is highly recommended that the file be placed in the .gitignore.
    /// </summary>
    [InitializeOnLoad]
    public class InitializeEventList
    {
        // How often to update
        private static int updateFrequency = 100;
        private static int curFrequency = 0;
        // Record of what was last in the file system
        private static List<string> previousEvents = new List<string>();


        /// <summary>
        /// Called on unity load. Creates the file.
        /// </summary>
        static InitializeEventList()
        {
            EventListFileManager.CreateFile();

            EditorApplication.update += UpdateCheck;
        }

        /// <summary>
        /// Called every frame by EditorApplication.update.
        /// </summary>
        private static void UpdateCheck()
        {
            if (++curFrequency > updateFrequency)
            {
                curFrequency = 0;

                Update();
            }
        }
        /// <summary>
        /// Called once every updatefrequency frames.
        /// </summary>
        private static void Update()
        {
            UnityEngine.Debug.Log("Update");
            // Re-create the file if it has changed since last time
            string[] curEvents = EventListFileManager.CurrentEvents;
            foreach (string eventName in curEvents)
            {
                if (!previousEvents.Contains(eventName))
                {
                    EventListFileManager.CreateFile();
                    break;
                }
            }

            previousEvents = new List<string>(curEvents);
        }
    }
}
