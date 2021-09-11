using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoundSystem.Internal
{
    /// <summary>
    /// Handles object pooling for 2D sounds.
    /// </summary>
    public class Sound2DPool : BaseSoundPool, ISound2DPool
    {
        // The gameobject holding all the AudioSources
        private readonly GameObject gameObject = null;
        // If the gameobject should be persistent through scenes
        private readonly bool shouldPersist = false;


        /// <summary>
        /// Constructs a Sound2DPool for managing AudioSources for Sounds.
        /// </summary>
        /// <param name="name">Name of the game object to create.</param>
        /// <param name="isScenePersistent">If the game object should be don't destroy on load.</param>
        /// <param name="startAudios">Amount of AudioSources to start with.</param>
        public Sound2DPool(string name = "Sound2DPool", bool isScenePersistent = false,
            int startAudios = 0) : base(startAudios)
        {
            // Create the object
            gameObject = new GameObject(name);
            
            // Handle what to do on a scene transition
            shouldPersist = isScenePersistent;
            if (isScenePersistent)
            {
                // Persist through scenes
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
            }
            else
            {
                // When a scene is changed clean up lists
                SceneManager.activeSceneChanged += ResetOnSceneChange;
            }

            // Add some empty audio sources
            for (int i = 0; i < startAudios; ++i)
            {
                CreateAudioSource();
            }
        }
        // Destructor
        ~Sound2DPool()
        {
            // Unsubscribe if we are subscribed
            if (!shouldPersist)
            {
                SceneManager.activeSceneChanged -= ResetOnSceneChange;
            }
            // Get rid of the gameobject
            UnityEngine.Object.Destroy(gameObject);
        }


        #region ISound2DPool
        public new ISoundInstance Play(ISound sound)
        {
            CheckIsSound2D(sound);

            return base.Play(sound);
        }
        #endregion ISound2DPool


        /// <summary>
        /// Creates a new audio source on the game object.
        /// </summary>
        /// <returns>AudioSource created.</returns>
        protected override AudioSource CreateAudioSource()
        {
            return gameObject.AddComponent<AudioSource>();
        }
        /// <summary>
        /// Destroys the given AudioSource.
        /// </summary>
        protected override void DeleteIdleAudioSource(AudioSource audioSource)
        {
            UnityEngine.Object.Destroy(audioSource);
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
        private void CheckIsSound2D(ISound sound)
        {
            if (sound.GetSpatialBlend() > 0)
            {
                Debug.LogWarning($"The given sound {sound} was specified as non-2D," +
                    $" but is trying to be used as a 2D sound");
            }
        }
        #endregion Helpers
    }
}