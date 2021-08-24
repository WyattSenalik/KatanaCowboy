using UnityEngine;

using SoundSystem.Internal;

namespace SoundSystem
{
    /// <summary>
    /// Instance created to play a 3D sound at a location.
    /// </summary>
    public class Sound3DInstance
    {
        // Parent objects and if they are initialized
        private static bool areParentObjectsInitialized = false;
        private static GameObject soundInstanceParent = null;
        private static GameObject persistentSoundInstanceParent = null;

        // Reference to the gameobject the audio is attached to
        public GameObject gameObject { get; private set; }


        /// <summary>
        /// Creates an instance of sound in 3D space.
        /// </summary>
        /// <param name="sound">Sound to play.</param>
        /// <param name="position">Position to play the sound.</param>
        /// <param name="isScenePersistent">If the sound should persist between scenes.</param>
        public Sound3DInstance(ISound sound, Vector3 position, bool isScenePersistent = false)
        {
            InitializeParentObjects();

            gameObject = new GameObject(sound.ToString() + " (Sound3DInstance)");
            gameObject.transform.position = position;
            if (isScenePersistent)
            {
                gameObject.transform.SetParent(persistentSoundInstanceParent.transform);
            }
            else
            {
                gameObject.transform.SetParent(soundInstanceParent.transform);
            }
        }


        /// <summary>
        /// Initializes the parent objects if they are not initialized yet.
        /// </summary>
        private static void InitializeParentObjects()
        {
            if (!areParentObjectsInitialized)
            {
                soundInstanceParent = new GameObject("Sound3DInstance Parent");
                persistentSoundInstanceParent = new GameObject("Sound3DInstance Parent (Persistent)");
                UnityEngine.Object.DontDestroyOnLoad(persistentSoundInstanceParent);

                areParentObjectsInitialized = true;
            }
        }
    }
}
