using Projectsln.darkcomsoft.src.engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Projectsln.darkcomsoft.src.resources.resourcestype
{
    /// <summary>
    /// Load a custom GLSL file, dglsl is a file group of vertex shader and fragment shader
    /// </summary>
    public static class GLSLLoad
    {
        public static ShaderFile LoadShaderFile(string path, string filename)
        {
            string filePath = string.Concat(Application.AssetsPath, path, filename, ".dglsl");

            if (!File.Exists(filePath))
            {
                Debug.LogError("dglsl Files Can't be found At: " + filePath, "RESOURCES-MANAGER");
                throw new Exception("dglsl Files Can't be found At: " + filePath);
            }

            string[] lines = File.ReadAllLines(filePath);
            List<string> vertexLines = new List<string>();
            List<string> fragmentLines = new List<string>();

            string finalVertex = "";
            string finalFragment = "";

            string shaderVersion = "";

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("version:"))
                {
                    string[] versionline = lines[i].Split(":"[0]);
                    shaderVersion = versionline[1];
                }

                if (lines[i].Contains("vertex ["))
                {
                    while (true)
                    {
                        i++;
                        if (lines[i].Contains("];") || lines[i].Contains("fragment ["))
                        {
                            break;
                        }
                        else
                        {
                            vertexLines.Add(lines[i]);
                        }
                    }
                }

                if (lines[i].Contains("fragment ["))
                {
                    while (true)
                    {
                        i++;
                        if (lines[i].Contains("];") || lines[i].Contains("vertex ["))
                        {
                            break;
                        }
                        else
                        {
                            fragmentLines.Add(lines[i]);
                        }
                    }
                }
            }

            for (int i = 0; i < vertexLines.Count; i++)
            {
                if (vertexLines[i].Contains("$version"))
                {
                    finalVertex += shaderVersion + "\n";
                }
                else
                {
                    finalVertex += vertexLines[i] + "\n";
                }
            }

            for (int i = 0; i < fragmentLines.Count; i++)
            {
                if (fragmentLines[i].Contains("$version"))
                {
                    finalFragment += shaderVersion + "\n";
                }
                else
                {
                    finalFragment += fragmentLines[i] + "\n";
                }
            }

            return new ShaderFile(finalVertex, finalFragment);
        }
    }
}
