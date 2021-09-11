using UnityEngine;

namespace SoundSystem.Internal
{
    /// <summary>
    /// Interface for the base class for handling object pooling for sounds.
    /// </summary>
    public interface IBaseSoundPool
    {
        /// <summary>
        /// Releases the given audio source from this pool's
        /// active audio sources and adds it to the idle audio sources.
        /// </summary>
        void ReleaseAudioSource(AudioSource audioSource);
        /// <summary>
        /// Destroys audio sources from the idle audio sources reserve.
        /// </summary>
        /// <param name="size">Amount of AudioSources to leave untouched.</param>
        void Clear(int size = 0);
    }
}