using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Component_Based_Game_Engine.Managers;
using OpenTK.Graphics;

namespace Component_Based_Game_Engine.Components
{
    public class cTexture : IComponent
    {
        int texture;

        public cTexture(string textureName)
        {
            texture = mResource.LoadTexture(textureName);
        }

        public int Texture
        {
            get { return texture; }
        }

        public ComponentMasks ComponentMask
        {
            get { return ComponentMasks.COMPONENT_TEXTURE; }
        }
    }
}
