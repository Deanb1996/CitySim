using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Component_Based_Game_Engine.Managers;

namespace Component_Based_Game_Engine.Objects
{
    public class oScene
    {
        protected mScene sceneManager;

        public oScene(mScene sceneManager)
        {
            this.sceneManager = sceneManager;
        }
    }
}
