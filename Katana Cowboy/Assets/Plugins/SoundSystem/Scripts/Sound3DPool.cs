using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoundSystem.Internal
{
    public class Sound3DPool : BaseSoundPool, ISound3DPool
    {
        private readonly GameObject parentObject = null;


        public Sound3DPool(string name = "Sound3D Pool")
        {
            parentObject = new GameObject(name);
            // When a scene is changed clean up lists
            SceneManager.activeSceneChanged += ResetOnSceneChange;
        }
        ~Sound3DPool()
        {
            UnityEngine.Object.Destroy(parentObject);
            SceneManager.activeSceneChanged -= ResetOnSceneChange;
        }


        #region ISound3DPool
        public ISoundInstance Play(ISound sound, Vector3 position)
        {
            CheckIsSound3D(sound);

            ISoundInstance soundInstance = Play(sound);
            soundInstance.gameObject.transform.position = position;

            return soundInstance;
        }
        #endregion ISound3DPool


        /// <summary>
        /// Creates a new gameobject, attached an AudioSource
        /// to it and returns that AudioSource.
        /// </summary>
        protected override AudioSource CreateAudioSource()
        {
            GameObject sourceObj = new GameObject("3D Audio Source");
            sourceObj.transform.SetParent(parentObject.transform);
            return sourceObj.AddComponent<AudioSource>();
        }
        /// <summary>
        /// Deletes the idle audio source and its gameobject.
        /// </summary>
        protected override void DeleteIdleAudioSource(AudioSource audioSource)
        {
            UnityEngine.Object.Destroy(audioSource.gameObject);
        }


        #region Helpers
        /// <summary>
        /// Clear the active and idle audio lists since they will be full of
        /// null references when the scene changes.
        /// </summary>
        private void ResetOnSceneChange(Scene current, Scene next)
        {
            activeAudios.Clear();
            idleAudios.Clear();
        }
        /// <summary>
        /// Checks if the given sound is actually meant to be 2D.
        /// </summary>
        private bool CheckIsSound3D(ISound sound)
        {
            if (sound.GetSpatialBlend() == 0)
            {
                Debug.LogWarning($"The given sound {sound} was specified as 2D," +
                    $" but is trying to be used as a 3D sound");
                return false;
            }
            return true;
        }
        #endregion Helpers
    }
}
