using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL4;

namespace ProjectSuelen.src.Engine
{
    public class Camera : ClassBase
    {
        private static Camera Main;

        private GameObject gameObject;
        public Vector3 position;
        public Quaternion rotation;

        public double _fildOfView = 75;
        public double _nearPlane = 0.1f;
        public double _farPlane = 1000;

        private double _aspectRatio;
        private Vector3 finalTarget;

        public Camera(GameObject obj, float camHight)
        {
            Main = this;

            gameObject = obj;

            position = new Vector3(0, camHight, 0);
            rotation = Quaternion.Identity;

            _aspectRatio = (double)Window.Instance.Width / (double)Window.Instance.Height;
        }
        /// <summary>
        /// Get view matrix with float precision fix
        /// </summary>
        /// <returns></returns>
        public Matrix4 GetViewMatrix()
        {
            var camRotation = Matrix3.CreateRotationX((float)rotation.X + (float)gameObject.transform.Rotation.X) * Matrix3.CreateRotationY((float)rotation.Y + (float)gameObject.transform.Rotation.Y) * Matrix3.CreateRotationZ((float)rotation.Z + (float)gameObject.transform.Rotation.Z);

            var camOriginalTarget = new Vector3d(0, 0, 1);
            var camRotatedTarget = Vector3.Transform((Vector3)camOriginalTarget, camRotation);
            finalTarget = new Vector3(Vector3.Zero) + camRotatedTarget;

            var camOriginalUpVector = new Vector3(0, 1, 0);
            var camRotatedUpVector = Vector3.Transform(camOriginalUpVector, camRotation);

            return Matrix4.LookAt(Vector3.Zero, finalTarget, camRotatedUpVector);
        }

        /// <summary>
        /// Get the projection matrix
        /// </summary>
        /// <returns></returns>
        public Matrix4 GetProjectionMatrix()
        {
            _aspectRatio = (double)Window.Instance.Width / (double)Window.Instance.Height;
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians((float)_fildOfView), (float)_aspectRatio, (float)_nearPlane, (float)_farPlane);
        }

        /// <summary>
        /// Get view matrix with-out float precision fix
        /// </summary>
        /// <returns></returns>
        public Matrix4 GetWorldViewMatrix()
        {
            var camRotation = Matrix3.CreateRotationX((float)rotation.X + (float)gameObject.transform.Rotation.X) * Matrix3.CreateRotationY((float)rotation.Y + (float)gameObject.transform.Rotation.Y) * Matrix3.CreateRotationZ((float)rotation.Z + (float)gameObject.transform.Rotation.Z);

            var camOriginalTarget = new Vector3d(0, 0, 1);
            var camRotatedTarget = Vector3.Transform((Vector3)camOriginalTarget, camRotation);
            finalTarget = new Vector3(position + (Vector3)gameObject.transform.Position) + camRotatedTarget;

            var camOriginalUpVector = new Vector3(0, 1, 0);
            var camRotatedUpVector = Vector3.Transform(camOriginalUpVector, camRotation);

            return Matrix4.LookAt(position + (Vector3)gameObject.transform.Position, finalTarget, camRotatedUpVector);
        }

        protected override void OnDispose()
        {
            if (Main == this)
            {
                Main = null;
            }
            gameObject = null;
            base.OnDispose();
        }

        public static double WrapAngle(double angle)
        {
            if ((angle > -MathHelper.Pi) && (angle <= MathHelper.Pi))
                return angle;
            angle %= MathHelper.TwoPi;
            if (angle <= -MathHelper.Pi)
                return angle + MathHelper.TwoPi;
            if (angle > MathHelper.Pi)
                return angle - MathHelper.TwoPi;
            return angle;
        }

        public static Camera main { get { return Main; } private set { } }
    }
}
