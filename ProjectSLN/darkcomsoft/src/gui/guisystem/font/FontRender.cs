using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using ProjectSLN.darkcomsoft.src.gui.guisystem.guielements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ProjectSLN.darkcomsoft.src.render;
using ProjectSLN.darkcomsoft.src.enums;
using ProjectSLN.darkcomsoft.src.engine.window;
using ProjectSLN.darkcomsoft.src.debug;

namespace ProjectSLN.darkcomsoft.src.gui.guisystem.font
{
	public class FontRender : ClassBase
	{
		private string m_text = "";

		private float m_fontSizee;
		private float m_lineMaxSize;
		private int m_numberOfLines;

		private bool m_Ready;

		private TextAling m_TextAling = TextAling.Left;

		private Color4 m_color = Color4.White;
		private Font m_font;
		private GUIBase m_gUIBase;
		private TextMeshData m_textMeshData;

		private Shader m_shader;

		private Matrix4 m_WorldMatrix;

		private int VAO, vbo, ubo;

		public FontRender(string text, float fontSize, float maxLine, GUIBase gUIBase, Font font, Shader shader)
		{
			m_Ready = false;

			m_text = text;
			m_fontSizee = fontSize;
			m_gUIBase = gUIBase;
			m_lineMaxSize = maxLine;

			m_shader = shader;
			m_font = font;

			m_textMeshData = createTextMesh();

			SetUpBuffers();
			UpdateTransform();
		}

		protected override void OnDispose()
		{
			GL.BindVertexArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.DisableVertexAttribArray(0);

			GL.BindVertexArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.DisableVertexAttribArray(0);

			GL.DeleteBuffer(vbo);
			GL.DeleteBuffer(ubo);
			GL.DeleteVertexArray(VAO);

			m_textMeshData.Clear();

			m_shader = null;
			m_gUIBase = null;
			m_font = null;
			base.OnDispose();
		}

		public void SetText(string value)
        {
			m_text = value;
			Refresh();
		}

		public void SetTextPivot(TextAling textAling)
        {
			m_TextAling = textAling;
			Refresh();
		}

		public void SetColor(Color4 color)
        {
			m_color = color;
        }

		public void Draw()
        {
			if (m_Ready)
			{
				GL.Scissor((int)m_gUIBase.GetFinalPosition.X, (int)m_gUIBase.GetFinalPosition.Y, (int)m_gUIBase.GetFinalPosition.Width, (int)m_gUIBase.GetFinalPosition.Height);

				GL.Enable(EnableCap.ScissorTest);
				GL.Enable(EnableCap.Blend);
				GL.Disable(EnableCap.DepthTest);
				GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

				GL.BindVertexArray(VAO);
				GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

				m_shader.Use();
				m_font.AtlasTexture.Use();

				m_shader.Set("fontColor", m_color);
				m_shader.Set("world", m_WorldMatrix);
				m_shader.Set("projection", m_gUIBase.GetProjectionMatrix);

				GL.DrawArrays(PrimitiveType.Triangles, 0, m_textMeshData.getVertexLength());

				GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
				GL.BindVertexArray(0);

				GL.Enable(EnableCap.DepthTest);
				GL.Disable(EnableCap.Blend);
				GL.Disable(EnableCap.ScissorTest);
			}
		}

		public void OnResize()
        {
			Refresh();
		}

		private void Refresh()
        {
			m_Ready = false;

			m_textMeshData.Clear();
			m_textMeshData = createTextMesh();

			GL.BindVertexArray(VAO);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, m_textMeshData.getVertexLength() * Vector2.SizeInBytes, m_textMeshData.getVertexPositions(), BufferUsageHint.DynamicDraw);
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexAttribArray(0);

			GL.BindBuffer(BufferTarget.ArrayBuffer, ubo);
			GL.BufferData(BufferTarget.ArrayBuffer, m_textMeshData.getTextureCoordsLength() * Vector2.SizeInBytes, m_textMeshData.getTextureCoords(), BufferUsageHint.DynamicDraw);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexAttribArray(1);

			UpdateTransform();

			m_Ready = true;
		}

		private void SetUpBuffers()
		{
			VAO = GL.GenVertexArray();
			vbo = GL.GenBuffer();
			ubo = GL.GenBuffer();

			GL.BindVertexArray(VAO);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, m_textMeshData.getVertexLength() * Vector2.SizeInBytes, m_textMeshData.getVertexPositions(), BufferUsageHint.DynamicDraw);
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexAttribArray(0);

