using UnityEngine;

namespace SoundSystem.Internal
{
    /// <summary>
    /// Interface for pooling objects for 3D sounds.
    /// </summary>
    public interface ISound3DPool : IBaseSoundPool
    {
        /// <summary>
        /// Plays the given sound.
        /// </summary>
        /// <param name="position">Position to play the sound at.</param>
        /// <returns>The instance of the sound being played.</returns>
        ISoundInstance Play(ISound sound, Vector3 position);
    }
}