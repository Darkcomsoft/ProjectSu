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
            v_normals = CalculateNormals();
        }

        public ChunkMesh(int[] v_indexs, Vector3[] v_Vertices, Vector2[] v_uvs, Vector3[] v_normals)
        {
            v_indices = v_indexs;
            this.v_Vertices = v_Vertices;
            this.v_uvs = v_uvs;
            this.v_normals = v_normals;
        }

        public Vector3[] CalculateNormals()
        {
            Vector3[] normals = new Vector3[v_Vertices.Length];
            int[] inds = v_indices;

            // Compute normals for each face
            for (int i = 0; i < inds.Length; i += 3)
            {
                Vector3 v1 = v_Vertices[inds[i]];
                Vector3 v2 = v_Vertices[inds[i + 1]];
                Vector3 v3 = v_Vertices[inds[i + 2]];

                // The normal is the cross product of two sides of the triangle
                normals[inds[i]] += Vector3.Cross(v2 - v1, v3 - v1);
                normals[inds[i + 1]] += Vector3.Cross(v2 - v1, v3 - v1);
                normals[inds[i + 2]] += Vector3.Cross(v2 - v1, v3 - v1);
            }

            for (int i = 0; i < v_Vertices.Length; i++)
            {
                normals[i] = normals[i].Normalized();
            }

            return normals;
        }

        public static ChunkMesh Empty { get { return new ChunkMesh(new int[] { }, new Vector3[] { }, new Vector2[] { }, new Vector3[] { }); } }
    }
}
