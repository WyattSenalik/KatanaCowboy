using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem.Example
{
    public class MusicExample : MonoBehaviour
    {
        [SerializeField] private Sound music = null;


        public void Play()
        {
            music.PlayMusic();
        }
        public void Pause()
        {
            SoundManager.PauseMusic();
        }
        public void Load()
        {
            music.LoadMusic();
        }
        public void Release()
        {
            music.ReleaseMusic();
        }
        public void Unload()
        {
            music.UnloadMusic();
        }
    }
}
