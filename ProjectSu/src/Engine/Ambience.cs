using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace ProjectSu.src.Engine
{
    public class Ambience : ClassBase
    {
        private static Dictionary<string, Ambience> instancesList = new Dictionary<string, Ambience>();

        private SkySphere skySphere;

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

        private string spacenname;

        public Ambience(string spaceName)
        {
            spacenname = spaceName;
            instancesList.Add(spaceName, this);

            skySphere = new SkySphere(this, "Sky");
        }

        protected override void OnDispose()
        {
            skySphere?.Dispose();
            skySphere = null;

            instancesList?.Remove(spacenname);
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