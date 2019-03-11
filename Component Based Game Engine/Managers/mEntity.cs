using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Component_Based_Game_Engine.Objects;
using System.Diagnostics;

namespace Component_Based_Game_Engine.Managers
{
    public class mEntity
    {
        List<oEntity> entityList;
        List<string> entityNames;

        public mEntity()
        {
            entityList = new List<oEntity>();
            entityNames = new List<string>();
        }

        public oEntity FindEntity(string name)
        {
            return entityList.Find(delegate (oEntity e)
            {
                return e.Name == name;
            });
        }

        public void AddEntity(oEntity entity)
        {
            bool added = false;
            int count = 0;
            string name = entity.Name;
            string tempName = entity.Name;

            //Checks if there is already an entity with the same name, and appends an iterating number to the end of each duplicate
            while (!added)
            {
                if (entityNames.Contains(tempName))
                {
                    tempName = string.Format("{0} ({1})", name, count);
                }
                else
                {
                    entityNames.Add(tempName);
                    entity.Name = tempName;
                    entityList.Add(entity);
                    added = true;
                }
                count++;
            }
        }

        public void RemoveEntity(oEntity entity)
        {
            entityList.Remove(entity);
            entityNames.Remove(entity.Name);
        }

        public List<oEntity> Entities()
        {
            return entityList;
        }
    }
}
