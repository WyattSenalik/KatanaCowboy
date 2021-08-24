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


        // Flat
        public static bool PlayFlat(ISound sound)
        {
            throw new NotImplementedException();
        }
        public static bool PauseFlat(ISound sound)
        {
            throw new NotImplementedException();
        }
        public static bool LoadFlat(ISound sound)
        {
            throw new NotImplementedException();
        }
        public static bool ReleaseFlat(ISound sound)
        {
            throw new NotImplementedException();
        }
        public static bool UnloadFlat(ISound sound)
        {
            throw new NotImplementedException();
        }
        // Music
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
        // Three Dimensional
        public static bool Play3D(ISound sound, Vector3 playLocation)
        {
            throw new NotImplementedException();
        }
        public static bool Pause3D(Sound3DInstance soundInstance)
        {
            throw new NotImplementedException();
        }
        public static bool Load3D(ISound sound)
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
    }
}