using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem.Internal
{
    /// <summary>
    /// Base class for handling object pooling for sounds.
    /// </summary>
    public abstract class BaseSoundPool : IBaseSoundPool
    {
        // GameObject and SoundInstanceDisposer for all sound pools to use to dispose of sound instances
        private static GameObject poolGameObject = null;
        protected static SoundInstanceDisposer soundInstanceDisposer { get; private set; }

        // Dictionary of all the in-use AudioSources
        protected readonly List<AudioSource> activeAudios = new List<AudioSource>();
        // Collection of not in-use AudioSources
        protected readonly Stack<AudioSource> idleAudios = new Stack<AudioSource>();


        protected BaseSoundPool(int startAudios = 0)
        {
            activeAudios = new List<AudioSource>();
            idleAudios = new Stack<AudioSource>(startAudios);

            InitializePoolGameObject();
        }
        private static void InitializePoolGameObject()
        {
            if (poolGameObject == null)
            {
                poolGameObject = new GameObject("SoundInstance Monitor");
                UnityEngine.Object.DontDestroyOnLoad(poolGameObject);
                soundInstanceDisposer = poolGameObject.AddComponent<SoundInstanceDisposer>();
            }
        }


        public void ReleaseAudioSource(AudioSource audioSource)
        {
            audioSource.clip = null;
            activeAudios.Remove(audioSource);
            idleAudios.Push(audioSource);
        }
        public void Clear(int size = 0)
        {
            while (idleAudios.Count > size)
            {
                DeleteIdleAudioSource(idleAudios.Pop());
            }
        }


        /// <summary>
        /// Gets an idle AudioSource from the list.
        /// If there are no idle AudioSources, creates a new one.
        /// Once returned, the AudioSource is no longer considered available.
        /// </summary>
        /// <returns>Available AudioSource.</returns>
        protected AudioSource GetAvailableAudioSource()
        {
            if (idleAudios.Count <= 0)
            {
                idleAudios.Push(CreateAudioSource());
            }

            return idleAudios.Pop();
        }
        /// <summary>
        /// Plays the given sound.
        /// </summary>
        /// <returns>Instance of the sounds being played.</returns>
        protected ISoundInstance Play(ISound sound)
        {
            AudioSource audioSource = GetAvailableAudioSource();
            audioSource.SetFromSound(sound);
            audioSource.Play();

            ISoundInstance soundInstance = new SoundInstance(audioSource, this);
            soundInstanceDisposer.MonitorSoundInstance(soundInstance);

            return soundInstance;
        }
        protected abstract AudioSource CreateAudioSource();
        protected abstract void DeleteIdleAudioSource(AudioSource audioSource);
    }
}