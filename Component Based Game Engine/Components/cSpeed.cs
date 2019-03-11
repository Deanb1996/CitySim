using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Component_Based_Game_Engine.Components
{
    public class cSpeed : IComponent
    {
        float speed;

        public cSpeed(float newSpeed)
        {
            speed = newSpeed;
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public ComponentMasks ComponentMask
        {
            get{ return ComponentMasks.COMPONENT_SPEED; }
        }
    }
}
