using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.render
{
    /// <summary>
    /// A struc for 3d Model data
    /// </summary>
    public struct Mesh
    {
        public int[] V_indexs;
        public Vector3[] v_Vertices;
        public Vector2[] v_uvs;
        public Vector3[] v_normals;

        public Mesh(int[] v_indexs, Vector3[] v_Vertices, Vector2[] v_uvs)
        {
            V_indexs = v_indexs;
            this.v_Vertices = v_Vertices;
            this.v_uvs = v_uvs;
            v_normals = new Vector3[] { };
            CalculateNormals();
        }

        public Mesh(int[] v_indexs, Vector3[] v_Vertices, Vector2[] v_uvs, Vector3[] v_normals)
        {
            V_indexs = v_indexs;
            this.v_Vertices = v_Vertices;
            this.v_uvs = v_uvs;
            this.v_normals = v_normals;
        }

        private void CalculateNormals()
        {
            v_normals = new Vector3[] { };
        }

        private static Mesh GenCube()
        {
            List<int> indices = new List<int>();
            List<Vector3> vertice = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();

            return new Mesh(indices.ToArray(), vertice.ToArray(), uv.ToArray());
        }

        public static Mesh Empty { get { return new Mesh(new int[] { }, new Vector3[] { }, new Vector2[] { }, new Vector3[] { }); } }
        public static Mesh Cube { get { return GenCube(); } }
    }
}
