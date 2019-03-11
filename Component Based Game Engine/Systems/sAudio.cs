using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Component_Based_Game_Engine.Objects;
using OpenTK;
using Component_Based_Game_Engine.Components;
using OpenTK.Audio.OpenAL;

namespace Component_Based_Game_Engine.Systems
{
    public class sAudio : ISystem
    {
        const ComponentMasks MASK = (ComponentMasks.COMPONENT_AUDIO | ComponentMasks.COMPONENT_TRANSFORM);

        private Vector3 listenerPosition;
        private Vector3 listenerDirection;
        private Vector3 listenerUp;
        private int mySource;
        private int newSource;

        oCamera camera;

        List<oEntity> entityList;

        public sAudio(oCamera camera)
        {

        }

        public string Name
        {
            get { return "SystemAudio"; }
        }

        public void AssignEntity(oEntity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                entityList.Add(entity);
            }
        }

        public void DestroyEntity(oEntity entity)
        {
            if (entityList.Contains(entity))
            {
                DeleteSource(entity);

                entityList.Remove(entity);
            }
        }

        public void OnAction()
        {
            //Updates listener position every frame
            UpdateListener();

            foreach (oEntity entity in entityList)
            {
                //Retrieves list of components from current entity
                List<IComponent> components = entity.Components;

                //Retrieves transform component from current entity
                IComponent transformComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentMask == ComponentMasks.COMPONENT_TRANSFORM;
                });

                //Retrieves translation vector from the transform component
                Vector3 translation = ((cTransform)transformComponent).Translation;

                //Retrieves audio component from current entity
                IComponent audioComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentMask == ComponentMasks.COMPONENT_AUDIO;
                });

                //Retrieves needed values from the audio component
                oAudioBuffer audio = ((cAudio)audioComponent).AudioBuffer();
                bool looping = ((cAudio)audioComponent).Looping();
                bool isPlaying = ((cAudio)audioComponent).IsPlaying;

                //Checks if the audio component has already been assigned an audio source and assigns it one if not already assigned
                if (((cAudio)audioComponent).AudioSource == 0)
                {
                    AL.GenSources(1, out newSource);
                    ((cAudio)audioComponent).AudioSource = newSource;
                }

                //Checks if play variable is true, and if true plays the audio source
                if (((cAudio)audioComponent).Play == true)
                {
                    mySource = ((cAudio)audioComponent).AudioSource;

                    PlayAudio(looping, isPlaying, mySource, audio, translation);

                    ((cAudio)audioComponent).IsPlaying = true;
                }
            }
        }

        public void PlayAudio(bool isLooping, bool isPlaying, int source, oAudioBuffer audio, Vector3 position)
        {
            audio.Play(isLooping, isPlaying, source, position);         
        }

        private void UpdateListener()
        {
            listenerPosition = camera.Position;
            listenerDirection = camera.Direction;
            listenerUp = camera.UpDirection;
            AL.Listener(ALListener3f.Position, ref listenerPosition);
            AL.Listener(ALListenerfv.Orientation, ref listenerDirection, ref listenerUp);
            AL.DistanceModel(ALDistanceModel.LinearDistance);
        }

        private void DeleteSource(oEntity entity)
        {
            List<IComponent> components = entity.Components;

            IComponent audioComponent = components.Find(delegate (IComponent component)
            {
                return component.ComponentMask == ComponentMasks.COMPONENT_AUDIO;
            });

            AL.DeleteSource(((cAudio)audioComponent).AudioSource);
        }
    }
}
