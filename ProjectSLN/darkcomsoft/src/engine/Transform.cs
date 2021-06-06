using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;

namespace ProjectIND.darkcomsoft.src.engine
{
    public class Transform : ClassBase
    {
        public Vector3d v_Position;
        public Quaterniond v_Rotation;
        public Vector3d v_Size;

        /// <summary>
        /// this is the boundbox, you can calculate with the 3d model, or set a custom value
        /// </summary>
        private Vector3d v_volumeSize;

        private Matrix4 v_transformMatrix;


        /// <summary>
        /// [REMOVER ASSIM QUE POSSIVEL]This is the global player position, used to do the fix impresission of the float.
        /// </summary>
        public static Vector3d PlayerRootPosition;//remover isso assim que possivel, e por na classe do player.

        public Transform()
        {
            v_Position = Vector3d.Zero;
            v_Rotation = Quaterniond.Identity;
            v_Size = Vector3d.One;

            v_volumeSize = Vector3d.One;

            UpdateWorldMatrix();
        }
        public Transform(Vector3d position, Quaterniond rotation, Vector3d size, Vector3d volumeSize)
        {
            v_Position = position;
            v_Rotation = rotation;
            v_Size = size;
            v_volumeSize = volumeSize;

            UpdateWorldMatrix();
        }
        public Transform(Vector3d position, Quaterniond rotation, Vector3d size)
        {
            v_Position = position;
            v_Rotation = rotation;
            v_Size = size;
            v_volumeSize = Vector3d.One;

            UpdateWorldMatrix();
        }
        public Transform(Vector3d position, Quaterniond rotation)
        {
            v_Position = position;
            v_Rotation = rotation;
            v_Size = Vector3d.One;
            v_volumeSize = Vector3d.One;

            UpdateWorldMatrix();
        }
        public Transform(Vector3d position)
        {
            v_Position = position;
            v_Rotation = Quaterniond.Identity;
            v_Size = Vector3d.One;
            v_volumeSize = Vector3d.One;

            UpdateWorldMatrix();
        }

        private void UpdateWorldMatrix()
        {
            v_transformMatrix = Matrix4.CreateScale((Vector3)v_Size) * Matrix4.CreateRotationX((float)v_Rotation.X) * Matrix4.CreateRotationY((float)v_Rotation.Y) * Matrix4.CreateRotationZ((float)v_Rotation.Z) * Matrix4.CreateTranslation((Vector3)v_Position - (Vector3)PlayerRootPosition);
        }

        public void Tick()
        {
            UpdateWorldMatrix();
        }

        public Matrix4 GetTransformWorld
        {
            get
            {
                return v_transformMatrix;
            }
        }

        /// <summary>
        /// this is the boundbox, you can calculate with the 3d model, or set a custom value
        /// </summary>
        public Vector3d VolumeSize
        {
            get
            {
                return v_volumeSize;
            }
            set { v_volumeSize = value; }
        }

        public Vector3d Forward { get { return (new Vector3d(v_Rotation.X, v_Rotation.Y, v_Rotation.Z) * Vector3d.UnitZ) * v_Size.Z; } }
        public Vector3d Up { get { return (new Vector3d(v_Rotation.X, v_Rotation.Y, v_Rotation.Z) * Vector3d.UnitY) * v_Size.Y; } }
        public Vector3d Right { get { return (new Vector3d(v_Rotation.X, v_Rotation.Y, v_Rotation.Z) * Vector3d.UnitX) * v_Size.X; } }
    }
}
