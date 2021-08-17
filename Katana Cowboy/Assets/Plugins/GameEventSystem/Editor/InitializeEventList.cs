using System.Collections.Generic;
using System.Linq;
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
            // Re-create the file if it has changed since last time
            // Event names in the EventIDList.cs file
            List<EventWithTypes> listedEvents = EventListFileManager.CurrentEvents;
            // Event names in the file system            
            List<EventWithTypes> filSysEvents = EventListFileManager.GetListOfEventsWithTypes();
            foreach (EventWithTypes eventWithTypes in filSysEvents)
            {
                if (!listedEvents.Contains(eventWithTypes))
                {
                    EventListFileManager.CreateFile();
                    break;
                }
            }
        }
    }
}
