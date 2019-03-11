using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Component_Based_Game_Engine.Components;

namespace Component_Based_Game_Engine.Objects
{
    public class oEntity
    {
        string name;
        List<IComponent> componentList = new List<IComponent>();
        ComponentMasks mask;

        public oEntity(string name)
        {
            this.name = name;
        }

        public void AddComponent(IComponent component)
        {
            Debug.Assert(component != null, "Component must not be null");

            componentList.Add(component);
            mask |= component.ComponentMask;
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public ComponentMasks Mask
        {
            get { return mask; }
        }

        public List<IComponent> Components
        {
            get { return componentList; }
        }

        public cTransform GetTransform()
        {
            return (cTransform)componentList.Find(delegate (IComponent e)
            {
                return e.ComponentMask == ComponentMasks.COMPONENT_TRANSFORM;
            });
        }

        public cSpeed GetSpeed()
        {
            return (cSpeed)componentList.Find(delegate (IComponent e)
            {
                 return e.ComponentMask == ComponentMasks.COMPONENT_SPEED;
            });
        }

        public cStock GetStock()
        {
            return (cStock)componentList.Find(delegate (IComponent e)
            {
                return e.ComponentMask == ComponentMasks.COMPONENT_STOCK;
            });
        }

        public cAI GetAI()
        {
            return (cAI)componentList.Find(delegate (IComponent e)
            {
                return e.ComponentMask == ComponentMasks.COMPONENT_AI;
            });
        }
    }
}
