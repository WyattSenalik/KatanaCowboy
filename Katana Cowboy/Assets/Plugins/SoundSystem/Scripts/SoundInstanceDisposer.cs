using System.Collections;
using UnityEngine;

namespace SoundSystem.Internal
{
    /// <summary>
    /// Controls the disposal of sound instances using coroutines.
    /// </summary>
    public class SoundInstanceDisposer : MonoBehaviour
    {
        /// <summary>
        /// Starts a coroutine to dispose of the sound instance after it
        /// has finished playing.
        /// </summary>
        /// <param name="soundInstance">Sound Instance to monitor.</param>
        public void MonitorSoundInstance(ISoundInstance soundInstance)
        {
            if (soundInstance.audioSource.loop)
            {
                return;
            }
            StartCoroutine(DisposeAfterPlayCoroutine(soundInstance));
        }


        /// <summary>
        /// Disposes the sound instance if it is no longer playing after
        /// waiting for the length of the clip.
        /// </summary>
        private IEnumerator DisposeAfterPlayCoroutine(ISoundInstance soundInstance)
        {
            AudioSource audioSource = soundInstance.audioSource;
            // Wait until the audio clip has finished
            yield return new WaitForSeconds(audioSource.clip.length + 0.01f);

            // Dispose of the sound instance if it is no longer playing
            if (!audioSource.isPlaying)
            {
                soundInstance.Dispose();
            }
            // If the sound instance is still playing, maybe it was set to loop while playing
            // so check if we should monitor it again
            else
            {
                MonitorSoundInstance(soundInstance);
            }
        }
    }
}
