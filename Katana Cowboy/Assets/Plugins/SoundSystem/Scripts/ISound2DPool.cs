
namespace SoundSystem.Internal
{
    /// <summary>
    /// Interface for pooling objects for 2D sounds.
    /// </summary>
    public interface ISound2DPool : IBaseSoundPool
    {
        /// <summary>
        /// Plays the given sound.
        /// </summary>
        /// <param name="option"> Option for handling what to do if the sound is already playing.
        /// FirstNoInterupt (default) - if the sound is already loaded and playing, will not restart the clip.
        /// FirstInterupt - if the sound is already loaded and playing, will restart it.
        /// MultipleInstance - if the sound is already loaded and playing, will create a new instance
        /// of the sound and start playing that new instance.
        /// </param>
        /// <returns>If the sound was successfully played. False if the sound was not played.</returns>
        ISoundInstance Play(ISound sound);
    }
}