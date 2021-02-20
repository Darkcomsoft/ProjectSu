﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;

namespace ProjectSu.src.Engine
{
    public class Transform : ClassBase
    {
        /// <summary>
        /// This is the global player position.
        /// </summary>
        public static Vector3d PlayerRootPosition;

        private Vector3d _Position;
        private Quaterniond _Rotation;
        private Vector3d _Size;

        public Transform Parent;
        public List<Transform> Childs = new List<Transform>();

        public Transform()
        {
            _Position = new Vector3d(0, 0, 0);
            _Rotation = Quaterniond.Identity;
            _Size = new Vector3d(1, 1, 1);
        }
        public Transform(Vector3d position, Quaterniond rotation, Vector3d size)
        {
            _Position = position;
            _Rotation = rotation;
            _Size = size;
        }
        public Transform(Vector3d position, Quaterniond rotation)
        {
            _Position = position;
            _Rotation = rotation;
            _Size = Vector3d.One;
        }
        public Transform(Vector3d position)
        {
            _Position = position;
            _Rotation = Quaterniond.Identity;
            _Size = Vector3d.One;
        }

        public Transform(Transform parent)
        {
            _Position = new Vector3d(0, 0, 0);
            _Rotation = Quaterniond.Identity;
            _Size = new Vector3d(1, 1, 1);

            Parent = parent;
            OnAddToAprent();
        }
        public Transform(Vector3d position, Quaterniond rotation, Vector3d size, Transform parent)
        {
            _Position = position;
            _Rotation = rotation;
            _Size = size;

            Parent = parent;
            OnAddToAprent();
        }
        public Transform(Vector3d position, Quaterniond rotation, Transform parent)
        {
            _Position = position;
            _Rotation = rotation;
            _Size = Vector3d.One;

            Parent = parent;
            OnAddToAprent();
        }
        public Transform(Vector3d position, Transform parent)
        {
            _Position = position;
            _Rotation = Quaterniond.Identity;
            _Size = Vector3d.One;

            Parent = parent;
            OnAddToAprent();
        }

        private void OnAddToAprent()
        {
            Parent.AddChild(Parent);
        }

        protected override void OnDispose()
        {
            if (Parent != null)
            {
                Parent.RemoveChild(this);
            }
            base.OnDispose();
        }

        public Matrix4 PositionMatrix
        {
            get
            {
                if (Parent != null)
                {
                    return Matrix4.CreateTranslation(((Vector3)_Position + (Vector3)Parent.Position) - (Vector3)PlayerRootPosition);
                }
                else
                {
                    return Matrix4.CreateTranslation((Vector3)_Position - (Vector3)PlayerRootPosition);
                }
            }
        }
        public Matrix4 RotationMatrix
        {
            get
            {
                if (Parent != null)
                {
                    return Matrix4.CreateRotationX((float)_Rotation.X + (float)Parent.Rotation.X) * Matrix4.CreateRotationY((float)_Rotation.Y + (float)Parent.Rotation.Y) * Matrix4.CreateRotationZ((float)_Rotation.Z + (float)Parent.Rotation.Z);
                }
                else
                {
                    return Matrix4.CreateRotationX((float)_Rotation.X) * Matrix4.CreateRotationY((float)_Rotation.Y) * Matrix4.CreateRotationZ((float)_Rotation.Z);
                }
            }
        }
        public Matrix4 GetTransformWorld
        {
            get
            {
                if (Parent != null)
                {
                    return Matrix4.CreateScale((Vector3)_Size + (Vector3)Parent.Size) * RotationMatrix * PositionMatrix;
                }
                else
                {
                    return Matrix4.CreateScale((Vector3)_Size) * RotationMatrix * PositionMatrix;
                }
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

        public void AddChild(Transform child)
        {
            Childs.Add(child);
        }

        public void RemoveChild(Transform child)
        {
            Childs.Remove(child);
        }

        public Vector3d Forwards { get { return (new Vector3d(_Rotation.X, _Rotation.Y, _Rotation.Z) * Vector3d.UnitZ) * _Size.Z; } }
        public Vector3d Up { get { return (new Vector3d(_Rotation.X, _Rotation.Y, _Rotation.Z) * Vector3d.UnitY) * _Size.Y; } }
        public Vector3d Right { get { return (new Vector3d(_Rotation.X, _Rotation.Y, _Rotation.Z) * Vector3d.UnitX) * _Size.X; } }
    }
}