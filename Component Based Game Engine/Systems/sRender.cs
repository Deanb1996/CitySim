using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Component_Based_Game_Engine.Components;
using Component_Based_Game_Engine.Objects;
using System.Drawing;

namespace Component_Based_Game_Engine.Systems
{
    public class sRender : ISystem
    {
        List<oEntity> entityList;

        const ComponentMasks MASK = (ComponentMasks.COMPONENT_TRANSFORM | ComponentMasks.COMPONENT_TEXTURE | ComponentMasks.COMPONENT_GEOMETRY | ComponentMasks.COMPONENT_SHADER);

        protected int pgmID;
        protected int vsID;
        protected int fsID;
        protected int attribute_vtex;
        protected int attribute_vpos;
        protected int uniform_stex;
        protected int uniform_mview;
        protected int currentShader = -1;
        oCamera camera;
        Rectangle clientRectangle;
        

        public sRender(oCamera cameraIn, Rectangle clientRectangleIn)
        {
            entityList = new List<oEntity>();
            camera = cameraIn;
            clientRectangle = clientRectangleIn;
        }

        public void AssignEntity(oEntity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                entityList.Add(entity);
            }
        }

        public void DestroyEntity(oEntity entity)
        {
            entityList.Remove(entity);
        }

        public string Name
        {
            get { return "RenderSystem"; }
        }

        public void OnAction()
        {
            foreach (oEntity entity in entityList)
            {
                //Retrieves list of components from current entity
                List<IComponent> components = entity.Components;

                //Retrieves geometry component from current entity
                IComponent geometryComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentMask == ComponentMasks.COMPONENT_GEOMETRY;
                });
                oGeometry geometry = ((cGeometry)geometryComponent).Geometry();

                //Retrieves transform component from current entity
                IComponent transformComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentMask == ComponentMasks.COMPONENT_TRANSFORM;
                });

                //Sets transform if not already set, and retrieves transform value
                if (((cTransform)transformComponent).SetTransform == false)
                {
                    SetTransform((cTransform)transformComponent);
                    ((cTransform)transformComponent).SetTransform = true;
                }
                Matrix4 transform = ((cTransform)transformComponent).Transform;

                //Retrieves texture component from current entity
                IComponent textureComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentMask == ComponentMasks.COMPONENT_TEXTURE;
                });
                int texture = ((cTexture)textureComponent).Texture;

                //Retrieves shader component from current entity
                IComponent shaderComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentMask == ComponentMasks.COMPONENT_SHADER;
                });
                int pgmID = ((cShader)shaderComponent).PgmID;

                Draw(transform, geometry, texture, pgmID);
            }
        }

        public void SetTransform(cTransform transform)
        {
            Matrix4 scaleMat = Matrix4.CreateScale(transform.Scale);
            Matrix4 rotateMat = Matrix4.CreateRotationX(transform.Rotation.X) * Matrix4.CreateRotationY(transform.Rotation.Y) * Matrix4.CreateRotationZ(transform.Rotation.Z);
            Matrix4 translateMat = Matrix4.CreateTranslation(transform.Translation);

            transform.Transform = scaleMat * rotateMat * translateMat;

            //Sets facing direction vectors in 3d space
            transform.Up = new Vector3(transform.Transform[1, 0], transform.Transform[1, 1], transform.Transform[1, 2]);
            transform.Forward = -new Vector3(transform.Transform[2, 0], transform.Transform[2, 1], transform.Transform[2, 2]);
            transform.Right = new Vector3(transform.Transform[0, 0], transform.Transform[0, 1], transform.Transform[0, 2]);
        }

        public void Draw(Matrix4 transform, oGeometry geometry, int texture, int pgmID)
        {
            //Makes sure that the correct shader is bound for current entity
            if (currentShader != pgmID)
            {
                BindShader(pgmID);
                currentShader = pgmID;
            }
            else
            {
                GL.UseProgram(currentShader);
            }

            GL.Uniform1(uniform_stex, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Viewport(clientRectangle);

            //Calculates and passes world view projection matrix to the shader
            Matrix4 worldViewProjection = transform * camera.View * camera.Projection;
            GL.UniformMatrix4(uniform_mview, false, ref worldViewProjection);

            //Renders entity
            geometry.Render();

            //Clears the vertex array and shader program
            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }

        public void BindShader(int pgmID)
        {
            GL.UseProgram(pgmID);

            attribute_vpos = GL.GetAttribLocation(pgmID, "a_Position");
            attribute_vtex = GL.GetAttribLocation(pgmID, "a_TexCoord");
            uniform_mview = GL.GetUniformLocation(pgmID, "WorldViewProj");
            uniform_stex = GL.GetUniformLocation(pgmID, "s_Texture");
        }
    }
}
