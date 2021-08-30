using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem.Example
{
    public class SoundExample : MonoBehaviour
    {
        [SerializeField] private Sound sound = null;


        public void Play()
        {
            sound.PlayAsFlat();
        }
        public void PlayNew()
        {
            sound.PlayAsFlat(Play2DOptions.MultipleInstance);
        }
        public void Pause()
        {
            sound.PauseAsFlat();
        }
        public void PauseAll()
        {
            sound.PauseAsFlat(Pause2DOptions.All);
        }
        public void Load()
        {
            sound.LoadAsFlat();
        }
        public void LoadNew()
        {
            sound.LoadAsFlat(Add2DOptions.MultipleInstance);
        }
        public void Release()
        {
            sound.ReleaseAsFlat();
        }
        public void ReleaseAll()
        {
            sound.ReleaseAsFlat(GetRid2DOptions.All);
        }
        public void ReleaseNonPlaying()
        {
            sound.ReleaseAsFlat(GetRid2DOptions.FirstNonPlaying);
        }
        public void ReleaseAllNonPlaying()
        {
            sound.ReleaseAsFlat(GetRid2DOptions.AllNonPlaying);
        }
        public void Unload()
        {
            sound.UnloadAsFlat();
        }
        public void UnloadAll()
        {
            sound.UnloadAsFlat(GetRid2DOptions.All);
        }
        public void UnloadNonPlaying()
        {
            sound.UnloadAsFlat(GetRid2DOptions.FirstNonPlaying);
        }
        public void UnloadAllNonPlaying()
        {
            sound.UnloadAsFlat(GetRid2DOptions.AllNonPlaying);
        }
    }
}
