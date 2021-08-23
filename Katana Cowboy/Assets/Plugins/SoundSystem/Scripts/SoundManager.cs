using System;
using System.Collections.Generic;
using UnityEngine;

using SoundSystem.Internal;

namespace SoundSystem
{
    public static class SoundManager
    {
        private static ISound2DPool persistentFlatSoundPool = new Sound2DPool("Persistent 2DSound Pool", true);
        private static ISound2DPool musicPool = new Sound2DPool("Music Pool", true);
        private static ISound2DPool flatSoundPool = new Sound2DPool("2DSound Pool", false);


        public static bool Play(ISound sound)
        {
            throw new NotImplementedException();
        }
        public static bool Pause(ISound sound)
        {
            throw new NotImplementedException();
        }
        public static bool Load(ISound sound)
        {
            throw new NotImplementedException();
        }
        public static bool Release(ISound sound)
        {
            throw new NotImplementedException();
        }
        public static bool Unload(ISound sound)
        {
            throw new NotImplementedException();
        }
        public static bool PlayMusic(ISound music)
        {
            return musicPool.Play(music);
        }
        public static bool PauseMusic(ISound music)
        {
            return musicPool.Pause(music);
        }
        public static bool LoadMusic(ISound music)
        {
            return musicPool.Add(music);
        }
        public static bool ReleaseMusic(ISound music)
        {
            return musicPool.Release(music);
        }
        public static bool UnloadMusic(ISound music)
        {
            return musicPool.Remove(music);
        }
    }
}