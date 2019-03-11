using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Component_Based_Game_Engine.Objects;

namespace Component_Based_Game_Engine.Systems
{
    public interface ISystem
    {
        void OnAction();
        void AssignEntity(oEntity entity);
        void DestroyEntity(oEntity entity);

        string Name
        {
            get;
        }
    }
}
