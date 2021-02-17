﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Engine.UI
{
    /// <summary>
    /// this is a 2D Rectangle, to use in GUI System
    /// </summary>
    public class RectangleMesh : ClassBase
    {
        public Vector2d[] _vertices;
        public int[] _indices;

        public RectangleMesh()
        {
            /*Vertices_Positions = new double[] 
            { 
                -1,  1, 
                -1, -1, 
                 1,  1, 
                 1, -1 
            };*/

            _vertices = new Vector2d[]
            {
                 //Position          Texture coordinates
                 new Vector2d(1,  1), // top right
                 new Vector2d(1, -1), // bottom right
                new Vector2d(-1, -1), // bottom left
                new Vector2d(-1,  1) // top left
            };

            _indices = new int[]
            {
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
            };
        }

        public void UpdateMesh(Rectangle rec)
        {
            _vertices = new Vector2d[]
            {
                 //Position          Texture coordinates
                 new Vector2d(1,  1), // top right
                 new Vector2d(1, -1), // bottom right
                 new Vector2d(-1, -1), // bottom left
                 new Vector2d(-1,  1) // top left
            };
        }

        protected override void OnDispose()
        {
            _vertices = null;
            _indices = null;
            base.OnDispose();
        }
    }
}