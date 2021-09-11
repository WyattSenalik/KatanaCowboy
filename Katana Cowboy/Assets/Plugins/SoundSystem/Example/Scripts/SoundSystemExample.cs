using UnityEngine;

namespace SoundSystem.Example
{
    public class SoundSystemExample : MonoBehaviour
    {
        public void PauseMusic()
        {
            SoundManager.PauseMusic();
        }
        public void RestartMusic()
        {
            SoundManager.RestartMusic();
        }
        public void ClearFlatSounds()
        {
            SoundManager.ClearFlat();
        }
        public void Clear3DSounds()
        {
            SoundManager.Clear3D();
        }
    }
}