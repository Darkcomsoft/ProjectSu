﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ProjectSu.src.Engine
{
    public class Camera : ClassBase
    {
        private static Camera Main;

        private GameObject gameObject;
        public Vector3 position;
        public Quaternion rotation;

        private double _fildOfView = 75;
        private double _nearPlane = 0.1f;
        private double _farPlane = 1000;

        private double _aspectRatio;
        private Vector3 finalTarget;

        private Matrix4 ViewMatrix;
        private Matrix4 ProjectionMatrix;

        public Camera(GameObject obj, float camHight)
        {
            Main = this;

            gameObject = obj;

            position = new Vector3(0, camHight, 0);
            rotation = Quaternion.Identity;

            UpdateProjection();
        }

        public void Tick()
        {
            var camRotation = rotation + new Quaternion((float)gameObject.transform.Rotation.X, (float)gameObject.transform.Rotation.Y, (float)gameObject.transform.Rotation.Z, (float)gameObject.transform.Rotation.W); 

            var camOriginalTarget = new Vector3d(0, 0, 1);
            var camRotatedTarget = Vector3.Transform((Vector3)camOriginalTarget, camRotation);
            finalTarget = position + camRotatedTarget;

            var camOriginalUpVector = new Vector3(0, 1, 0);
            var camRotatedUpVector = Vector3.Transform(camOriginalUpVector, camRotation);

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

        /// <summary>
        /// (DONT WORKING RIGHT NOW)Get view matrix with-out float precision fix
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
            _aspectRatio = (double)Window.Instance.Width / (double)Window.Instance.Height;
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
