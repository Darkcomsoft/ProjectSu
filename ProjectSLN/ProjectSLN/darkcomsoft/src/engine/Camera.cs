using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using Projectsln.darkcomsoft.src.engine.window;
using Projectsln.darkcomsoft.src.entity;

namespace Projectsln.darkcomsoft.src.engine
{
    public class Camera : ClassBase //MAYBE THIS NEED A CODE CLEAN-UP TO THE NEW SYSTEMS
    {
        private static Camera Main;

        private Entity entityBase;
        public Vector3 position;
        public Quaternion rotation;

        private double _fildOfView = 65;
        private double _nearPlane = 0.1f;
        private double _farPlane = 1000;

        private double _aspectRatio;
        private Vector3 finalTarget;

        private Matrix4 ViewMatrix;
        private Matrix4 ProjectionMatrix;

        public Camera(Entity entity, float camHight)
        {
            Main = this;

            entityBase = entity;

            position = new Vector3(0, camHight, 0);
            rotation = Quaternion.Identity;

            UpdateProjection();
        }

        public void Tick()
        {
            var camRotation = Matrix3.CreateRotationX((float)rotation.X + (float)entityBase.transform.Rotation.X) * Matrix3.CreateRotationY((float)rotation.Y + (float)entityBase.transform.Rotation.Y) * Matrix3.CreateRotationZ((float)rotation.Z + (float)entityBase.transform.Rotation.Z);

            var camOriginalTarget = new Vector3d(0, 0, 1);
            var camRotatedTarget = Vector3.Transform((Vector3)camOriginalTarget, camRotation.ExtractRotation());
            finalTarget = position + camRotatedTarget;

            var camOriginalUpVector = new Vector3(0, 1, 0);
            var camRotatedUpVector = Vector3.Transform(camOriginalUpVector, camRotation.ExtractRotation());

            UpdateProjection();

            ViewMatrix = Matrix4.LookAt(position, finalTarget, camRotatedUpVector);
        }

        /// <summary>
        /// Get view matrix with float precision fix
        /// </summary>
        /// <returns></returns>
        public Matrix4 GetViewMatrix()
        {
            return ViewMatrix;
        }

        /// <summary>
        /// Get the projection matrix
        /// </summary>
        /// <returns></returns>
        public Matrix4 GetProjectionMatrix()
        {
            return ProjectionMatrix;
        }

        //FIX THIS FUNCTION, WHY, I DONT KNOW,  MAYBE IN THE NEAR FUTURE SOMEONE NEED THIS
        /// <summary>
        /// Get view matrix with-out float precision fix
        /// </summary>
        /// <returns></returns>
        public Matrix4 GetWorldViewMatrix()
        {
            /*var camRotation = Matrix3.CreateRotationX((float)rotation.X + (float)gameObject.transform.Rotation.X) * Matrix3.CreateRotationY((float)rotation.Y + (float)gameObject.transform.Rotation.Y) * Matrix3.CreateRotationZ((float)rotation.Z + (float)gameObject.transform.Rotation.Z);

            var camOriginalTarget = new Vector3d(0, 0, 1);
            var camRotatedTarget = Vector3.Transform((Vector3)camOriginalTarget, camRotation);
            finalTarget = new Vector3(position + (Vector3)gameObject.transform.Position) + camRotatedTarget;

            var camOriginalUpVector = new Vector3(0, 1, 0);
            var camRotatedUpVector = Vector3.Transform(camOriginalUpVector, camRotation);

            return Matrix4.LookAt(position + (Vector3)gameObject.transform.Position, finalTarget, camRotatedUpVector);*/
            return Matrix4.Identity;
        }

        private void UpdateProjection()
        {
            _aspectRatio = (double)WindowClass.Instance.Width / (double)WindowClass.Instance.Height;
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians((float)_fildOfView), (float)_aspectRatio, (float)_nearPlane, (float)_farPlane);
        }

        public void UpdateFOV(float newfov)
        {
            _fildOfView = newfov;
            UpdateProjection();
        }

        public void UpdateNearPlane(float newUpdateNearPlane)
        {
            _nearPlane = newUpdateNearPlane;
            UpdateProjection();
        }

        public void UpdateFarPlane(float newUpdateFarPlane)
        {
            _farPlane = newUpdateFarPlane;
            UpdateProjection();
        }

        protected override void OnDispose()
        {
            if (Main == this)
            {
                Main = null;
            }
            entityBase = null;
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
