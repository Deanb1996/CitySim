using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using OpenTK;

namespace Component_Based_Game_Engine.Managers
{
    public class mInput
    {
        List<string> keyboardInput;
        List<string> mouseInput;
        private Vector2 mousePosition;
        mScene sceneManager;

        public mInput(mScene sceneManagerIn)
        {
            sceneManager = sceneManagerIn;
            keyboardInput = new List<string>();
            mouseInput = new List<string>();
            sceneManager.KeyDown += GameKeyDown;
            sceneManager.KeyUp += GameKeyUp;
            sceneManager.MouseMove += MouseMovement;
            sceneManager.MouseDown += MouseClickDown;
            sceneManager.MouseUp += MouseClickUp;
        }

        public void CenterCursor()
        {
            Mouse.SetPosition(sceneManager.Bounds.Left + sceneManager.Bounds.Width / 2, sceneManager.Bounds.Top + sceneManager.Bounds.Height / 2);
        }

        public void CursorVisible(bool hideCursor)
        {
            if (hideCursor == true)
            {
                sceneManager.CursorVisible = true;
            }
            if (hideCursor == false)
            {
                sceneManager.CursorVisible = false;
            }
        }

        public List<string> Keyboard()
        {
            return keyboardInput;
        }

        public List<string> MouseInput()
        {
            return mouseInput;
        }

        public Vector2 MousePosition()
        {
            return mousePosition;
        }

        void GameKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (!keyboardInput.Contains(e.Key.ToString()))
            {
                keyboardInput.Add(e.Key.ToString());
            }
        }

        void GameKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            keyboardInput.Remove(e.Key.ToString());
        }

        void MouseClickDown(object sender, MouseButtonEventArgs e)
        {
            if (!mouseInput.Contains(e.Button.ToString()))
            {
                mouseInput.Add(e.Button.ToString());
            }
        }

        void MouseClickUp(object sender, MouseButtonEventArgs e)
        {
            mouseInput.Remove(e.Button.ToString());
        }

        void MouseMovement(object sender, MouseMoveEventArgs e)
        {
            mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }
    }
}
