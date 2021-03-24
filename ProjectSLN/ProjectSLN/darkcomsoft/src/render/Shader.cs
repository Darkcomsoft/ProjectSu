using Projectsln.darkcomsoft.src.resources.resourcestype;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;
using Projectsln.darkcomsoft.src.engine;
using OpenTK.Mathematics;

namespace Projectsln.darkcomsoft.src.render
{
    /// <summary>
    /// OpenGL Shader, complier and binder
    /// </summary>
    public class Shader : ClassBase
    {
        private int m_shaderHandler;
        private bool m_isvalid = false;

        private Dictionary<string, int> _uniformLocations;

        public Shader(ShaderFile fileShader)
        {
            _uniformLocations = new Dictionary<string, int>();
            CompileShader(fileShader);
        }

        private void CompileShader(ShaderFile fileShader)
        {
            int vert_shader = GL.CreateShader(ShaderType.VertexShader);
            int frag_shader = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(vert_shader, fileShader._vertshader);
            GL.ShaderSource(frag_shader, fileShader._fragshader);

            GL.CompileShader(vert_shader);
            GL.CompileShader(frag_shader);

            int program = GL.CreateProgram();
            GL.AttachShader(program, vert_shader);
            GL.AttachShader(program, frag_shader);
            GL.LinkProgram(program);

            int status;
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out status);
            if (status == 0)
            {
                Debug.LogError(String.Format("Error linking program: {0}", GL.GetProgramInfoLog(program)), "SHADER");
                GL.DeleteProgram(program);
            }

            GL.DeleteShader(vert_shader);
            GL.DeleteShader(frag_shader);

            GL.GetProgram(program, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            for (var i = 0; i < numberOfUniforms; i++)
            {
                var key = GL.GetActiveUniform(program, i, out _, out _);
                var location = GL.GetUniformLocation(program, key);
                _uniformLocations.Add(key, location);
            }

            GL.ValidateProgram(program);
            var glerror = GL.GetError();
            if (glerror != ErrorCode.NoError)
            {
                Debug.LogError("Failed to create program : " + glerror, "SHADER");
                GL.DeleteProgram(program);
            }
            m_shaderHandler = program;
            m_isvalid = true;
        }

        public void Use()
        {
            if (m_isvalid)
            {
                GL.UseProgram(m_shaderHandler);
            }
        }

        protected override void OnDispose()
        {
            if (m_isvalid)
            {
                GL.DeleteProgram(m_shaderHandler);
                m_isvalid = false;
            }

            m_shaderHandler = 0;
            _uniformLocations.Clear();
            _uniformLocations = null;

            base.OnDispose();
        }

        public void Set(string name, int value)
        {
            Use();
            GL.Uniform1(_uniformLocations[name], value);
        }

        public void Set(string name, float value)
        {
            Use();
            GL.Uniform1(_uniformLocations[name], value);
        }

        public void Set(string name, bool value)//REVER ISSO AQUI NAO SEI SE GLSL TEM BOOLEAN ENTAO USA INT, SE NAO TIVER VER SE DA PAARA MUDAR PARA BYTE
        {
            Use();
            GL.Uniform1(_uniformLocations[name], value ? 1 : 0);
        }

        public void Set(string name, Color4 value)
        {
            Use();
            GL.Uniform4(_uniformLocations[name], value);
        }

        public void Set(string name, Vector3 value)
        {
            Use();
            GL.Uniform3(_uniformLocations[name], value);
        }

        public void Set(string name, Vector4 value)
        {
            Use();
            GL.Uniform4(_uniformLocations[name], value);
        }

        public void Set(string name, Matrix4 value, bool transpose = true)
        {
            Use();
            GL.UniformMatrix4(_uniformLocations[name], transpose, ref value);
        }
    }
}