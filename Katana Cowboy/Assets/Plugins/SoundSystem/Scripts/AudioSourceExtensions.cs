using UnityEngine;

namespace SoundSystem.Internal
{
    /// <summary>
    /// Extension methods for AudioSource for the SoundSystem.
    /// </summary>
    public static class AudioSourceExtensions
    {
        /// <summary>
        /// Sets the values for the current AudioSource based on the given sound data.
        /// </summary>
        public static void SetFromSound(this AudioSource audio, ISound sound)
        {
            audio.clip = sound.GetClip();
            audio.outputAudioMixerGroup = sound.GetMixerGroup();
            audio.mute = false;
            audio.playOnAwake = false;
            audio.loop = sound.IsLoop();
            audio.priority = sound.GetPriority();
            audio.volume = sound.GetVolume();
            audio.pitch = sound.GetPitch();
            audio.panStereo = sound.GetStereoPan();
            audio.spatialBlend = sound.GetSpatialBlend();
            audio.reverbZoneMix = sound.GetReverbZoneMix();
        }
    }
}