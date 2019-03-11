using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Component_Based_Game_Engine.Managers;
using Component_Based_Game_Engine.Objects;

namespace Component_Based_Game_Engine.Components
{
    public class cGeometry : IComponent
    {
        oGeometry geometry;

        public cGeometry(string geometryName)
        {
            this.geometry = mResource.LoadGeometry(geometryName);
        }

        public ComponentMasks ComponentMask
        {
            get { return ComponentMasks.COMPONENT_GEOMETRY; }
        }

        public oGeometry Geometry()
        {
            return geometry;
        }
    }
}
