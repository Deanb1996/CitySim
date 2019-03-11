using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using Component_Based_Game_Engine.Objects;

namespace Component_Based_Game_Engine.Managers
{
    public class mScene : GameWindow
    {
        oScene scene;
        static int windowWidth;
        static int windowHeight;

        public delegate void SceneDelegate(FrameEventArgs e);
        public SceneDelegate renderer;
        public SceneDelegate updater;

        public float dt;
        public float time;

        public mScene() : base(
            1920,
            1080,
            GraphicsMode.Default,
            "Transport Simulation Game",
            GameWindowFlags.Fullscreen,
            DisplayDevice.Default,
            3,
            3,
            GraphicsContextFlags.ForwardCompatible
            )
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Sets window width and height
            Width = 1920;
            Height = 1080;
            windowWidth = Width;
            windowHeight = Height;

            //Load GUI
            GUI.LoadGUI(windowWidth, windowHeight);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            dt = (float)e.Time;
            time += (float)e.Time;
            updater(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            renderer(e);

            GL.Flush();
            SwapBuffers();
        }

        public void LoadScene(oScene sceneIn)
        {
            scene = sceneIn;
        }
    }
}
