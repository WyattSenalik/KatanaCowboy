using UnityEngine;
using UnityEngine.Audio;

namespace SoundSystem.Internal
{
    /// <summary>
    /// SInterface for the sound data container.
    /// </summary>
    public interface ISound
    {
        AudioClip GetClip();
        AudioMixerGroup GetMixerGroup();
        bool IsLoop();
        int GetPriority();
        float GetVolume();
        float GetPitch();
        float GetStereoPan();
        float GetSpatialBlend();
        float GetReverbZoneMix();

        bool IsScenePersistent();
    }
}
