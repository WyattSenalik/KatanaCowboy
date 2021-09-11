using UnityEngine;

using SoundSystem.Internal;

namespace SoundSystem
{
    public static class SoundManager
    {
        private static ISound2DPool persistentFlatSoundPool = new Sound2DPool("Persistent 2DSound Pool", true);
        private static IMusicPool musicPool = new MusicPool("Music Pool");
        private static ISound2DPool flatSoundPool = new Sound2DPool("2DSound Pool", false);
        private static ISound3DPool sound3DPool = new Sound3DPool("3DSound Pool");


        /// <summary>
        /// Plays the given sound as a 2D sound.
        /// </summary>
        /// <param name="sound">Sound to play.</param>
        /// <returns>ISoundInstance - instance of the sound.</returns>
        public static ISoundInstance PlayFlat(ISound sound)
        {
            VerifyIsFlatSound(sound);

            ISound2DPool pool = ChooseSound2DPool(sound);
            return pool.Play(sound);
        }
        /// <summary>
        /// Destroys unused audiosources for 2D sounds.
        /// </summary>
        /// <param name="size">Amount of audiosources to keep.</param>
        public static void ClearFlat(int size = 0)
        {
            persistentFlatSoundPool.Clear(size);
            flatSoundPool.Clear(size);
        }

        /// <summary>
        /// Plays the given music clip.
        /// Stops any other music clips.
        /// </summary>
        /// <param name="music">Music clip to play.</param>
        public static void PlayMusic(ISound music)
        {
            VerifyIsFlatSound(music);

            musicPool.Play(music);
        }
        /// <summary>
        /// Pauses the current music playing (if any)
        /// </summary>
        public static void PauseMusic()
        {
            musicPool.Pause();
        }
        /// <summary>
        /// Restarts the current music playing.
        /// </summary>
        public static void RestartMusic()
        {
            musicPool.Restart();
        }

        /// <summary>
        /// Plays the given sound as a 3D sound in space at the given position.
        /// </summary>
        /// <param name="sound">Sound to play.</param>
        /// <param name="position">Position to play the sound at.</param>
        /// <returns>ISoundInstance - instance of the sound.</returns>
        public static ISoundInstance Play3D(ISound sound, Vector3 position)
        {
            VerifyIs3DSound(sound);

            return sound3DPool.Play(sound, position);
        }
        /// <summary>
        /// Destroys unused audiosources for 3D sounds.
        /// </summary>
        /// <param name="size">Amount of audiosources to keep.</param>
        public static void Clear3D(int size = 0)
        {
            sound3DPool.Clear(size);
        }


        #region Helpers
        private static void VerifyIsFlatSound(ISound sound)
        {
            if (sound.GetSpatialBlend() > 0.0f)
            {
                Debug.LogWarning($"{sound} is trying to be played as a Flat (2D) sound " +
                    $"despite it having a non-zero spatial blend.");
            }
        }
        private static void VerifyIs3DSound(ISound sound)
        {
            if (sound.GetSpatialBlend() <= 0.0f)
            {
                Debug.LogWarning($"{sound} is trying to be played as a 3D sound " +
                    $"despite it having a flat (2D) spatial blend.");
            }
        }
        private static ISound2DPool ChooseSound2DPool(ISound sound)
        {
            return sound.IsScenePersistent() ? persistentFlatSoundPool : flatSoundPool;
        }
        #endregion Helpers
    }
}