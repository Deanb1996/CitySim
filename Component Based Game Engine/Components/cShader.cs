using Component_Based_Game_Engine.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component_Based_Game_Engine.Components
{
    public class cShader : IComponent
    {
        int pgmID;

        public cShader(string vShaderName, string fShaderName)
        {
            pgmID = mResource.LoadShaderProgram(vShaderName, fShaderName);
        }
        public cShader()
        {
            pgmID = mResource.LoadShaderProgram("Shaders/vs.glsl", "Shaders/fs.glsl");
        }

        public int PgmID
        {
            get { return pgmID; }
            set { pgmID = value; }
        }

        public ComponentMasks ComponentMask
        {
            get { return ComponentMasks.COMPONENT_SHADER; }
        }
    }
}
