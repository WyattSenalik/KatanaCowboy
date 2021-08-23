using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem.Internal
{
    /// <summary>
    /// Base class for handling object pooling for sounds.
    /// </summary>
    /// <typeparam name="T">Key for the active AudioSource dictionary.</typeparam>
    public abstract class BaseSoundPool<T>: IBaseSoundPool
    {
        // Dictionary of all the in-use AudioSources
        protected Dictionary<T, List<AudioSource>> activeAudios = new Dictionary<T, List<AudioSource>>();
        // Collection of not in-use AudioSources
        protected Stack<AudioSource> idleAudios = new Stack<AudioSource>();

        
        protected BaseSoundPool(int startAudios = 0)
        {
            activeAudios = new Dictionary<T, List<AudioSource>>();
            idleAudios = new Stack<AudioSource>(startAudios);
        }
        ~BaseSoundPool()
        {
            activeAudios.Clear();
            idleAudios.Clear();
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
        protected abstract AudioSource CreateAudioSource();
    }
}