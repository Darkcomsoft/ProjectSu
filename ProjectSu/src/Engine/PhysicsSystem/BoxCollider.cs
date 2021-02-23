using BEPUphysics.Entities.Prefabs;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSu.src.Engine.PhysicsSystem
{
    public class BoxCollider : ClassBase
    {
        private Box BoxHandler;
        public Vector3d _Size;

        public BoxCollider(Transform ptransform)
        {
            _Size = new Vector3d(1, 1, 1);

            BoxHandler = new Box(new BEPUutilities.Vector3(ptransform.Position.X, ptransform.Position.Y, ptransform.Position.Z), _Size.X, _Size.Y, _Size.Z);

            Physics.Add(BoxHandler);
        }

        public BoxCollider(Transform ptransform, Vector3d Size)
        {
            _Size = Size;

            BoxHandler = new Box(new BEPUutilities.Vector3(ptransform.Position.X, ptransform.Position.Y, ptransform.Position.Z), _Size.X, _Size.Y, _Size.Z);

            Physics.Add(BoxHandler);
        }

        public void OnDestroy()
        {
            Physics.Remove(BoxHandler);
            BoxHandler = null;
        }
    }
}
