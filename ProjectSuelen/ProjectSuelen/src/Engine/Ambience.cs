using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace ProjectSuelen.src.Engine
{
    public class Ambience : ClassBase
    {
        private static Dictionary<string, Ambience> instancesList = new Dictionary<string, Ambience>();

        public bool Enabled = true;
        public float Density = 0.014f;
        public float Distance = 3.5f;
        /// <summary>
        /// the fog color use vector4 But is the same of the (OpenTK.Color4)
        /// </summary>
        public Color4 FogColor = new Color4(0, 0.7490196f, 1, 1);

        public Color4 SkyColor = Color4.DeepSkyBlue;
        public Color4 SkyHorizonColor = Color4.SkyBlue;

        public Color4 AmbienceColor = Color4.White;

        public Ambience(string spaceName)
        {
            instancesList = new Dictionary<string, Ambience>();
            instancesList.Add(spaceName, this);
        }

        protected override void OnDispose()
        {
            if (instancesList != null)
            {
                instancesList.Clear();
                instancesList = null;
            }
            base.OnDispose();
        }

        public static void GetFogColorFromViewDirection()
        {

        }

        public static Ambience GetEnvironment(string spaceName)
        {
            if (instancesList.TryGetValue(spaceName, out Ambience env))
            {
                return env;
            }
            return null;
        }
    }
}