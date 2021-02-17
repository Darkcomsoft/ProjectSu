﻿using System;
using OpenTK;
using OpenTK.Graphics;

namespace ProjectSu.src.Engine.AssetsPipeline
{
    public struct Mesh
    {
        public Vector3[] _vertices;
        public int[] _indices;
        public Vector2[] _texCoords;
        public Color4[] _Colors;
        public Vector3[] _Normals;

        public Mesh(Mesh newmesh)
        {
            _vertices = newmesh._vertices;
            _indices = newmesh._indices;
            _texCoords = newmesh._texCoords;
            _Colors = newmesh._Colors;
            _Normals = newmesh._Normals;
        }

        public Mesh(object obj)
        {
            _vertices = new Vector3[]
            {
                 //Position          Texture coordinates
                 new Vector3(0.5f,  0.5f, 0.0f), // top right
                 new Vector3(0.5f, -0.5f, 0.0f), // bottom right
                new Vector3(-0.5f, -0.5f, 0.0f), // bottom left
                new Vector3(-0.5f,  0.5f, 0.0f) // top left
            };

            _indices = new int[]
            {
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
            };

            _Colors = new Color4[] {
                new Color4(1.0f,1.0f,1.0f,1.0f),
                new Color4(1.0f,1.0f,1.0f,1.0f),
                new Color4(1.0f,1.0f,1.0f,1.0f)
            };

            _texCoords = new Vector2[] {
               new Vector2( 1.0f, 1.0f),
                new Vector2(1.0f, 0.0f),
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 1.0f)
            };

            _Normals = new Vector3[] { };
            _Normals = CalculateNormals();

            OptimizeMesh();
        }

        public Mesh(Vector3[] vertices, int[] indices, Color4[] colors)
        {
            _vertices = vertices;
            _indices = indices;
            _Colors = colors;

            _texCoords = new Vector2[] { };


            _Normals = null;
            _Normals = CalculateNormals();

            OptimizeMesh();
        }
        public Mesh(Vector3[] vertices, int[] indices)
        {
            _vertices = vertices;
            _indices = indices;

            _texCoords = new Vector2[] { };
            _Colors = new Color4[] { };

            _Normals = null;
            _Normals = CalculateNormals();

            OptimizeMesh();
        }

        public Mesh(Vector3[] vertices, Vector2[] textures, Color4[] colors, int[] indices)
        {
            _vertices = vertices;
            _indices = indices;
            _texCoords = textures;
            _Colors = colors;

            _Normals = null;
            _Normals = CalculateNormals();

            OptimizeMesh();
        }

        public Mesh(Vector3[] vertices, Vector3[] normals, Vector2[] textures, Color4[] colors, int[] indices)
        {
            _vertices = vertices;
            _indices = indices;
            _texCoords = textures;
            _Colors = colors;

            _Normals = normals;
            //_Normals = CalculateNormals();

            OptimizeMesh();
        }

        public void OptimizeMesh()
        {
            /*List<Vector3d> vertTo = new List<Vector3d>();
            List<uint> indTo = new List<uint>();

            List<double> finalvert = new List<double>();
            List<int> finalindice = new List<int>();

            for (int i = 0; i < _vertices.Length; i += 3)
            {
                vertTo.Add(new Vector3d(_vertices[i], _vertices[i + 1], _vertices[i + 2]));
            }

            for (int i = 0; i < _indices.Length; i++)
            {
                indTo.Add((uint)_indices[i]);
            }

             Tuple<Vector3d[], uint[]> u = MeshOptimizer.MeshOperations.Optimize<Vector3d>(vertTo.ToArray(), indTo.ToArray(), sizeof(double) * 3);

            for (int i = 0; i < u.Item1.Length; i++)
            {
                finalvert.Add(u.Item1[i].X);
                finalvert.Add(u.Item1[i].Y);
                finalvert.Add(u.Item1[i].Z);
            }

            for (int i = 0; i < u.Item2.Length; i++)
            {
                finalindice.Add((int)u.Item2[i]);
            }

            _vertices = finalvert.ToArray();
            _indices = finalindice.ToArray();*/
        }

        public Vector3[] CalculateNormals()
        {
            Vector3[] normals = new Vector3[_vertices.Length];
            int[] inds = _indices;

            // Compute normals for each face
            for (int i = 0; i < inds.Length; i += 3)
            {
                Vector3 v1 = _vertices[inds[i]];
                Vector3 v2 = _vertices[inds[i + 1]];
                Vector3 v3 = _vertices[inds[i + 2]];

                // The normal is the cross product of two sides of the triangle
                normals[inds[i]] += Vector3.Cross(v2 - v1, v3 - v1);
                normals[inds[i + 1]] += Vector3.Cross(v2 - v1, v3 - v1);
                normals[inds[i + 2]] += Vector3.Cross(v2 - v1, v3 - v1);
            }

            for (int i = 0; i < _vertices.Length; i++)
            {
                normals[i] = normals[i].Normalized();
            }

            return normals;
        }

        public void ClearMesh()
        {
            _vertices = new Vector3[] { };
            _indices = new int[] { };
            _texCoords = new Vector2[] { };
            _Colors = new Color4[] { };
            _Normals = new Vector3[] { };
        }
    }
}
