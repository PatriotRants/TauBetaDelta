using OpenTK.Graphics.OpenGL4;

using ForgeWorks.RailThorn.Fonts;
using ForgeWorks.RailThorn.Graphics;
using ForgeWorks.RailThorn.Controls;
using OpenTK.Mathematics;

namespace ForgeWorks.TauBetaDelta;

public class Text
{
    protected static readonly ShaderManager SHADERS = ShaderManager.Instance;

    private const uint DEFAULT_PIXEL_HEIGHT = 32;

    private readonly IViewContainer container;

    private ICharacterSet characterSet;
    private string content = string.Empty;
    private Font font = FontManager.Instance.Default;
    private int vao;
    private int vbo;

    public event Action ContentChanged;
    public event Action FontChanged;

    private Shader Shader { get; set; }

    public string Content
    {
        get => new(content);
        set => ChangeContent(value);
    }
    public Font Font
    {
        get => font;
        set => ChangeFont(value);
    }

    public Text(IViewContainer viewContainer)
    {
        container = viewContainer;
    }

    public void Init()
    {
        /* **
            Configure GL
        ** */
        // set 1 byte pixel alignment 
        GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
        // set texture unit
        GL.ActiveTexture(TextureUnit.Texture0);

        //  load font
        Font.Height = DEFAULT_PIXEL_HEIGHT;
        characterSet = Font.CharacterSet;
        //  load shader
        Shader = SHADERS.LoadShader("text.shaders");
        Shader.Use();

        //  bind default texture
        GL.BindTexture(TextureTarget.Texture2D, 0);
        //  set default (4 byte) pixel alignment 
        GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

        //  configure vquad
        float[] vquad =
        {
            //  x      y     u     v    
                0.0f, -1.0f, 0.0f, 0.0f,
                0.0f,  0.0f, 0.0f, 1.0f,
                1.0f,  0.0f, 1.0f, 1.0f,
                0.0f, -1.0f, 0.0f, 0.0f,
                1.0f,  0.0f, 1.0f, 1.0f,
                1.0f, -1.0f, 1.0f, 0.0f
        };

        //  create vbo
        vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, 4 * 6 * 4, vquad, BufferUsageHint.StaticDraw);
        //  create vao
        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * 4, 0);
        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * 4, 2 * 4);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }
    public void Update()
    {
        Matrix4 projectionM = Matrix4.CreateScale(new Vector3(1f / container.Width, 1.0f / container.Height, 1.0f));
        projectionM = Matrix4.CreateOrthographicOffCenter(0.0f, container.Width, container.Height, 0.0f, -1.0f, 1.0f);

        GL.Viewport(0, 0, (int)container.Width, (int)container.Height);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.Enable(EnableCap.Blend);
        //GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        GL.BlendFunc(0, BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

        Shader.Use();
        GL.UniformMatrix4(1, false, ref projectionM);

        GL.Uniform3(2, new Vector3(0.5f, 0.8f, 0.2f));

        // GL.Uniform3(2, new Vector3(0.3f, 0.7f, 0.9f));
        // _font.RenderText("(C) LearnOpenGL.com", 50.0f, 200.0f, 0.9f, new Vector2(1.0f, -0.25f));
    }
    public void Render()
    {
        RenderText(25.0f, 50.0f, 1.2f, new Vector2(1f, 0f));
    }

    private void ChangeContent(string textContent)
    {
        if (!string.Equals(content, textContent))
        {
            content = textContent;
            ContentChanged?.Invoke();
        }
    }
    private void ChangeFont(Font bodyFont)
    {
        FontChanged?.Invoke();
    }
    private void RenderText(float x, float y, float scale, Vector2 dir)
    {
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindVertexArray(vao);

        //  calculate rotation translation matrix
        float angle_rad = (float)Math.Atan2(dir.Y, dir.X);
        Matrix4 rotateM = Matrix4.CreateRotationZ(angle_rad);
        Matrix4 transOriginM = Matrix4.CreateTranslation(new Vector3(x, y, 0f));

        //  iterate text content
        int ndx = 0;
        float char_x = 0.0f;
        while (ndx < content.Length)
        {
            //  TODO: add safe character get
            var ch = characterSet[content[ndx]];
            float w = ch.Size.X * scale;
            float h = ch.Size.Y * scale;
            float xRel = char_x + ch.Bearing.X * scale;
            float yRel = (ch.Size.Y - ch.Bearing.Y) * scale;

            //  advance cursor for next glyph; 1/64 pixels
            char_x += (ch.Advance >> 6) * scale;

            Matrix4 scaleM = Matrix4.CreateScale(w, h, 1.0f);
            Matrix4 transRelM = Matrix4.CreateTranslation(xRel, yRel, 0.0f);

            Matrix4 modelM = scaleM * transRelM * rotateM * transOriginM;
            GL.UniformMatrix4(0, false, ref modelM);

            //  render glyph texture over quad
            GL.BindTexture(TextureTarget.Texture2D, ch.TextureId);

            //  redner quad
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            ++ndx;
        }
    }
}
