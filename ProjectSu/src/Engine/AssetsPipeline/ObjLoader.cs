using ObjParser;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Engine.AssetsPipeline
{
    public class ObjLoader
    {
        private List<int> triangles;
        private List<Vector3> vertices;
        private List<Vector2> uv;
        private List<Vector3> normals;
        private List<Vector3Int> faceData;
        private List<int> intArray;

        private const int MIN_POW_10 = -16;
        private const int MAX_POW_10 = 16;
        private const int NUM_POWS_10 = MAX_POW_10 - MIN_POW_10 + 1;
        private static readonly float[] pow10 = GenerateLookupTable();

        // Use this for initialization
        public Mesh ImportFile(string path, string filename)
        {
            string filePath = string.Concat(Application.AssetsPath, path, filename, ".obj");

            if (!File.Exists(filePath))
            {
                Debug.LogError("Obj Files Can't be found At: " + filePath);
                throw new Exception("Obj Files Can't be found At: " + filePath);
            }

            triangles = new List<int>();
            vertices = new List<Vector3>();
            uv = new List<Vector2>();
            normals = new List<Vector3>();
            faceData = new List<Vector3Int>();
            intArray = new List<int>();

            LoadMeshData(filePath);

            Vector3[] newVerts = new Vector3[faceData.Count];
            Vector2[] newUVs = new Vector2[faceData.Count];
            Vector3[] newNormals = new Vector3[faceData.Count];

            /* The following foreach loops through the facedata and assigns the appropriate vertex, uv, or normal
             * for the appropriate Unity mesh array.
             */
            for (int i = 0; i < faceData.Count; i++)
            {
                newVerts[i] = vertices[faceData[i].x - 1];
                if (faceData[i].y >= 1)
                    newUVs[i] = uv[faceData[i].y - 1];

                if (faceData[i].z >= 1)
                    newNormals[i] = normals[faceData[i].z - 1];
            }

            Mesh mesh = new Mesh();

            mesh._vertices = newVerts;
            mesh._texCoords = newUVs;
            mesh._Normals = newNormals;
            mesh._indices = triangles.ToArray();
            mesh._Colors = new Color4[] { };

            return mesh;
        }

        private void LoadMeshData(string fileName)
        {

            StringBuilder sb = new StringBuilder();
            string text = File.ReadAllText(fileName);
            int start = 0;
            string objectName = null;
            int faceDataCount = 0;

            StringBuilder sbFloat = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    sb.Remove(0, sb.Length);

                    // Start +1 for whitespace '\n'
                    sb.Append(text, start + 1, i - start);
                    start = i;

                    if (sb[0] == 'o' && sb[1] == ' ')
                    {
                        sbFloat.Remove(0, sbFloat.Length);
                        int j = 2;
                        while (j < sb.Length)
                        {
                            objectName += sb[j];
                            j++;
                        }
                    }
                    else if (sb[0] == 'v' && sb[1] == ' ') // Vertices
                    {
                        int splitStart = 2;

                        vertices.Add(new Vector3(GetFloat(sb, ref splitStart, ref sbFloat),
                            GetFloat(sb, ref splitStart, ref sbFloat), GetFloat(sb, ref splitStart, ref sbFloat)));
                    }
                    else if (sb[0] == 'v' && sb[1] == 't' && sb[2] == ' ') // UV
                    {
                        int splitStart = 3;

                        uv.Add(new Vector2(GetFloat(sb, ref splitStart, ref sbFloat),
                            GetFloat(sb, ref splitStart, ref sbFloat)));
                    }
                    else if (sb[0] == 'v' && sb[1] == 'n' && sb[2] == ' ') // Normals
                    {
                        int splitStart = 3;

                        normals.Add(new Vector3(GetFloat(sb, ref splitStart, ref sbFloat),
                            GetFloat(sb, ref splitStart, ref sbFloat), GetFloat(sb, ref splitStart, ref sbFloat)));
                    }
                    else if (sb[0] == 'f' && sb[1] == ' ')
                    {
                        int splitStart = 2;

                        int j = 1;
                        intArray.Clear();
                        int info = 0;
                        // Add faceData, a face can contain multiple triangles, facedata is stored in following order vert, uv, normal. If uv or normal are / set it to a 0
                        while (splitStart < sb.Length && char.IsDigit(sb[splitStart]))
                        {
                            faceData.Add(new Vector3Int(GetInt(sb, ref splitStart, ref sbFloat),
                                GetInt(sb, ref splitStart, ref sbFloat), GetInt(sb, ref splitStart, ref sbFloat)));
                            j++;

                            intArray.Add(faceDataCount);
                            faceDataCount++;
                        }

                        info += j;
                        j = 1;
                        while (j + 2 < info) //Create triangles out of the face data.  There will generally be more than 1 triangle per face.
                        {
                            triangles.Add(intArray[0]);
                            triangles.Add(intArray[j]);
                            triangles.Add(intArray[j + 1]);

                            j++;
                        }
                    }
                }
            }
        }

        private float GetFloat(StringBuilder sb, ref int start, ref StringBuilder sbFloat)
        {
            sbFloat.Remove(0, sbFloat.Length);
            while (start < sb.Length &&
                   (char.IsDigit(sb[start]) || sb[start] == '-' || sb[start] == '.'))
            {
                sbFloat.Append(sb[start]);
                start++;
            }
            start++;

            return ParseFloat(sbFloat);
        }

        private int GetInt(StringBuilder sb, ref int start, ref StringBuilder sbInt)
        {
            sbInt.Remove(0, sbInt.Length);
            while (start < sb.Length &&
                   (char.IsDigit(sb[start])))
            {
                sbInt.Append(sb[start]);
                start++;
            }
            start++;

            return IntParseFast(sbInt);
        }


        private static float[] GenerateLookupTable()
        {
            var result = new float[(-MIN_POW_10 + MAX_POW_10) * 10];
            for (int i = 0; i < result.Length; i++)
                result[i] = (float)((i / NUM_POWS_10) *
                        Math.Pow(10, i % NUM_POWS_10 + MIN_POW_10));
            return result;
        }

        private float ParseFloat(StringBuilder value)
        {
            float result = 0;
            bool negate = false;
            int len = value.Length;
            int decimalIndex = value.Length;
            for (int i = len - 1; i >= 0; i--)
                if (value[i] == '.')
                { decimalIndex = i; break; }
            int offset = -MIN_POW_10 + decimalIndex;
            for (int i = 0; i < decimalIndex; i++)
                if (i != decimalIndex && value[i] != '-')
                    result += pow10[(value[i] - '0') * NUM_POWS_10 + offset - i - 1];
                else if (value[i] == '-')
                    negate = true;
            for (int i = decimalIndex + 1; i < len; i++)
                if (i != decimalIndex)
                    result += pow10[(value[i] - '0') * NUM_POWS_10 + offset - i];
            if (negate)
                result = -result;
            return result;
        }

        private int IntParseFast(StringBuilder value)
        {
            // An optimized int parse method.
            int result = 0;
            for (int i = 0; i < value.Length; i++)
            {
                result = 10 * result + (value[i] - 48);
            }
            return result;
        }

        public static Mesh LoadModel(string path, string filename)
        {
            string finalpath = string.Concat(Application.AssetsPath, path, filename, ".obj");

            if (!File.Exists(finalpath))
            {
                Debug.LogError("Obj Files Can't be found At: " + finalpath);
                throw new Exception("Obj Files Can't be found At: " + finalpath);
            }

            string[] fileData = File.ReadAllLines(finalpath);

            List<Vector3> vertices = new List<Vector3>();
            List<int> indices = new List<int>();
            List<Vector2> textures = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            float[] verticesArray;
            float[] textureArray = null;
            float[] normalsArray = null;
            int[] indicesArray = null;

            try
            {
                for (int i = 0; i < fileData.Length; i++)
                {
                    string[] line = fileData[i].Split(" "[0]);

                    if (fileData[i].StartsWith("v "))
                    {
                        Vector3 vertex = new Vector3(float.Parse(line[1]), float.Parse(line[2]), float.Parse(line[3]));
                        vertices.Add(vertex);
                    }
                    else if (fileData[i].StartsWith("vt "))
                    {
                        Vector2 textureCor = new Vector2(float.Parse(line[1]), float.Parse(line[2]));
                        textures.Add(textureCor);
                    }
                    else if (fileData[i].StartsWith("vn "))
                    {
                        Vector3 normal = new Vector3(float.Parse(line[1]), float.Parse(line[2]), float.Parse(line[3]));
                        normals.Add(normal);
                    }
                    else if (fileData[i].StartsWith("f "))
                    {
                        textureArray = new float[vertices.Count * 2];
                        normalsArray = new float[vertices.Count * 3];
                    }
                }

                for (int i = 0; i < fileData.Length; i++)
                {
                    if (fileData[i].StartsWith("f "))
                    {
                        string[] line = fileData[i].Split(" "[0]);
                        string[] vertex1 = line[1].Split("/"[0]);
                        string[] vertex2 = line[2].Split("/"[0]);
                        string[] vertex3 = line[3].Split("/"[0]);


                        processVertex(vertex1, indices, textures, normals, textureArray, normalsArray);
                        processVertex(vertex2, indices, textures, normals, textureArray, normalsArray);
                        processVertex(vertex3, indices, textures, normals, textureArray, normalsArray);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            verticesArray = new float[vertices.Count * 3];
            indicesArray = new int[indices.Count];

            List<Vector3> vet = new List<Vector3>();
            List<Vector2> text = new List<Vector2>();
            List<Vector3> norm = new List<Vector3>();

            for (int i = 0; i < textureArray.Length; i += 2)
            {
                text.Add(new Vector2(textureArray[i], textureArray[i + 1]));
            }

            return new Mesh(vertices.ToArray(), normals.ToArray(), text.ToArray(), new Color4[] { }, indices.ToArray());
        }

        private static void processVertex(string[] vertexData, List<int> indices, List<Vector2> textures, List<Vector3> normals, float[] textureArray, float[] normalsArray)
        {
            int currentVertexPointer = int.Parse(vertexData[0]) - 1;
            indices.Add(currentVertexPointer);
            Vector2 currentTex = textures[int.Parse(vertexData[1]) - 1];
            textureArray[currentVertexPointer * 2] = currentTex.X;
            textureArray[currentVertexPointer * 2 + 1] = currentTex.Y;
            Vector3 currentNorm = normals[int.Parse(vertexData[2]) - 1];
            normalsArray[currentVertexPointer * 3] = currentNorm.X;
            normalsArray[currentVertexPointer * 3 + 1] = currentNorm.Y;
            normalsArray[currentVertexPointer * 3 + 2] = currentNorm.Z;
        }
    }
}

public sealed class Vector3Int
{
    public int x { get; set; }
    public int y { get; set; }
    public int z { get; set; }

    public Vector3Int() { }

    public Vector3Int(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}