using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

using SoundSystem.Internal;

namespace SoundSystem
{
    public static class SoundManager
    {
        // Persistent Audio
        private static GameObject persistentFlatObj = null;
        private static Dictionary<ISound, AudioSource> persistentFlatAudios = new Dictionary<ISound, AudioSource>();
        private static GameObject activeFlatObj = null;


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
        public static bool Unload(ISound sound)
        {
            throw new NotImplementedException();
        }
        public static bool PlayMusic(ISound music = null)
        {
            throw new NotImplementedException();
        }
        public static bool PauseMusic(ISound music = null)
        {
            throw new NotImplementedException();
        }
        public static bool LoadMusic(ISound music = null)
        {
            throw new NotImplementedException();
        }
        public static bool UnloadMusic(ISound music = null)
        {
            throw new NotImplementedException();
        }
    }
}