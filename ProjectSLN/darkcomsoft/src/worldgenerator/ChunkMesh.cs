using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.worldgenerator
{
    /// <summary>
    /// Mesh struct for Chunk 3d Mesh
    /// </summary>
    public struct ChunkMesh
    {
        public int[] v_indices;
        public Vector3[] v_Vertices;
        public Vector2[] v_uvs;
        public Vector3[] v_normals;

        public ChunkMesh(int[] v_indexs, Vector3[] v_Vertices, Vector2[] v_uvs)
        {
            v_indices = v_indexs;
            this.v_Vertices = v_Vertices;
            this.v_uvs = v_uvs;
            v_normals = new Vector3[] { };
            CalculateNormals();
        }

        public ChunkMesh(int[] v_indexs, Vector3[] v_Vertices, Vector2[] v_uvs, Vector3[] v_normals)
        {
            v_indices = v_indexs;
            this.v_Vertices = v_Vertices;
            this.v_uvs = v_uvs;
            this.v_normals = v_normals;
        }

        private void CalculateNormals()
        {
            v_normals = new Vector3[] { };
        }

        public static ChunkMesh Empty { get { return new ChunkMesh(new int[] { }, new Vector3[] { }, new Vector2[] { }, new Vector3[] { }); } }
    }
}
