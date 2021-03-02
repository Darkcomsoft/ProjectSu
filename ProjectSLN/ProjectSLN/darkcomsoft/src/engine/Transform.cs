﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;

namespace Projectsln.darkcomsoft.src.engine
{
    public class Transform : ClassBase
    {
        /// <summary>
        /// [REMOVER ASSIM QUE POSSIVEL]This is the global player position.
        /// </summary>
        public static Vector3d PlayerRootPosition;//remover isso assim que possivel, e por na classe do player.

        private Vector3d _Position;
        private Quaterniond _Rotation;
        private Vector3d _Size;

        private Matrix4 transformMatrix;

        public Transform()
        {
            _Position = new Vector3d(0, 0, 0);
            _Rotation = Quaterniond.Identity;
            _Size = new Vector3d(1, 1, 1);

            UpdateWorldMatrix();
        }
        public Transform(Vector3d position, Quaterniond rotation, Vector3d size)
        {
            _Position = position;
            _Rotation = rotation;
            _Size = size;

            UpdateWorldMatrix();
        }
        public Transform(Vector3d position, Quaterniond rotation)
        {
            _Position = position;
            _Rotation = rotation;
            _Size = Vector3d.One;

            UpdateWorldMatrix();
        }
        public Transform(Vector3d position)
        {
            _Position = position;
            _Rotation = Quaterniond.Identity;
            _Size = Vector3d.One;

            UpdateWorldMatrix();
        }

        private void UpdateWorldMatrix()
        {
            transformMatrix = Matrix4.CreateScale((Vector3)_Size) * Matrix4.CreateRotationX((float)_Rotation.X) * Matrix4.CreateRotationY((float)_Rotation.Y) * Matrix4.CreateRotationZ((float)_Rotation.Z) * Matrix4.CreateTranslation((Vector3)_Position - (Vector3)PlayerRootPosition);
        }

        public void Tick()
        {
            UpdateWorldMatrix();
        }

        public Matrix4 GetTransformWorld
        {
            get
            {
                return transformMatrix;
            }
        }

        public Vector3d Position
        {
            get
            {
                return _Position;
            }
            set { _Position = value; }
        }
        public Quaterniond Rotation
        {
            get
            {
                return _Rotation;
            }
            set { _Rotation = value; }
        }
        public Vector3d Size
        {
            get
            {
                return _Size;
            }
            set { _Size = value; }
        }

        public Vector3d Forwards { get { return (new Vector3d(_Rotation.X, _Rotation.Y, _Rotation.Z) * Vector3d.UnitZ) * _Size.Z; } }
        public Vector3d Up { get { return (new Vector3d(_Rotation.X, _Rotation.Y, _Rotation.Z) * Vector3d.UnitY) * _Size.Y; } }
        public Vector3d Right { get { return (new Vector3d(_Rotation.X, _Rotation.Y, _Rotation.Z) * Vector3d.UnitX) * _Size.X; } }
    }
}
