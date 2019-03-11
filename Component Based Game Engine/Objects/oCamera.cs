using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component_Based_Game_Engine.Objects
{
    public class oCamera
    {
        private Matrix4 view, projection;
        private Vector3 position, viewDirection, upDirection;
        private int fov;
        private float aspectRatio, near, far, width, height;

        /// <summary>
        /// Perspective Camera
        /// </summary>
        /// <param name="inPosition"></param>
        /// <param name="inViewDirection"></param>
        /// <param name="inUpDirection"></param>
        /// <param name="fov"></param>
        /// <param name="aspectRatio"></param>
        /// <param name="near"></param>
        /// <param name="far"></param>
        public oCamera(Vector3 inPosition, Vector3 inViewDirection, Vector3 inUpDirection, int fov, float aspectRatio, float near, float far)
        {
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

            GL.CullFace(CullFaceMode.Front);
            //GL.Enable(EnableCap.DepthTest);

            position = inPosition;
            viewDirection = inViewDirection;
            upDirection = inUpDirection;

            view = Matrix4.LookAt(position, viewDirection, upDirection);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), aspectRatio, near, far);
        }

        /// <summary>
        /// Orthographic Camera
        /// </summary>
        /// <param name="inPosition"></param>
        /// <param name="inViewDirection"></param>
        /// <param name="inUpDirection"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="near"></param>
        /// <param name="far"></param>
        public oCamera(Vector3 inPosition, Vector3 inViewDirection, Vector3 inUpDirection, float width, float height, float near, float far)
        {
            GL.ClearColor(0.0f, 1.0f, 0.0f, 1.0f);

            GL.CullFace(CullFaceMode.Front);
            GL.Enable(EnableCap.DepthTest);

            position = inPosition;
            viewDirection = inViewDirection;
            upDirection = inUpDirection;

            view = Matrix4.LookAt(position, viewDirection, upDirection);
            projection = Matrix4.CreateOrthographic(width, height, near, far);
        }

        public Matrix4 View
        {
            get { return view; }
            set { view = value; }
        }

        public Matrix4 Projection
        {
            get { return projection; }
            set { projection = value; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector3 Direction
        {
            get { return viewDirection; }
            set { viewDirection = value; }
        }

        public Vector3 UpDirection
        {
            get { return upDirection; }
            set { upDirection = value; }
        }

        public int FOV
        {
            get { return fov; }
            set { fov = value; }
        }

        public float AspectRatio
        {
            get { return aspectRatio; }
            set { aspectRatio = value; }
        }

        public float Near
        {
            get { return near; }
            set { near = value; }
        }

        public float Far
        {
            get { return far; }
            set { far = value; }
        }

        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        public void MoveCamera(Vector3 translation)
        {
            position += translation;
            viewDirection += translation;
            view = Matrix4.LookAt(position, viewDirection, upDirection);
        }
        public void RotateCamera(Vector3 rotation)
        {
            viewDirection = new Vector3(0, 0, 1) * Matrix3.CreateRotationX(rotation.X) * Matrix3.CreateRotationY(rotation.Y);
            view = Matrix4.LookAt(position, viewDirection, upDirection);
        }
    }
}
