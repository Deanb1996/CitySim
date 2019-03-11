using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Component_Based_Game_Engine.Objects
{
   public interface IScene
    {
        void Render(FrameEventArgs e);
        void Update(FrameEventArgs e);
    }
}
