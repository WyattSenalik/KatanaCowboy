using UnityEngine;

namespace SoundSystem
{
    public interface ISoundInstance
    {
        GameObject gameObject { get; }
        AudioSource audioSource { get; }


        void Dispose();
        void Pause();
        void Play();
    }
}
