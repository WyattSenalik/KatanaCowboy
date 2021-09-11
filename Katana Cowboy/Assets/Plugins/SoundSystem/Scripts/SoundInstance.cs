using UnityEngine;

using SoundSystem.Internal;

namespace SoundSystem
{
    public class SoundInstance : ISoundInstance
    {
        public GameObject gameObject => audioSource.gameObject;
        public AudioSource audioSource { get; private set; }

        private IBaseSoundPool soundPool = null;


        public SoundInstance(AudioSource source, IBaseSoundPool pool)
        {
            audioSource = source;
            soundPool = pool;
        }


        public void Dispose()
        {
            soundPool.ReleaseAudioSource(audioSource);
        }
        public void Pause()
        {
            audioSource.Pause();
        }
        public void Play()
        {
            audioSource.Play();
        }
    }
}
