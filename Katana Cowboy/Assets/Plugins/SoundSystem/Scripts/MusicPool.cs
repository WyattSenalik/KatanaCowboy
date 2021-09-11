using UnityEngine;

namespace SoundSystem.Internal
{
    public class MusicPool : IMusicPool
    {
        private readonly GameObject musicPoolObject = null;
        private readonly AudioSource audioSource = null;
        private ISound activeMusic = null;


        public MusicPool(string name = "MusicPool")
        {
            musicPoolObject = new GameObject(name);
            UnityEngine.Object.DontDestroyOnLoad(musicPoolObject);
            audioSource = musicPoolObject.AddComponent<AudioSource>();
        }


        public void Pause()
        {
            if (activeMusic == null)
            {
                return;
            }

            audioSource.Pause();
        }
        public void Play(ISound music)
        {
            activeMusic = music;
            audioSource.SetFromSound(music);
            audioSource.Play();
        }
        public void Restart()
        {
            audioSource.time = 0;
        }
    }
}