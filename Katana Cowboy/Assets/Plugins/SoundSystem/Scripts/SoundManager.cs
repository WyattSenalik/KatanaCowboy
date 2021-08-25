using System;
using System.Collections.Generic;
using UnityEngine;

using SoundSystem.Internal;

namespace SoundSystem
{
    public static class SoundManager
    {
        private static ISound2DPool persistentFlatSoundPool = new Sound2DPool("Persistent 2DSound Pool", true);
        private static IMusicPool musicPool = new MusicPool("Music Pool");
        private static ISound2DPool flatSoundPool = new Sound2DPool("2DSound Pool", false);


        #region FlatSound
        public static bool PlayFlat(ISound sound, Play2DOptions option = Play2DOptions.FirstNoInterupt)
        {
            VerifyIsFlatSound(sound);

            ISound2DPool pool = ChooseSound2DPool(sound);
            return pool.Play(sound, option);
        }
        public static bool PauseFlat(ISound sound, Pause2DOptions option = Pause2DOptions.FirstPlaying)
        {
            VerifyIsFlatSound(sound);

            ISound2DPool pool = ChooseSound2DPool(sound);
            return pool.Pause(sound, option);
        }
        public static bool LoadFlat(ISound sound, Add2DOptions option = Add2DOptions.SingleInstance)
        {
            VerifyIsFlatSound(sound);

            ISound2DPool pool = ChooseSound2DPool(sound);
            return pool.Add(sound, option);
        }
        public static bool ReleaseFlat(ISound sound, GetRid2DOptions option = GetRid2DOptions.First)
        {
            VerifyIsFlatSound(sound);

            ISound2DPool pool = ChooseSound2DPool(sound);
            return pool.Release(sound, option);
        }
        public static bool UnloadFlat(ISound sound, GetRid2DOptions option = GetRid2DOptions.First)
        {
            VerifyIsFlatSound(sound);

            ISound2DPool pool = ChooseSound2DPool(sound);
            return pool.Remove(sound, option);
        }
        #endregion FlatSound


        #region Music
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
        /// Loads the given music clip.
        /// Ends any previous music clips.
        /// </summary>
        /// <param name="music">Music to prepare to play.</param>
        /// <returns>If the music has been successfully loaded.</returns>
        public static bool LoadMusic(ISound music)
        {
            VerifyIsFlatSound(music);

            return musicPool.Add(music);
        }
        /// <summary>
        /// Releases the current music audio back to available music audio.
        /// </summary>
        /// <param name="music">Music to release.</param>
        /// <returns>If the music was successfully released.</returns>
        public static bool ReleaseMusic(ISound music)
        {
            return musicPool.Release(music);
        }
        /// <summary>
        /// Unloads the current music audio source.
        /// </summary>
        /// <param name="music">Music to unload.</param>
        /// <returns>If the music was successfully unloaded.</returns>
        public static bool UnloadMusic(ISound music)
        {
            return musicPool.Remove(music);
        }
        #endregion Music


        #region 3DSound
        public static Sound3DInstance Play3D(ISound sound, Vector3 position)
        {
            throw new NotImplementedException();
        }
        public static bool Pause3D(Sound3DInstance soundInstance)
        {
            throw new NotImplementedException();
        }
        public static Sound3DInstance Load3D(ISound sound, Vector3 position)
        {
            throw new NotImplementedException();
        }
        public static bool Release3D(Sound3DInstance soundInstance)
        {
            throw new NotImplementedException();
        }
        public static bool Unload3D(Sound3DInstance soundInstance)
        {
            throw new NotImplementedException();
        }
        #endregion 3DSound


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
    }
}