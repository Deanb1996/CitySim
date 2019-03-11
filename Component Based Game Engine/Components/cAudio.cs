using Component_Based_Game_Engine.Managers;
using Component_Based_Game_Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component_Based_Game_Engine.Components
{
    public class cAudio : IComponent
    {
        private oAudioBuffer audioBuffer;
        private int audioSource;
        private bool looping;
        private bool isPlaying;
        private bool play;

        public cAudio(string audioName, bool loopingIn, bool playIn)
        {
            audioBuffer = mResource.LoadWav(audioName);
            looping = loopingIn;
            audioSource = 0;
            play = playIn;
        }

        public ComponentMasks ComponentMask
        {
            get { return ComponentMasks.COMPONENT_AUDIO; }
        }

        public oAudioBuffer AudioBuffer()
        {
            return audioBuffer;
        }

        public bool Looping()
        {
            return looping;
        }

        public bool IsPlaying
        {
            get { return isPlaying; }
            set { isPlaying = value; }
        }

        public int AudioSource
        {
            get { return audioSource; }
            set { audioSource = value; }
        }

        public bool Play
        {
            get { return play; }
            set { play = value; }
        }
    }
}
