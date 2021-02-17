using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectSu;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace ProjectSu.src.Engine.Sound
{
    /// <summary>
    /// AudioListener is the listener off audio system, Recomended use with Camera
    /// </summary>
    public class AudioListener : ClassBase
    {
        public AudioListener()
        {
            AL.Listener(ALListener3f.Position, 0.0f, 0.0f, 0.0f);
        }

        public AudioListener(Vector3d defaultPosition)
        {
            AL.Listener(ALListener3f.Position, (float)defaultPosition.X, (float)defaultPosition.Y, (float)defaultPosition.Z);
        }

        public void UpdatePosition(Vector3 position)
        {
            AL.Listener(ALListener3f.Position, position.X, position.Y, position.Z);
            //AL.Listener(ALListener3f.Velocity, 1, 1, 1);
        }

        public void UpdatePosition(Vector3d position)
        {
            AL.Listener(ALListener3f.Position, (float)position.X, (float)position.Y, (float)position.Z);
            //AL.Listener(ALListener3f.Velocity, 1, 1, 1);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
