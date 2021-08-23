using UnityEngine;
using UnityEngine.Audio;

using SoundSystem.Internal;

namespace SoundSystem
{
    /// <summary>
    /// Scriptable Object to hold sound data to be played by the SoundManager.
    /// </summary>
    [CreateAssetMenu(fileName = "new Sound", menuName = "ScriptableObjects/SoundSystem/Sound")]
    public class Sound : ScriptableObject, ISound
    {
        public AudioClip GetClip() => clip;
        [SerializeField] private AudioClip clip = null;

        public AudioMixerGroup GetMixerGroup() => mixer;
        [SerializeField] private AudioMixerGroup mixer = null;

        public bool IsLoop() => isLoop;
        [SerializeField] private bool isLoop = false;

        public int GetPriority() => priority;
        [SerializeField, Range(0, 256)] private int priority = 128;

        public float GetVolume() => volume;
        [SerializeField, Range(0.0f, 1.0f)] private float volume = 1.0f;

        public float GetPitch() => pitch;
        [SerializeField, Range(-3.0f, 3.0f)] private float pitch = 1.0f;

        public float GetStereoPan() => stereoPan;
        [SerializeField, Range(-1.0f, 1.0f)] private float stereoPan = 0.0f;

        public float GetSpatialBlend() => spatialBlend;
        [SerializeField, Range(0.0f, 1.0f)] private float spatialBlend = 0.0f;

        public float GetReverbZoneMix() => reverbZoneMix;
        [SerializeField, Range(0.0f, 1.1f)] private float reverbZoneMix = 1.0f;

        public bool IsScenePersistent() => isScenePersistent;
        [SerializeField] private bool isScenePersistent = false;


        // Sound
        public bool Play()
        {
            return SoundManager.Play(this);
        }
        public bool Pause()
        {
            return SoundManager.Pause(this);
        }
        public bool Load()
        {
            return SoundManager.Load(this);
        }
        public bool Release()
        {
            return SoundManager.Release(this);
        }
        public bool Unload()
        {
            return SoundManager.Unload(this);
        }
        // Music
        public bool PlayMusic()
        {
            return SoundManager.PlayMusic(this);
        }
        public bool PauseMusic()
        {
            return SoundManager.PauseMusic(this);
        }
        public bool LoadMusic()
        {
            return SoundManager.LoadMusic(this);
        }
        public bool ReleaseMusic()
        {
            return SoundManager.ReleaseMusic(this);
        }
        public bool UnloadMusic()
        {
            return SoundManager.UnloadMusic(this);
        }
    }
}
