using UnityEngine;

namespace SoundSystem.Example
{
    public class SoundExample : MonoBehaviour
    {
        [SerializeField] private Sound sound = null;
        private ISoundInstance flatInstance = null;
        private ISoundInstance instance3D = null;


        public void PlayAsFlat()
        {
            flatInstance = sound.PlayAsFlat();
        }
        public void PauseFlat()
        {
            flatInstance.Pause();
        }
        public void PlayAs3D()
        {
            instance3D = sound.PlayAs3D(new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)));
        }
        public void Pause3D()
        {
            instance3D.Pause();
        }
        public void PlayAsMusic()
        {
            sound.PlayAsMusic();
        }
    }
}
