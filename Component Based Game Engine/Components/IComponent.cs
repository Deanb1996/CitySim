using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component_Based_Game_Engine.Components
{
    public enum ComponentMasks
    {
        COMPONENT_NONE = 0,
        COMPONENT_TRANSFORM = 1 << 0,
        COMPONENT_GEOMETRY = 1 << 1,
        COMPONENT_TEXTURE = 1 << 2,
        COMPONENT_SPEED = 1 << 3,
        COMPONENT_AI = 1 << 4,
        COMPONENT_AUDIO = 1 << 5,
        COMPONENT_SHADER = 1 << 6,
        COMPONENT_STOCK = 1 << 7
    }

    public interface IComponent
    {
        ComponentMasks ComponentMask
        {
            get;
        }
    }
}
