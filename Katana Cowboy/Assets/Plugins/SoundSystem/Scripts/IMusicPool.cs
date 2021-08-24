
namespace SoundSystem.Internal
{
    public interface IMusicPool
    {
        /// <summary>
        /// Plays the music sound.
        /// </summary>
        void Play(ISound music);
        /// <summary>
        /// Pauses the current music track.
        /// </summary>
        void Pause();
        /// <summary>
        /// Adds the given music to the pool.
        /// </summary>
        /// <returns>If the music was added. False if no sound was added.</returns>
        bool Add(ISound music);
        /// <summary>
        /// Releases the given music from the active pool to the available pool.
        /// </summary>
        /// <returns>If the music was released. False if no music was releases.</returns>
        bool Release(ISound music);
        /// <summary>
        /// Removes the given music from the pool.
        /// </summary>
        /// <returns>If the music was removed. False if no music was removed.</returns>
        bool Remove(ISound music);
    }
}
