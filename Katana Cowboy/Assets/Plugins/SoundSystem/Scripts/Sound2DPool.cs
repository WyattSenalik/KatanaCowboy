using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoundSystem.Internal
{
    /// <summary>
    /// Handles object pooling for 2D sounds.
    /// </summary>
    public class Sound2DPool : BaseSoundPool<ISound>, ISound2DPool
    {
        // The gameobject holding all the AudioSources
        private GameObject gameObject = null;
        // If the gameobject should be persistent through scenes
        private bool shouldPersist = false;


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
        public bool Add(ISound sound, Add2DOptions option = Add2DOptions.SingleInstance)
        {
            CheckIsSound2D(sound);

            // Initialize a list in the dictionary if it doesn't currently have one
            if (!activeAudios.ContainsKey(sound))
            {
                activeAudios.Add(sound, new List<AudioSource>());
            }

            List<AudioSource> audioList = activeAudios[sound];
            // Handle what to do when there is already an instance in the currently active audio
            if (audioList.Count > 0)
            {
                switch (option)
                {
                    // Don't allow for another instance if it is single instance
                    case Add2DOptions.SingleInstance:
                        {
                            return false;
                        }
                    // Allow the addition of the new audio the the current list if
                    // multiple instances are allowed
                    case Add2DOptions.MultipleInstance:
                        {
                            break;
                        }
                    default:
                        {
                            PrintUnhandledCase(option);
                            return false;
                        }
                }
            }

            // Add the audio source
            AudioSource audio = GetAvailableAudioSource();
            audio.SetFromSound(sound);
            audioList.Add(audio);

            return true;
        }
        public bool Pause(ISound sound, Pause2DOptions option = Pause2DOptions.FirstPlaying)
        {
            CheckIsSound2D(sound);

            // Cannot pause audio that does not exist
            if (!activeAudios.ContainsKey(sound))
            {
                return false;
            }
            List<AudioSource> audioList = activeAudios[sound];
            // Cannot pause audio that does not exist
            if (audioList.Count == 0)
            {
                return false;
            }

            // Find all currently playing audio sources
            List<AudioSource> playingAudio = new List<AudioSource>();
            foreach (AudioSource source in audioList)
            {
                if (source.isPlaying)
                {
                    playingAudio.Add(source);
                }
            }
            // If there are no audio sources playing
            if (playingAudio.Count == 0)
            {
                return false;
            }
            // If at least 1 sound is active
            switch (option)
            {
                // Pause first AudioSource
                case Pause2DOptions.FirstPlaying:
                    {
                        playingAudio[0].Pause();
                        return true;
                    }
                // Pause all AudioSources
                case Pause2DOptions.All:
                    {
                        foreach (AudioSource source in playingAudio)
                        {
                            source.Pause();
                        }
                        return true;
                    }
                default:
                    {
                        PrintUnhandledCase(option);
                        return false;
                    }
            }
        }
        public bool Play(ISound sound, Play2DOptions option = Play2DOptions.FirstNoInterupt)
        {
            CheckIsSound2D(sound);

            // Try to add the sound before playing it.
            // This will not add a sound if it already exists.
            Add(sound);

            List<AudioSource> audioList = activeAudios[sound];
            switch (option)
            {
                // Play the first audio source if its not already playing
                case Play2DOptions.FirstNoInterupt:
                    {
                        if (!audioList[0].isPlaying)
                        {
                            audioList[0].Play();
                            return true;
                        }
                        return false;
                    }
                // Play the first audio source regardless of if its already playing or not
                case Play2DOptions.FirstInterupt:
                    {
                        audioList[0].Play();
                        return true;
                    }
                // Play the first source that isn't already playing or
                // create a new instance of the sound to play
                case Play2DOptions.MultipleInstance:
                    {
                        // Try to play the first source that isn't already playing
                        foreach (AudioSource source in audioList)
                        {
                            if (!source.isPlaying)
                            {
                                source.Play();
                                return true;
                            }
                        }
                        // If there were none, create one and play that
                        Add(sound, Add2DOptions.MultipleInstance);
                        audioList = activeAudios[sound];
                        audioList[audioList.Count - 1].Play();
                        return true;
                    }
                default:
                    {
                        PrintUnhandledCase(option);
                        return false;
                    }
            }
        }
        public bool Release(ISound sound, GetRid2DOptions option = GetRid2DOptions.First)
        {
            // When found, add the AudioSource to the avaible audio sources
            return ReleaseOrRemove(sound, option, (AudioSource source) =>
            {
                source.clip = null;
                idleAudios.Push(source);
            });
        }
        public bool Remove(ISound sound, GetRid2DOptions option = GetRid2DOptions.First)
        {
            // When found, delete the AudioSource
            return ReleaseOrRemove(sound, option, (AudioSource source) => UnityEngine.Object.Destroy(source));
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
        /// <summary>
        /// Prints and error message for the given enum.
        /// </summary>
        private void PrintUnhandledCase(Enum e)
        {
            Debug.LogError($"Unhandled {e.GetType().Name} of {e}");
        }
        /// <summary>
        /// Release and Remove are very similar with the only difference being what is
        /// done with the AudioSource once it is found.
        /// This allows an action to be defined for what to do with it so that
        /// both Release and Remove can simply call this function.
        /// </summary>
        /// <param name="sound">Sound to find the AudioSource for.</param>
        /// <param name="option">Specification for which AudioSource(s) to try and find.</param>
        /// <param name="action">What to do with the AudioSource once found.</param>
        /// <returns>True if the AudioSource was found.</returns>
        private bool ReleaseOrRemove(ISound sound, GetRid2DOptions option, Action<AudioSource> action)
        {
            // Can't find it if there are none
            if (!activeAudios.ContainsKey(sound))
            {
                return false;
            }
            List<AudioSource> audioList = activeAudios[sound];
            // Can't find it if there are none
            if (audioList.Count == 0)
            {
                return false;
            }

            AudioSource source;
            switch (option)
            {
                // Remove/Release the first AudioSource
                case GetRid2DOptions.First:
                    {
                        source = audioList[0];
                        action?.Invoke(source);
                        audioList.RemoveAt(0);
                        return true;
                    }
                // Remove/Release all the AudioSources
                case GetRid2DOptions.All:
                    {
                        foreach (AudioSource singleSource in audioList)
                        {
                            action?.Invoke(singleSource);
                        }
                        audioList.Clear();
                        return true;
                    }
                // Remove/Release only the first non-playing AudioSource
                case GetRid2DOptions.FirstNonPlaying:
                    {
                        for (int i = 0; i < audioList.Count; ++i)
                        {
                            source = audioList[i];
                            if (!source.isPlaying)
                            {
                                action?.Invoke(source);
                                audioList.RemoveAt(i);
                                break;
                            }
                        }
                        return true;
                    }
                // Remove/Release all the non-playing AudioSource
                case GetRid2DOptions.AllNonPlaying:
                    {
                        int curIndex = 0;
                        bool atLeastOneRemoved = false;
                        while (curIndex < audioList.Count)
                        {
                            source = audioList[curIndex];
                            if (!source.isPlaying)
                            {
                                action?.Invoke(source);
                                audioList.RemoveAt(curIndex);
                                atLeastOneRemoved = true;
                            }
                            else
                            {
                                ++curIndex;
                            }
                        }
                        return atLeastOneRemoved;
                    }
                default:
                    {
                        PrintUnhandledCase(option);
                        return false;
                    }
            }
        }
        #endregion Helpers
    }
}