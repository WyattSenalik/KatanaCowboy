
namespace SoundSystem
{
    // Options for each function
    public enum Play2DOptions { FirstNoInterupt, FirstInterupt, MultipleInstance }
    public enum Pause2DOptions { FirstPlaying, All }
    public enum Add2DOptions { SingleInstance, MultipleInstance }
    public enum GetRid2DOptions { First, All, FirstNonPlaying, AllNonPlaying }
}

namespace SoundSystem.Internal
{
    /// <summary>
    /// Interface for pooling objects for 2D sounds.
    /// </summary>
    public interface ISound2DPool
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
        bool Play(ISound sound, Play2DOptions option = Play2DOptions.FirstNoInterupt);
        /// <summary>
        /// Pauses the given sound.
        /// </summary>
        /// <param name="option">
        /// FirstPlaying (default) - pauses the first instance playing the sound.
        /// All - pauses all instances playing the sound.
        /// </param>
        /// <returns>If the sound was found and paused. False if the sound was not paused.</returns>
        bool Pause(ISound sound, Pause2DOptions option = Pause2DOptions.FirstPlaying);
        /// <summary>
        /// Adds the given sound to the pool.
        /// </summary>
        /// <param name="option">
        /// SingleInstance (default) - if the sound is already loaded, will not load a new instance.
        /// MultipleInstance - if the sound is already loaded, will create a new instance.
        /// </param>
        /// <returns>If the sound was added. False if no sound was added.</returns>
        bool Add(ISound sound, Add2DOptions option = Add2DOptions.SingleInstance);
        /// <summary>
        /// Releases the given sound from the active pool to the available pool.
        /// </summary>
        /// <param name="option">
        /// First (default) - removes the first instance of the loaded sound.
        /// All - removes all instances of the loaded sound.
        /// FirstNonPlaying - removes the first non playing instance of the loaded sound.
        /// AllNonPlaying - removes all instances of the loaded sound that are not playing.
        /// </param>
        /// <returns>If the sound was released. False if no sounds were releaseds.</returns>
        bool Release(ISound sound, GetRid2DOptions option = GetRid2DOptions.First);
        /// <summary>
        /// Removes the given sound from the pool.
        /// </summary>
        /// <param name="option">
        /// First (default) - removes the first instance of the loaded sound.
        /// All - removes all instances of the loaded sound.
        /// FirstNonPlaying - removes the first non playing instance of the loaded sound.
        /// AllNonPlaying - removes all instances of the loaded sound that are not playing.
        /// </param>
        /// <returns>If the sound was removed. False if no sound was removed.</returns>
        bool Remove(ISound sound, GetRid2DOptions option = GetRid2DOptions.First);
        /// <summary>
        /// Releases all the current sounds to make the audios available.
        /// </summary>
        void ReleaseAll();
        /// <summary>
        /// Removes all the added spunds.
        /// </summary>
        void RemoveAll();
    }
}