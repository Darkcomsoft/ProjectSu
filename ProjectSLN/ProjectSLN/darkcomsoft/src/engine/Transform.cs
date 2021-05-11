using System;
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
        private Vector3d m_Position;
        private Quaterniond m_Rotation;
        private Vector3d m_Size;

        /// <summary>
        /// this is the boundbox, you can calculate with the 3d model, or set a custom value
        /// </summary>
        private Vector3d m_volumeSize;

        private Matrix4 m_transformMatrix;


        /// <summary>
        /// [REMOVER ASSIM QUE POSSIVEL]This is the global player position, used to do the fix impresission of the float.
        /// </summary>
        public static Vector3d PlayerRootPosition;//remover isso assim que possivel, e por na classe do player.

        public Transform()
        {
            m_Position = Vector3d.Zero;
            m_Rotation = Quaterniond.Identity;
            m_Size = Vector3d.One;

            m_volumeSize = Vector3d.One;

            UpdateWorldMatrix();
        }
        public Transform(Vector3d position, Quaterniond rotation, Vector3d size, Vector3d volumeSize)
        {
            m_Position = position;
            m_Rotation = rotation;
            m_Size = size;
            m_volumeSize = volumeSize;

            UpdateWorldMatrix();
        }
        public Transform(Vector3d position, Quaterniond rotation, Vector3d size)
        {
            m_Position = position;
            m_Rotation = rotation;
            m_Size = size;
            m_volumeSize = Vector3d.One;

            UpdateWorldMatrix();
        }
        public Transform(Vector3d position, Quaterniond rotation)
        {
            m_Position = position;
            m_Rotation = rotation;
            m_Size = Vector3d.One;
            m_volumeSize = Vector3d.One;

            UpdateWorldMatrix();
        }
        public Transform(Vector3d position)
        {
            m_Position = position;
            m_Rotation = Quaterniond.Identity;
            m_Size = Vector3d.One;
            m_volumeSize = Vector3d.One;

            UpdateWorldMatrix();
        }

        private void UpdateWorldMatrix()
        {
            m_transformMatrix = Matrix4.CreateScale((Vector3)m_Size) * Matrix4.CreateRotationX((float)m_Rotation.X) * Matrix4.CreateRotationY((float)m_Rotation.Y) * Matrix4.CreateRotationZ((float)m_Rotation.Z) * Matrix4.CreateTranslation((Vector3)m_Position - (Vector3)PlayerRootPosition);
        }

        public void Tick()
        {
            UpdateWorldMatrix();
        }

        public Matrix4 GetTransformWorld
        {
            get
            {
                return m_transformMatrix;
            }
        }

        public Vector3d Position
        {
            get
            {
                return m_Position;
            }
            set { m_Position = value; }
        }
        public Quaterniond Rotation
        {
            get
            {
                return m_Rotation;
            }
            set { m_Rotation = value; }
        }
        public Vector3d Size
        {
            get
            {
                return m_Size;
            }
            set { m_Size = value; }
        }
        /// <summary>
        /// this is the boundbox, you can calculate with the 3d model, or set a custom value
        /// </summary>
        public Vector3d VolumeSize
        {
            get
            {
                return m_volumeSize;
            }
            set { m_volumeSize = value; }
        }

        public Vector3d Forward { get { return (new Vector3d(m_Rotation.X, m_Rotation.Y, m_Rotation.Z) * Vector3d.UnitZ) * m_Size.Z; } }
        public Vector3d Up { get { return (new Vector3d(m_Rotation.X, m_Rotation.Y, m_Rotation.Z) * Vector3d.UnitY) * m_Size.Y; } }
        public Vector3d Right { get { return (new Vector3d(m_Rotation.X, m_Rotation.Y, m_Rotation.Z) * Vector3d.UnitX) * m_Size.X; } }
    }
}
