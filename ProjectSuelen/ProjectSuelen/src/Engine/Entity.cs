using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Engine
{
    public class Entity : ObjectBase
    {
        private Transform Transform;

        public Entity()
        {
            OnCreate();
            Transform = new Transform();
            Start();
        }

        protected override void OnDispose()
        {
            OnDestroy();

            Transform.Dispose();
            base.OnDispose();
        }

        protected virtual void Start()
        {

        }

        protected virtual void OnDestroy()
        {

        }

        protected virtual void OnCreate()
        {

        }

        public Transform transform { get { return Transform; } }
    }
}
