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


        #region Flat
        /// <summary>
        /// Plays the sound as a 2D sound.
        /// </summary>
        /// <param name="option">Option for playing.</param>
        /// <returns>If the sound was played.</returns>
        public bool PlayAsFlat(Play2DOptions option = Play2DOptions.FirstNoInterupt)
        {
            return SoundManager.PlayFlat(this, option);
        }
        /// <summary>
        /// Pauses the sound (2D sound).
        /// </summary>
        /// <param name="option">Option for pausing.</param>
        /// <returns>If the sound was paused</returns>
        public bool PauseAsFlat(Pause2DOptions option = Pause2DOptions.FirstPlaying)
        {
            return SoundManager.PauseFlat(this, option);
        }
        /// <summary>
        /// Loads the sound as a 2D sound.
        /// </summary>
        /// <param name="option">Option for loading.</param>
        /// <returns>If the sound was loaded.</returns>
        public bool LoadAsFlat(Add2DOptions option = Add2DOptions.SingleInstance)
        {
            return SoundManager.LoadFlat(this, option);
        }
        /// <summary>
        /// Releases the sound (2D sound).
        /// </summary>
        /// <param name="option">Option for releasing.</param>
        /// <returns>If the sound was released.</returns>
        public bool ReleaseAsFlat(GetRid2DOptions option = GetRid2DOptions.First)
        {
            return SoundManager.ReleaseFlat(this, option);
        }
        /// <summary>
        /// Unloads the sound (2D sound).
        /// </summary>
        /// <param name="option">Option for unloading.</param>
        /// <returns>If the sound was unloaded.</returns>
        public bool UnloadAsFlat(GetRid2DOptions option = GetRid2DOptions.First)
        {
            return SoundManager.UnloadFlat(this, option);
        }
        #endregion Flat


        #region Music
        /// <summary>
        /// Plays the sound as music.
        /// </summary>
        /// <returns>If the sound was successfully played.</returns>
        public void PlayMusic()
        {
            SoundManager.PlayMusic(this);
        }
        /// <summary>
        /// Loads the sound as music.
        /// </summary>
        /// <returns>If the sound was successfully loaded.</returns>
        public bool LoadMusic()
        {
            return SoundManager.LoadMusic(this);
        }
        /// <summary>
        /// Releases the sound from the music player.
        /// </summary>
        /// <returns>If the sound was successfully released.</returns>
        public bool ReleaseMusic()
        {
            return SoundManager.ReleaseMusic(this);
        }
        /// <summary>
        /// Unloads the sound from the music player.
        /// </summary>
        /// <returns>If the sound was successfully removed.</returns>
        public bool UnloadMusic()
        {
            return SoundManager.UnloadMusic(this);
        }
        #endregion Music


        #region ThreeDim
        public Sound3DInstance PlayAs3D(Vector3 position)
        {
            return SoundManager.Play3D(this, position);
        }
        public bool PauseAs3D(Sound3DInstance soundInstance)
        {
            return SoundManager.Pause3D(soundInstance);
        }
        public Sound3DInstance LoadAs3D(Vector3 position)
        {
            return SoundManager.Load3D(this, position);
        }
        public bool ReleaseAs3D(Sound3DInstance soundInstance)
        {
            return SoundManager.Release3D(soundInstance);
        }
        public bool UnloadAs3D(Sound3DInstance soundInstance)
        {
            return SoundManager.Unload3D(soundInstance);
        }
        #endregion ThreeDim
    }
}
