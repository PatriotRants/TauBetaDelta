using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

using ForgeWorks.RailThorn.Fonts;
using ForgeWorks.RailThorn.Mathematics;

using ForgeWorks.TauBetaDelta.Logging;
using ForgeWorks.TauBetaDelta.Collections;

namespace ForgeWorks.RailThorn.Controls;

public abstract class TextRenderer : RendererControl
{
    private ICharacterSet characterMap;
    private float width;
    private float height;
    private Matrix4 projection;
    private int _vao;
    private int _vbo;

    public string Text { get; set; } = string.Empty;
    public Color4 Color { get; set; } = Color4.Black;
    public Font Font { get; set; } = "LiberationMono-Regular"; //   default font
    public float Scale { get; set; } = 0.5f;

    protected TextRenderer(IViewContainer container, string name) : base(name)
    {
        width = container.Width;
        height = container.Height;

        Shader = SHADERS.LoadShader("label.shaders");
    }

    public override void Init()
    {
        //  get the font's character map
        characterMap = Font.CharacterSet;

        //  configure everything for rendering Text
        GL.Enable(EnableCap.Blend);
        // GL.Enable(EnableCap.CullFace);
        // GL.Enable(EnableCap.Texture2D);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        //  CreateOrthographicOffCenter(float left, float right, float bottom, float top, float depthNear, float depthFar)
        projection = Matrix4.CreateOrthographicOffCenter(0.0f, width, 0.0f, height, 0.1f, 1.0f);
        LOGGER.Post(LogLevel.GLDebug, $"[{nameof(TextRenderer)}.{nameof(Init)}] Projection:\n{{{projection}}}");

        Shader.SetMatrix4("projection", projection);

        //  create VAO & VBO for texture quads
        GL.GenVertexArrays(1, out _vao);
        GL.GenBuffers(1, out _vbo);
        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 4 * 6, nint.Zero, BufferUsageHint.DynamicDraw);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }
    public override void Update()
    {
        //  if font changes
        if (characterMap?.Name != Font.Name)
        {
            characterMap = Font.CharacterSet;
        }

        //  activate corresponding render state
        Shader.SetVector3("textColor", (Color.R, Color.G, Color.B));
        GL.ActiveTexture(TextureUnit.Texture0);
        //  bind VAO
        GL.BindVertexArray(_vao);
    }
    public override void Render()
    {
        Shader.Use();

        // iterate character map
        var text = Text.ToArray();
        var ndx = 0;
        var x = (float)Location.X;
        var y = (float)Location.Y;

        while (ndx < text.Length)
        {
            Vertex[] vertices = MapText(text[ndx], ref x, ref y, out int textureId);
            LOGGER.Post(LogLevel.GLDebug, $"[MapText] ({text[ndx]})(tex:{textureId}){vertices.ToString(vertices.Length)}");

            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferSubData(BufferTarget.ArrayBuffer, 0, sizeof(float) * 4 * 6, vertices);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            ++ndx;
        }
    }

    private Vertex[] MapText(char c, ref float x, ref float y, out int textureId)
    {
        Font.Character character = characterMap[c];
        textureId = character.TextureId;
        var map = character.GetMapping();

        float xpos = x + map.xpos * Scale;
        float ypos = y - map.ypos * Scale;

        float w = map.w * Scale;
        float h = map.h * Scale;

        Vertex[] vertices = new Vertex[6]{
            ((xpos,     ypos + h),   (0.0f, 0.0f)),
            ((xpos,     ypos),       (0.0f, 1.0f)),
            ((xpos + w, ypos),       (1.0f, 1.0f)),
            ((xpos,     ypos + h),   (0.0f, 0.0f)),
            ((xpos + w, ypos),       (1.0f, 1.0f)),
            ((xpos + w, ypos + h),   (1.0f, 0.0f))
        };

        /*         
            // now advance cursors for next glyph (note that advance is number of 1/64 pixels)
            x += (ch.Advance >> 6) * scale; // bitshift by 6 to get value in pixels (2^6 = 64 (divide amount of 1/64th pixels by 64 to get amount of pixels))

            ** was applying conversion here but we apply pixel scaling in CharacterTexMap
         */
        x += character.Advance;

        return vertices;
    }
}