
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
        /// Restarts the currently playing music track.
        /// </summary>
        void Restart();
    }
}
