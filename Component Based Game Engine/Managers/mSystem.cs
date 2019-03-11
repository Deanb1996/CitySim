using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Component_Based_Game_Engine.Systems;
using Component_Based_Game_Engine.Objects;

namespace Component_Based_Game_Engine.Managers
{
    public class mSystem
    {
        List<ISystem> systemRenderList = new List<ISystem>();
        List<ISystem> systemUpdateList = new List<ISystem>();

        public mSystem()
        {
        }

        public void AssignEntities(mEntity entityManager)
        {
            var systemList = systemRenderList.Concat(systemUpdateList);
            List<oEntity> entityList = entityManager.Entities();
            foreach (ISystem system in systemList)
            {
                foreach (oEntity entity in entityList)
                {
                    system.AssignEntity(entity);
                }
            }
        }

        public void AssignNewEntity(oEntity entity)
        {
            var systemList = systemRenderList.Concat(systemUpdateList);
            foreach (ISystem system in systemList)
            {
                system.AssignEntity(entity);
            }
        }

        public void DestroyEntity(oEntity entity)
        {
            var systemList = systemRenderList.Concat(systemUpdateList);
            foreach (ISystem system in systemList)
            {
                system.DestroyEntity(entity);
            }
        }

        public void RenderSystems()
        {
            foreach (ISystem system in systemRenderList)
            {
                system.OnAction();
            }
        }

        public void UpdateSystems()
        {
            foreach (ISystem system in systemUpdateList)
            {
                system.OnAction();
            }
        }

        public void AddRenderSystem(ISystem system)
        {
            ISystem result = FindRenderSystem(system.Name);
            systemRenderList.Add(system);
        }

        public void AddUpdateSystem(ISystem system)
        {
            ISystem result = FindUpdateSystem(system.Name);
            systemUpdateList.Add(system);
        }

        private ISystem FindRenderSystem(string name)
        {
            return systemRenderList.Find(delegate (ISystem system)
            {
                return system.Name == name;
            }
            );
        }

        private ISystem FindUpdateSystem(string name)
        {
            return systemUpdateList.Find(delegate (ISystem system)
            {
                return system.Name == name;
            }
            );
        }
    }
}
