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


        // Flat sounds
        /// <summary>
        /// Plays the sound as a 2D sound.
        /// </summary>
        /// <returns>If the sound was played.</returns>
        public bool PlayAsFlat()
        {
            return SoundManager.PlayFlat(this);
        }
        /// <summary>
        /// Pauses the sound (2D sound).
        /// </summary>
        /// <returns>If the sound was paused</returns>
        public bool PauseAsFlat()
        {
            return SoundManager.PauseFlat(this);
        }
        /// <summary>
        /// Loads the sound as a 2D sound.
        /// </summary>
        /// <returns>If the sound was loaded.</returns>
        public bool LoadAsFlat()
        {
            return SoundManager.LoadFlat(this);
        }
        /// <summary>
        /// Releases the sound (2D sound).
        /// </summary>
        /// <returns>If the sound was released.</returns>
        public bool ReleaseAsFlat()
        {
            return SoundManager.ReleaseFlat(this);
        }
        /// <summary>
        /// Unloads the sound (2D sound).
        /// </summary>
        /// <returns>If the sound was unloaded.</returns>
        public bool UnloadAsFlat()
        {
            return SoundManager.UnloadFlat(this);
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
        // Three Dimensional
        public bool Play3D()
        {
            return SoundManager.PlayFlat(this);
        }
        public bool Pause3D()
        {
            return SoundManager.PauseMusic(this);
        }
        public bool Load3D()
        {
            return SoundManager.LoadMusic(this);
        }
        public bool Release3D()
        {
            return SoundManager.ReleaseMusic(this);
        }
        public bool Unload3D()
        {
            return SoundManager.UnloadMusic(this);
        }
    }
}