			GL.BindBuffer(BufferTarget.ArrayBuffer, ubo);
			GL.BufferData(BufferTarget.ArrayBuffer, m_textMeshData.getTextureCoordsLength() * Vector2.SizeInBytes, m_textMeshData.getTextureCoords(), BufferUsageHint.DynamicDraw);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexAttribArray(1);

			m_Ready = true;
		}

		private void UpdateTransform()
        {
			Vector2 textPosition = new Vector2();

            switch (m_TextAling)
            {
                case TextAling.Center:
					textPosition.X = 0;
					textPosition.Y = 0;
					break;
                case TextAling.Left:
					textPosition.X = -m_gUIBase.GetFinalPosition.Width / 2f;
					textPosition.Y = 0;
					break;
                case TextAling.Right:
					textPosition.X = m_gUIBase.GetFinalPosition.Width / 2f;
					textPosition.Y = 0;
					break;
                case TextAling.Top:
					textPosition.X = 0;
					textPosition.Y = -m_gUIBase.GetFinalPosition.Width / 2f;
					break;
                case TextAling.Bottom:
					textPosition.X = 0;
					textPosition.Y = m_gUIBase.GetFinalPosition.Width / 2f;
					break;
            }
            m_WorldMatrix = Matrix4.CreateScale(m_fontSizee * m_fontSizee) * Matrix4.CreateTranslation((m_gUIBase.GetFinalPosition.X + m_gUIBase.GetFinalPosition.Width / 2) + textPosition.X, (m_gUIBase.GetFinalPosition.Y + m_gUIBase.GetFinalPosition.Height / 2) + textPosition.Y, 0);
		}

		private TextMeshData createTextMesh()
		{
			List<Line> lines = createStructure();
			TextMeshData data = createQuadVertices(lines);
			return data;
		}

		private List<Line> createStructure()
		{
			char[] chars = m_text.ToCharArray();
			List<Line> lines = new List<Line>();
			Line currentLine = new Line(m_font.getSpaceWidth(), m_lineMaxSize);
			Word currentWord = new Word(0);

			for (int i = 0; i < chars.Length; i++)
			{
				int ascii = (int)chars[i];
				if (ascii == Font.SPACE_ASCII)
				{
					bool added = currentLine.attemptToAddWord(currentWord);
					if (!added)
					{
						lines.Add(currentLine);
						currentLine = new Line(m_font.getSpaceWidth(), m_lineMaxSize);
						currentLine.attemptToAddWord(currentWord);
					}
					currentWord = new Word(0);
					continue;
				}
				Character character = m_font.getCharacter(ascii);
				currentWord.addCharacter(character);
			}

			completeStructure(lines, currentLine, currentWord);
			return lines;
		}

		private void completeStructure(List<Line> lines, Line currentLine, Word currentWord)
		{
			bool added = currentLine.attemptToAddWord(currentWord);

			if (!added)
			{
				lines.Add(currentLine);
				currentLine = new Line(m_font.getSpaceWidth(), m_lineMaxSize);
				currentLine.attemptToAddWord(currentWord);
			}
			lines.Add(currentLine);
		}

		private TextMeshData createQuadVertices(List<Line> lines)
		{
			m_numberOfLines = lines.Count;
			float curserX = 0f;
			float curserY = 0f;

			List<Vector2> vertices = new List<Vector2>();
			List<Vector2> textureCoords = new List<Vector2>();

			foreach (var line in lines)
			{
				switch (m_TextAling)
				{
					case TextAling.Center:
						curserX = (m_font.getSpaceWidth()) - (line.getLineLength() / 2f);
						curserY = (Font.LINE_HEIGHT) / 2f;
						break;
					case TextAling.Left:
						curserX = (m_font.getSpaceWidth()) / 2f;
						curserY = (Font.LINE_HEIGHT) / 2f;
						break;
					case TextAling.Right:
						curserX = (m_font.getSpaceWidth()) - (line.getLineLength());
						curserY = (Font.LINE_HEIGHT) / 2f;
						break;
					default:
						curserX = (m_font.getSpaceWidth()) - (line.getLineLength() / 2f);
						curserY = (Font.LINE_HEIGHT) / 2f;
						break;
				}

				foreach (var word in line.getWords())
				{
					foreach (var letter in word.getCharacters())
					{
						addVerticesForCharacter(curserX, curserY, letter, vertices);
						addTexCoords(textureCoords, letter.getxTextureCoord(), letter.getyTextureCoord(), letter.getXMaxTextureCoord(), letter.getYMaxTextureCoord());
						curserX += letter.getxAdvance();
					}
					curserX += m_font.getSpaceWidth();
				}
				curserX = 0;
				curserY += Font.LINE_HEIGHT;
			}

			return new TextMeshData(vertices.ToArray(), textureCoords.ToArray());
		}

		private void addVerticesForCharacter(float curserX, float curserY, Character character, List<Vector2> vertices)
		{
			float x = curserX + (character.getxOffset());
			float y = curserY - (character.getyOffset());
			float maxX = x + (character.getSizeX());
			float maxY = y - (character.getSizeY());
			float properX = (2 * x);
			float properY = (-2 * y);
			float properMaxX = (2 * maxX);
			float properMaxY = (-2 * maxY);
			addVertices(vertices, properX, -properY, properMaxX, -properMaxY);
		}

		private void addVertices(List<Vector2> vertices, float x, float y, float maxX, float maxY)
		{
			vertices.Add(new Vector2((float)x, (float)y));
			vertices.Add(new Vector2((float)x, (float)maxY));
			vertices.Add(new Vector2((float)maxX, (float)maxY));
			vertices.Add(new Vector2((float)maxX, (float)maxY));
			vertices.Add(new Vector2((float)maxX, (float)y));
			vertices.Add(new Vector2((float)x, (float)y));
		}

		private void addTexCoords(List<Vector2> texCoords, float x, float y, float maxX, float maxY)
		{
			texCoords.Add(new Vector2((float)x, (float)y));
			texCoords.Add(new Vector2((float)x, (float)maxY));
			texCoords.Add(new Vector2((float)maxX, (float)maxY));
			texCoords.Add(new Vector2((float)maxX, (float)maxY));
			texCoords.Add(new Vector2((float)maxX, (float)y));
			texCoords.Add(new Vector2((float)x, (float)y));
		}
	}

	public struct Line
	{
		private float maxLength;
		private float spaceSize;

		private List<Word> words;
		private float currentLineLength;

		public Line(float spaceWidth, float maxLength)
		{
			words = new List<Word>();
			currentLineLength = 0;

			this.spaceSize = spaceWidth;
			this.maxLength = maxLength;
		}

		public bool attemptToAddWord(Word word)
		{
			float additionalLength = word.getWordWidth();

			if (words.Count == 0)
			{
				additionalLength += 0;
			}
			else
			{
				additionalLength += spaceSize;
			}

			if (currentLineLength + additionalLength <= maxLength)
			{
				words.Add(word);
				currentLineLength += additionalLength;
				return true;
			}
			else
			{
				return false;
			}
		}

		public float getMaxLength()
		{
			return maxLength;
		}

		public float getLineLength()
		{
			return currentLineLength;
		}

		public List<Word> getWords()
		{
			return words;
		}
	}

	public struct Word
	{
		private List<Character> characters;
		private float width;

		public Word(float w)
		{
			width = w;
			characters = new List<Character>();
		}

		public void addCharacter(Character character)
		{
			characters.Add(character);
			width += character.getxAdvance();
		}

		public List<Character> getCharacters()
		{
			return characters;
		}

		public float getWordWidth()
		{
			return width;
		}
	}

	public struct TextMeshData
	{
		private Vector2[] vertexPositions;
		private Vector2[] textureCoords;

		public TextMeshData(Vector2[] vertexPositions, Vector2[] textureCoords)
		{
			this.vertexPositions = vertexPositions;
			this.textureCoords = textureCoords;
		}

		public void Clear()
        {
			vertexPositions = null;
			vertexPositions = null;
		}

		public Vector2[] getVertexPositions()
		{
			return vertexPositions;
		}

		public Vector2[] getTextureCoords()
		{
			return textureCoords;
		}

		public int getVertexCount()
		{
			return vertexPositions.Length / 2;
		}

		public int getVertexLength()
		{
			return vertexPositions.Length;
		}

		public int getTextureCoordsLength()
		{
			return textureCoords.Length;
		}
	}

	public enum TextAling : byte
	{
		Center, Left, Right, Top, Bottom
	}
}