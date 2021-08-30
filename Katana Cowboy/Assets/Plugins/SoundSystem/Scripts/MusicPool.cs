
namespace SoundSystem.Internal
{
    public class MusicPool : IMusicPool
    {
        private ISound2DPool sound2DPool = null;
        private ISound activeMusic = null;


        public MusicPool(string name = "MusicPool")
        {
            sound2DPool = new Sound2DPool(name, true);
        }


        public bool Add(ISound music)
        {
            return sound2DPool.Add(music);
        }
        public void Pause()
        {
            if (activeMusic == null)
            {
                return;
            }

            sound2DPool.Pause(activeMusic);
        }
        public void Play(ISound music)
        {
            // Pause the last music
            if (activeMusic != null)
            {
                sound2DPool.Pause(activeMusic);
            }

            activeMusic = music;
            sound2DPool.Play(music, Play2DOptions.FirstInterupt);
        }
        public bool Release(ISound music)
        {
            if (music == activeMusic)
            {
                activeMusic = null;
            }
            return sound2DPool.Release(music);
        }
        public bool Remove(ISound music)
        {
            if (music == activeMusic)
            {
                activeMusic = null;
            }
            return sound2DPool.Remove(music);
        }
    }
}